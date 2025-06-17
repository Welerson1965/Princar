using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Princar.Core.Domain.Interfaces.UoW;
using Princar.Sicoob.DTOs.ObterToken;
using Princar.WebApi.Controllers.Base;

namespace Princar.WebApi.Controllers.Sicoob
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class SicoobController : BaseController
    {
        private readonly IMediator _mediator;

        public SicoobController(IUnitOfWork uow, IMediator mediator) : base(uow)
        {
            _mediator = mediator;
        }
    }
}
