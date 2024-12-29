using Fluxos.Domain;
using Fluxos.Domain.Wpp;
using Fluxos.Repository;
using Fluxos.Service;
using System.Data.Common;
using System.Linq;

public class PerguntasService
{
    private readonly PerguntasRepository _perguntasRepository;
    private readonly WppService _wppService;

    public PerguntasService(PerguntasRepository perguntasRepository, WppService wppService)
    {
        _perguntasRepository = perguntasRepository;
        _wppService = wppService;
    }

    public async Task<bool>  RespostasAsync(ReceberMensagem mensagem) {

        var empresa = await _perguntasRepository.ConsultarEmpresa(mensagem.To);

        if (empresa != null) {
            var msgWpp = new EnviarMensagem();

            var primeiraMensagem = await _perguntasRepository.ConsultarHistorico(mensagem.From, empresa.id);
            
            if (primeiraMensagem.Count == 0) { // nunca foi atendido
                var generico = await _perguntasRepository.ConsultarTextoGenerico(1);

                msgWpp.content = generico.texto;
                msgWpp.chatId = mensagem.From;
                msgWpp.contentType = "string";
                //await _wppService.SendMessageAsync(msgWpp);

                var fluxo = await _perguntasRepository.ConsultarFluxo(mensagem.Body);

                var perguntaIncio = await GetPerguntaAsync(fluxo.id_pergunta.ToString());

                var textoOpcao = perguntaIncio.PerguntaTexto;
                foreach (var item in perguntaIncio.Opcoes)
                {
                    textoOpcao += item.Id + " - " + item.Texto;
                }

                msgWpp.content = textoOpcao;
                msgWpp.chatId = mensagem.From;
                msgWpp.contentType = "string";
                //await _wppService.SendMessageAsync(msgWpp);

                var historico = new Historico { 
                Data = DateTime.Now,
                Id_Pergunta = perguntaIncio.Id,
                Telefone = mensagem.From
                };

                await _perguntasRepository.InsertHistoricoAsync(historico,empresa.id);
            }
            else { //já foi atendido
                
                var ultimoAtendimento = primeiraMensagem.OrderByDescending(m => m.Data).FirstOrDefault();
                var ultimaPergunta = await _perguntasRepository.ConsultarPergunta(ultimoAtendimento.Id_Pergunta);
                var respostasPossiveis = await _perguntasRepository.ConsultarRespostasPorPergunta(ultimoAtendimento.Id_Pergunta);

                // Verificar se algum item contém o texto
                //bool contemTexto = respostasPossiveis.Any(opcao => opcao.Texto.Contains(mensagem.Body, StringComparison.OrdinalIgnoreCase)); //procurar por texto

                var contemTexto = respostasPossiveis.FirstOrDefault(opcao => opcao.Id.ToString() == mensagem.Body);

                if (contemTexto == null){// não reconhece a resposta 
                    var generico = await _perguntasRepository.ConsultarTextoGenerico(2);
                    msgWpp.content = generico.texto;
                    msgWpp.chatId = mensagem.From;
                    msgWpp.contentType = "string";
                    //await _wppService.SendMessageAsync(msgWpp);
                }
                else //reconheci a resposta, seguir o fluxo
                {
                    var x = 0;
                }

            }
            return true;
        }
        else
        {
            return false;
            throw new Exception("Empresa não encontrada");
        }
        
    
    }

    public async Task<Pergunta> GetPerguntaAsync(string fluxo)
    {
        if (!int.TryParse(fluxo, out int fluxoId))
        {
            throw new ArgumentException("O fluxo fornecido deve ser um número válido.", nameof(fluxo));
        }

        var pergunta = await _perguntasRepository.ConsultaPerguntaFluxo(fluxoId);
            
        if (pergunta != null)
        {
            pergunta.Opcoes = await _perguntasRepository.ConsultaRespostasFluxo(pergunta.Id);
                
        }

        return pergunta;
    }

    public async Task<int> InsertHistoricoAsync(Historico historico,int idEmpresa)
    {
        return await _perguntasRepository.InsertHistoricoAsync(historico,idEmpresa);
    }

    public async Task<IEnumerable<Historico>> GetHistoricoAsync(string telefone)
    {
       return await _perguntasRepository.GetHistoricoAsync(telefone);
    }

    public async Task<List<Historico>> ConsultarHistorico(string telefone, int idEmpresa)
    {
       return await _perguntasRepository.ConsultarHistorico(telefone,idEmpresa);

    }
}
