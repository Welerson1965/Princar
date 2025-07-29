using Microsoft.AspNetCore.Mvc;
using MercadoLivre.Core.Domain.DTOs;
using MercadoLivre.Core.Domain.Interfaces.UoW;
using prmToolkit.NotificationPattern;

namespace MercadoLivre.WebApi.Controllers.Base
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public abstract class BaseController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        protected BaseController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        protected IActionResult IspacGetActionResult(CommandResponse commandResponse)
        {
            if (!commandResponse.Sucesso)
                return BadRequest(commandResponse);

            if (commandResponse.Dados == null)
                return NoContent();

            return Ok(commandResponse);
        }

        protected IActionResult IspacPostActionResult(CommandResponse commandResponse)
        {
            if (!commandResponse.Sucesso)
                return BadRequest(commandResponse);

            var commitResult = _uow.Commit();
            if (commitResult.Sucesso)
                return Created("", commandResponse);

            return TratativaErroPersistencia(commitResult);
        }

        protected IActionResult IspacPutActionResult(CommandResponse commandResponse)
        {
            if (!commandResponse.Sucesso)
                return BadRequest(commandResponse);

            var commitResult = _uow.Commit();
            if (commitResult.Sucesso)
                return Ok(commandResponse);

            return TratativaErroPersistencia(commitResult);
        }

        protected IActionResult IspacDeleteActionResult(CommandResponse commandResponse)
        {
            if (!commandResponse.Sucesso)
                return BadRequest(commandResponse);

            var commitResult = _uow.Commit();
            if (commitResult.Sucesso)
                return Ok(commandResponse);

            return TratativaErroPersistencia(commitResult);
        }

        private IActionResult TratativaErroPersistencia(CommitResult commitResult)
        {
            var erroResponse = new List<Notification>
            {
                new Notification("Persitência", commitResult.MensagemErroDetalhada != null
                    ? $"Ocorreu o seguinte erro ao persistir os dados: {commitResult.MensagemErro} - {commitResult.MensagemErroDetalhada}"
                    : $"Ocorreu o seguinte erro ao persistir os dados: {commitResult.MensagemErro}")

            };
            return BadRequest(new
            {
                Sucesso = false,
                Notificacoes = erroResponse
            });
        }
    }
}
