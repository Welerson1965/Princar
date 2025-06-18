using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Princar.Core.Domain.Interfaces.UoW;
using Princar.Seguranca.Domain.Commands.Produto;
using Princar.WebApi.Controllers.Base;

namespace Princar.WebApi.Controllers.Produto
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProdutoController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public ProdutoController(IConfiguration configuration, IUnitOfWork uow, IMediator mediator) : base(uow)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("consulta_produto")]
        public async Task<IActionResult> ConsultaProduto(ProdutoRequest request)
        {
            var commandResponse = await _mediator.Send(request);

            if (!commandResponse.Sucesso)
                return BadRequest(commandResponse);

            return IspacGetActionResult(commandResponse);
        }
    }
}
