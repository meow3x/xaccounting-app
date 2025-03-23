using Api.Features.ChartOfAccounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/ChartOfAccounts")]
    [ApiController]
    public class ChartOfAccountsController : ControllerBase
    {
        public readonly IMediator _mediator;
        public readonly ISender _sender;

        public ChartOfAccountsController(IMediator mediator, ISender sender)
        {
            _mediator = mediator;
            _sender = sender;
        }

        // GET: api/<ChartOfAccountsController>
        [HttpGet]
        public async Task<GetAccountResponse[]> Get()
        {
            return await _sender.Send(new GetAllAccountsQuery());
        }

        [HttpGet(nameof(GetAccountTypes))]
        public async Task<GetAccountTypes.GetAccountTypeResponse[]> GetAccountTypes()
        {
            return await _sender.Send(new GetAccountTypes.GetAllAccountTypesQuery());
        }

        // GET api/<ChartOfAccountsController>/5
        [HttpGet("{id}")]
        public async Task<GetAccountResponse> Get(int id)
        {
            return await _sender.Send(new GetAccountById.GetAccountByIdQuery(id));
        }

        // POST api/<ChartOfAccountsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAccount.CreateAccountCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { Id = result });
        }

        // PUT api/<ChartOfAccountsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateAccount.UpdateAccountCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { Error = "route/{id} must be same as body.id" });
            }
            var result = await _mediator.Send(command);
            return Ok(new { Id = result });
        }

        // DELETE api/<ChartOfAccountsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
