using Api.Entities;
using Api.Features.PaymentMaintenance.Command;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Features.PaymentMaintenance;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST api/<PaymentsController>
    [HttpPost]
    public async Task<ActionResult<Payment>> Post([FromBody] CreatePaymentCommand command)
    {
        return (await _mediator.Send(command)).ToActionResult(this);
    }

}
