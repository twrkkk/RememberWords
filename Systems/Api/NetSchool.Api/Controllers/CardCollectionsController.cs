using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetSchool.Common.Exceptions;
using NetSchool.Common.Security;
using NetSchool.Services.CardCollections;
using NetSchool.Services.CardCollections.CardCollections;
using NetSchool.Services.Logger;

namespace NetSchool.Api.Controllers
{
    // [Authorize]
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
        //[Authorize(AppScopes.CollectionsRead)]
        public async Task<IEnumerable<CardCollectionModel>> GetAll()
        {
            var result = await _cartCollectionService.GetAll();

            return result;
        }

        [HttpGet("{id:Guid}")]
        //[Authorize(AppScopes.CollectionsRead)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var result = await _cartCollectionService.Get(id);
            return Ok(result);

            //var result = await _cartCollectionService.Get(id);
            //return Ok(result);
        }

        [HttpPost("")]
        // [Authorize(AppScopes.CollectionsWrite)]
        public async Task<CardCollectionModel> Create([FromBody] CreateModel request)
        {
            var result = await _cartCollectionService.Create(request);
            return result;
        }

        [HttpPut("{id:Guid}")]
        //[Authorize(AppScopes.CollectionsWrite)]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateModel request)
        {
            await _cartCollectionService.Update(id, request);
            return Ok();
        }

        [HttpDelete("{id:Guid}")]
        //[Authorize(AppScopes.CollectionsWrite)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _cartCollectionService.Delete(id);
            return Ok();
        }
    }
}
