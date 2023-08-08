using DataAccess.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;
using uPLibrary.Networking.M2Mqtt;

namespace IotCoreDemo.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [Produces("application/json")]
    public class LogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public LogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("/logs")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var logList = await _unitOfWork.Logs.GetAllAsync();
            return Ok(logList);
        }
    }
}
