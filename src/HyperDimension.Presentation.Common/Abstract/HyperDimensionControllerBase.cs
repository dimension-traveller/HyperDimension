using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HyperDimension.Presentation.Common.Abstract;

public class HyperDimensionControllerBase : ControllerBase
{
    private readonly IMediator _mediator;

    public HyperDimensionControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected Task<IActionResult> SendAsync<T>(T request) where T : IRequest<IActionResult>
    {
        return _mediator.Send(request);
    }
}
