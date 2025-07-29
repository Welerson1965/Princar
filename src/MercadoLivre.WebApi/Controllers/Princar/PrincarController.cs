using MediatR;
using MercadoLivre.Core.Domain.Interfaces.UoW;
using MercadoLivre.Seguranca.Domain.Commands.Princar;
using MercadoLivre.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercadoLivre.WebApi.Controllers.Princar
{
    public class PrincarController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public PrincarController(IConfiguration configuration, IUnitOfWork uow, IMediator mediator) : base(uow)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("princar_notificacoes")]
        public async Task<IActionResult> PrincarNotificacoes(NotificacoesPrincarRequest request)
        {
            var commandResponse = await _mediator.Send(request);

            if (!commandResponse.Sucesso)
                return BadRequest(commandResponse);

            return IspacGetActionResult(commandResponse);
        }
    }
}
