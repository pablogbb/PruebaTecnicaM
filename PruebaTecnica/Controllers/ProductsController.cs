using Application.Products.Add;
using Application.Products.List;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace PruebaTecnica.Controllers
{
    [Route("api/{slugTenant}/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        IMediator Mediator { get; }

        public ProductsController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductAsync([FromBody] AddProductCommand request)
        {
            await Mediator.Send(request);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            var emptyQuery = new ListProductsQuery();
            var products = await Mediator.Send(emptyQuery);
            return Ok(products);
        }
    }
}
