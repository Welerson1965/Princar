using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Princar.Core.Domain.Interfaces.UoW;
using Princar.Seguranca.Domain.Commands.Seguranca;
using Princar.WebApi.Controllers.Base;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Princar.WebApi.Controllers.Seguranca
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

        //[AllowAnonymous]
        //[HttpPost("autenticar")]
        //public async Task<IActionResult> Autenticar(AutenticarRequest request)
        //{
        //    var commandResponse = await _mediator.Send(request);

        //    if (!commandResponse.Sucesso)
        //        return BadRequest(commandResponse);

        //    var empresa = (AutenticarResponse)commandResponse.Dados;

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name, empresa.NomeEmpresa)
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));

        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: "ispac",
        //        audience: "ispac",
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(1),
        //        signingCredentials: creds
        //    );

        //    return Ok(new
        //    {
        //        AcessToken = new JwtSecurityTokenHandler().WriteToken(token)
        //    });
        //}
    }
}
