﻿namespace ContosoUniversityBlazor.WebUI.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

[ApiController]
[Route("api/[controller]")]
public abstract class ContosoApiController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}
