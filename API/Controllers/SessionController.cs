using DataAccess.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace IotCoreDemo.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [Produces("application/json")]
    public class SessionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SessionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("/users/{id}/sessions")]
        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            var sessionList = await _unitOfWork.Sessions.GetAllAsync(id);
            return Ok(sessionList);
        }
    }
}
