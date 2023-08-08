using Core.Models;
using DataAccess.Repository.Abstract;
using IotCoreDemo.Requests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace IotCoreDemo.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [Produces("application/json")]
    public class ActionController : Controller
    {
        private readonly MqttClient _client;
        private readonly IUnitOfWork _unitOfWork;
        public ActionController(MqttClient client, IUnitOfWork unitOfWork)
        {
            _client = client;
            _unitOfWork = unitOfWork;
        }

        [Route("cabinets/{id}/slots/{slotId}/stop")]
        [HttpPost]
        public async Task<IActionResult> FinishAsync(int slotId, string id, [FromBody] CommandRequest request)
        {
            var selectedSlot = await _unitOfWork.Slots.GetByIdAsync(id, slotId);
            if (selectedSlot == null)
                return NotFound();

            var orderNumber = await _unitOfWork.Logs.GetActiveAsync(id, request.user_uuid, slotId);
            if (String.IsNullOrEmpty(orderNumber))
                return BadRequest();

            var timeStamp = DateTime.Now;
            var order = $"ORD-{Guid.NewGuid()}";
            var command = new SlotCommadModel()
            {
                slot_id = slotId,
                slot_info = $"0x04",
                order_number = order,
                timestamp = timeStamp
            };
            string topic = $"/stations/open_slot/{id}";
            _client.Publish(topic, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command)));

            Thread.Sleep(5000);
            command = new SlotCommadModel()
            {
                slot_id = slotId,
                slot_info = $"0x02",
                order_number = order,
                timestamp = timeStamp
            };
            topic = $"/stations/open_slot/{id}";
            _client.Publish(topic, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command)));

            var startingTime = await _unitOfWork.Sessions.GetStartSessionAsync(request.user_uuid, id, slotId);
            var sessionTransactionResult = await _unitOfWork.Sessions.EndAsync(request.user_uuid, id, slotId);

            var logTransactionResult = await _unitOfWork.Logs.CloseAsync(orderNumber, timeStamp);
            var dateDiff = timeStamp.Subtract(startingTime);
            var timing = (new DateTime() + dateDiff);
            _ = await _unitOfWork.Slots.CloseAsync(id, slotId, timeStamp);
            return Ok(new { message = $"Chargin Duration {timing.Hour}:{timing.Minute}" });
        }

        [Route("cabinets/{id}/slots/{slotId}/failure")]
        [HttpPost]
        public async Task<IActionResult> ErrorAsync(int slotId, string id)
        {
            var timeStamp = DateTime.Now;
            var failureTransactionResult = await _unitOfWork.Slots.ReportErrorAsync(id, slotId, timeStamp);
            if (!failureTransactionResult)
                return BadRequest();

            return Ok();
        }

        [Route("cabinets/{id}/open")]
        [HttpPost]
        public async Task<IActionResult> OpenAsync(string id, [FromBody] CommandRequest request)
        {
            //OBTENER EL MAC ADDRESS ANTES DE ENVIAR EL REQUEST
            var slotCount = await _unitOfWork.Cabinets.CountSlotsAsync(id);
            if (slotCount == 0)
                return NotFound(new { message = "Invalid Cabinet" });

            var nextSlot = await _unitOfWork.Cabinets.CheckLastSlotOpenedAsync(id);
            bool valid = false;
            for (int i = 1; i <= slotCount; i++)
            {

                var check = await _unitOfWork.Slots.CheckAvailabilityAsync(id, nextSlot);
                if (check)
                {
                    valid = true;
                    break;
                }
                if (nextSlot == slotCount)
                    nextSlot = 1;
                else
                    nextSlot++;
            }

            if (!valid)
                return BadRequest(new { message = "There are no available slots right now"});

            var timeStamp = DateTime.Now;
            var order = $"ORD-{Guid.NewGuid()}";
            var command = new SlotCommadModel()
            {
                slot_id = nextSlot,
                slot_info = $"0x01",
                order_number = order,
                timestamp = timeStamp
            };
            string topic = $"/stations/open_slot/{id}";
            _client.Publish(topic, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command)));

            var transactionResult = await _unitOfWork.Slots.OpenAsync(id, nextSlot, timeStamp);
            if (!transactionResult)
                return BadRequest(new { message = "An error ocurred registering the open command" });

            var sessionStart = await _unitOfWork.Sessions.StartAsync(request.user_uuid, id, nextSlot);
            if (!sessionStart)
                return BadRequest(new { message = "An error ocurred registering the open command" });
            _ = await _unitOfWork.Logs.AddAsync(id, request.user_uuid, nextSlot, order);

            return Ok();

        }
    }
}
