using Microsoft.AspNetCore.Mvc;
using NetSchool.Services.CardCollections;
using NetSchool.Services.CardCollections.CardCollections;
using NetSchool.Services.Logger;

namespace NetSchool.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class CardCollectionsController : ControllerBase
    {
        private readonly IAppLogger _logger;
        private readonly ICartCollectionService _cartCollectionService;

        public CardCollectionsController(IAppLogger logger, ICartCollectionService cardCollectionService)
        {
            _logger = logger;
            _cartCollectionService = cardCollectionService;
        }

        [HttpGet("")]
        public async Task<IEnumerable<CardCollectionModel>> GetAll()
        {
            var result = await _cartCollectionService.GetAll();

            return result;
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var result = await _cartCollectionService.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("")]
        public async Task<CardCollectionModel> Create([FromBody] CreateModel request)
        {
            var result = await _cartCollectionService.Create(request);

            return result;
        }

        [HttpPut("{id:Guid}")]
        public async Task Update([FromRoute] Guid id, UpdateModel request)
        {
            await _cartCollectionService.Update(id, request);
        }

        [HttpDelete("{id:Guid}")]
        public async Task Delete([FromRoute] Guid id)
        {
            await _cartCollectionService.Delete(id);
        }
    }
}
