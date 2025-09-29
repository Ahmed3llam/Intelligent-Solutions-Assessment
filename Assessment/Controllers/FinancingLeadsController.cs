using Application.DTOs;
using Application.FinancingLeads.Commands.ReviewFinancingLead;
using Application.FinancingLeads.Commands.SubmitFinancingLead;
using Application.FinancingLeads.Queries.GetFinancingLeadById;
using Application.FinancingLeads.Queries.GetFinancingLeads;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Assessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancingLeadsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FinancingLeadsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> SubmitLead([FromBody] SubmitFinancingLeadRequest data)
        {
            try
            {
                var command = new SubmitFinancingLeadCommand(data);
                var id = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetLeadById), new { id = id }, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(FinancingLeadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetLeadById(Guid id)
        {
            var query = new GetFinancingLeadByIdQuery(id);
            var lead = await _mediator.Send(query);

            if (lead == null)
                return NotFound();

            return Ok(lead);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetLeads([FromQuery] GetFinancingLeadsQueryDto data)
        {
            var query = new GetFinancingLeadsQuery(data);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("Review")]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> ReviewLead([FromBody] ReviewRequest request)
        {
            var command = new ReviewFinancingLeadCommand(request);
            var data = await _mediator.Send(command);

            if (data == null)
                return NotFound();

            return Ok(data);
        }
    }
}
