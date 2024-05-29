using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PruebaTecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly OrganizationService _organizationService;

        public OrganizationsController(OrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet, Route("list")]
        public IActionResult ListOfOrganizations()
        {
            var orgs = _organizationService.GetList();
            return Ok(orgs);
        }

        [HttpPost,Route("Create")]
        public async Task<IActionResult> CreateOrganizationAsync([FromBody] OrganizationDto organizationCreateDto)
        {
            await _organizationService.CreateOrganization(organizationCreateDto);
            return Ok();
        }
    }
}
