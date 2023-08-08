using DataAccess.Repository.Abstract;
using Core.Models;
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
    public class CabinetController : Controller
    {
        private readonly MqttClient _client;
        private readonly IUnitOfWork _unitOfWork;
        public CabinetController(MqttClient client, IUnitOfWork unitOfWork)
        {
            _client = client;
            _unitOfWork = unitOfWork;
        }



        [Route("cabinets/{id}")]
        [HttpPost]
        public async Task<IActionResult> ExecuteCommandAsync(string id, [FromBody] CommandRequest request)
        {
            //OBTENER EL MAC ADDRESS ANTES DE ENVIAR EL REQUEST
            //AGREGAR MENSAJES AL BADREQUEST
            switch (request.commad)
            {
                case 1:
                    var transaction = await OpenAsync(id, request);
                    if(!transaction)
                        return BadRequest();
                    else 
                        return Ok();

                default: return BadRequest();
            }
        }

        private async Task<bool> OpenAsync(string id, CommandRequest request)
        {
            var slotCount = await _unitOfWork.Cabinets.CountSlotsAsync(id);
            if (slotCount == 0)
                return false;

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
                return false;
            
            var timeStamp = DateTime.Now;
            var order = $"ORD-{Guid.NewGuid()}";
            var command = new SlotCommadModel()
            {
                slot_id = nextSlot,
                slot_info = $"0x0{request.commad}",
                order_number = order,
                timestamp = timeStamp
            };
            string topic = $"/stations/open_slot/{id}";
            _client.Publish(topic, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command)));

            var transactionResult = await _unitOfWork.Slots.OpenAsync(id, nextSlot, timeStamp);
            if (!transactionResult)
                return false;

            var sessionStart = await _unitOfWork.Sessions.StartAsync(request.user_uuid, id, nextSlot);
            if (!sessionStart)
                return false;
            _ = await _unitOfWork.Logs.AddAsync(id, request.user_uuid, nextSlot, order);

            return true;
        }

        [Route("cabinets")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] CabinetRequest request)
        {
            if (request == null)
                return BadRequest("Invalid Request");

            var transactionResult = await _unitOfWork.Cabinets.CreateAsync(request.uuid, 
                request.mac_addr, request.name, request.location);
            if (!transactionResult)
                return BadRequest(new { message = "An error ocurred creating the cabinet." });

            for (int i = 1; i <= request.slot_number; i++)
                _ = await _unitOfWork.Slots.CreateAsync(request.uuid, i);

            return StatusCode(StatusCodes.Status201Created, new { message = "Cabinet created correctly." });
        }

        [Route("cabinets")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var cabinetList = await _unitOfWork.Cabinets.GetAllAsync();
            return Ok(cabinetList);
        }

        [Route("cabinets/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var cabinet = await _unitOfWork.Cabinets.GetByIdAsync(id);
            if (cabinet == null)
                return NotFound();

            return Ok(cabinet);
        }
    }
}
