using Fluxos.Domain;
using Fluxos.Domain.Wpp;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

namespace Fluxos.Controllers
{
    [ApiController]
    [Route("api/perguntas")]
    public class PerguntasController : ControllerBase
    {
        private readonly PerguntasService _service;

        public PerguntasController(PerguntasService service)
        {
            _service = service;
        }

        [HttpPost("inicio")]
        public async Task<IActionResult> GetInicio()
         {


            var pergunta = await _service.GetPerguntaAsync("1");
            if (pergunta == null)
            {
                return NotFound(new { message = "Pergunta inicial não encontrada." });
            }
            return Ok(pergunta);
        }

        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] ReceberMensagem message)
        {

            await _service.RespostasAsync(message);
            return Ok();
        }

        [HttpPost("continuar")]
        public async Task<IActionResult> Continuar([FromBody] ContinuarRequest request)
        {
            if (string.IsNullOrEmpty(request.Fluxo))
            {
                return BadRequest(new { message = "O fluxo é obrigatório." });
            }

            var pergunta = await _service.GetPerguntaAsync(request.Fluxo);
            if (pergunta == null)
            {
                return NotFound(new { message = "Fluxo não encontrado." });
            }

            return Ok(pergunta);
        }

    }
}
