using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewBank.Core.Domain.Interfaces.UoW;
using NewBank.Seguranca.Domain.Commands.Seguranca;
using NewBank.WebApi.Controllers.Base;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NewBank.WebApi.Controllers.Seguranca
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AutenticarController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public AutenticarController(IConfiguration configuration, IUnitOfWork uow, IMediator mediator) : base(uow)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("autenticar")]
        public async Task<IActionResult> Autenticar(AutenticarRequest request)
        {
            var commandResponse = await _mediator.Send(request);

            if (!commandResponse.Sucesso)
                return BadRequest(commandResponse);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "ispac",
                audience: "ispac",
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return Ok(new
            {
                AcessToken = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
