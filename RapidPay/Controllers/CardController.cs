using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Application.Dtos;
using RapidPay.Application.Features.CardFeatures.Commands.CreateCard;
using RapidPay.Application.Features.CardFeatures.Commands.CreatePayment;
using RapidPay.Application.Features.CardFeatures.Querys;

namespace RapidPay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "user,admin")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCard([FromBody] CreateCardCommand createCardCommand)
        {
            var result = await _mediator.Send(createCardCommand);
            return StatusCode(result.Success ? 201 : 400, result);
        }

        [Authorize(Roles = "user,admin")]
        [HttpPost("pay")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentCommand createPaymentCommand)
        {
            var result = await _mediator.Send(createPaymentCommand);
            return StatusCode(result.Success ? 201 : 400, result);
        }

        [Authorize(Roles = "user,admin")]
        [HttpGet("balance/{cardNumber}")]
        public async Task<IActionResult> GetBalance(string cardNumber)
        {
            var result = await _mediator.Send(new GetCardBalanceQuery { CardNumber = cardNumber });
            return StatusCode(result.Success ? 200 : 404, result);
        }
    }
}
