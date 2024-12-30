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
        public async Task<IActionResult> GetInicio([FromBody] JsonElement model)
        {
            try
            {
                if (model.TryGetProperty("data", out JsonElement dataElement) &&
                    dataElement.TryGetProperty("message", out JsonElement messageElement) &&
                    messageElement.TryGetProperty("_data", out JsonElement dataContentElement) &&
                    dataContentElement.TryGetProperty("body", out JsonElement bodyElement))
                {
                    var mensagem = bodyElement.GetString();

                    var pergunta = await _service.GetPerguntaAsync("1");
                    if (pergunta == null)
                    {
                        return NotFound(new { message = "Pergunta inicial não encontrada." });
                    }
                    return Ok(new { pergunta, mensagem });
                }
                else
                {
                    return BadRequest(new { message = "Formato de entrada inválido ou campo 'body' não encontrado." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao processar o JSON.", error = ex.Message });
            }
        }

        //[HttpPost("inicio")]
        //public async Task<IActionResult> GetInicio([FromBody] dynamic model)
        //{


        //    var pergunta = await _service.GetPerguntaAsync("1");
        //    if (pergunta == null)
        //    {
        //        return NotFound(new { message = "Pergunta inicial não encontrada." });
        //    }
        //    return Ok(pergunta);
        //}

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
