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
        private readonly IUnitOfWork _unitOfWork;
        public CabinetController(MqttClient client, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
