using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Yape.Entities;
using Yape.Entities.Base;
using YapeServices.Entities.Dtos.Transactions;
using YapeServices.Entities.Models;
using YapeServices.Ports.Services;

namespace Yape.Adapter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionsService;

        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ServiceResult<Transaction>))]
        public async Task<ActionResult> Get()
        {
            return Ok(await _transactionsService.GetAllAsync());
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ServiceResult<List<Transaction>>))]
        public async Task<ActionResult> Get(string id)
        {
            return Ok(await _transactionsService.GetByIdAsync(id));
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ServiceResult<CreateTransactionResponse>))]
        public async Task<ActionResult> Post(CreateTransactionRequest model)
        {
            return Ok(await _transactionsService.AddAsync(model));
        }
    }
}
