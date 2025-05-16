using APBD_Kolos_1.Model.DTOs;
using APBD_Kolos_1.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Kolos_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitController : ControllerBase
    {
        private readonly IVisitService _visitService;

        public VisitController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVisit(int id)
        {
            if (!await _visitService.DoesVisitExist(id))
            {
                return NotFound();
            }
            var visit = await _visitService.GetVisit(id);
            return Ok(visit);
        }

        [HttpPost]
        public async Task<IActionResult> addVisit(NewVisitDTO newVisit)
        {
            if (! await _visitService.DoesClientExist(newVisit.client_id))
            {
                return NotFound();
            }

            foreach (var service in newVisit.visits)
            {
                if (!await _visitService.DoesServiceExist(service.service.service_id))
                {
                    return NotFound();
                }
            }

            if (! await _visitService.DoesMechanicExist(newVisit.mechanic_id))
            {
                return NotFound();
            }
            await _visitService.CreateVisit(newVisit);
            return Created(Request.Path.Value?? "api/visit", newVisit);
        }
    }
}
