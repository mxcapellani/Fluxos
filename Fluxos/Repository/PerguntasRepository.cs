using Dapper;
using Fluxos.Domain;
using System.Data;
using System.Data.Common;

namespace Fluxos.Repository
{
    public class PerguntasRepository
    {
        private readonly IDbConnection _connection;

        public PerguntasRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<Historico>> GetHistoricoAsync(string telefone)
        {
            const string sql = @"
            SELECT id, data, telefone, id_pergunta AS IdPergunta, id_resposta AS IdResposta
            FROM historico
            WHERE telefone = @Telefone";

            return await _connection.QueryAsync<Historico>(sql, new { Telefone = telefone });
        }

        public async Task<int> InsertHistoricoAsync(Historico historico,int idEmpresa)
        {
            const string sql = @"
            INSERT INTO historico (data, telefone, id_pergunta, id_resposta,id_cliente)
            VALUES (@Data, @Telefone, @Id_Pergunta, @Id_Resposta,@idEmpresa)
            RETURNING id;";

            // Retorna o ID do registro inserido
            return await _connection.ExecuteScalarAsync<int>(sql, new
            {
                Data = DateTime.UtcNow,
                historico.Telefone,
                historico.Id_Pergunta,
                historico.Id_Resposta,
                idEmpresa = idEmpresa
            });
        }

        public async Task<Pergunta> ConsultaPerguntaFluxo(int fluxoId)
        {
            var pergunta = await _connection.QueryFirstOrDefaultAsync<Pergunta>(
           "SELECT id, texto AS PerguntaTexto FROM perguntas WHERE id = @FluxoId",
           new { FluxoId = fluxoId }
            );
            return pergunta;
        }

        public async Task<List<Opcao>> ConsultaRespostasFluxo(int pergunta)
        {

            var respostas = await _connection.QueryAsync<Opcao>(
                "SELECT id, texto, fluxo_destino AS FluxoDestino, pergunta_id AS PerguntaId " +
                "FROM opcoes WHERE pergunta_id = @PerguntaId",
                new { PerguntaId = pergunta }
            );
            var lista = respostas.ToList();
            return lista;
        }

        public async Task<Empresa> ConsultarEmpresa(string telefone)
        {
            var empresa = await _connection.QueryAsync<Empresa>($"select * from empresa e where e.telefone = '{telefone}'");
            return empresa.FirstOrDefault();
 
        }

        public async Task<List<Historico>> ConsultarHistorico(string telefone, int idEmpresa)
        {
            var sql = $"select * from historico h where h.telefone = '{telefone}' and h.id_cliente = {idEmpresa}";
            var historico = await _connection.QueryAsync<Historico>(sql);
            var retorno = historico.ToList();
            return retorno;

        }

        public async Task<TextoGenerico> ConsultarTextoGenerico(int idTexto)
        {
            var sql = $"select * from textos_genericos h where h.id = {idTexto}";
            var historico = await _connection.QueryAsync<TextoGenerico>(sql);
            return historico.First();
        }

        public async Task<Fluxo> ConsultarFluxo(string texto)
        {
            var fluxoTexto = await _connection.QueryAsync<Fluxo>($"select * from fluxos f where f.id_empresa =1 and f.ativo =1 and f.palavras like '%{texto}%'");
            
            if(fluxoTexto.Count() == 0 )
            {
                var fluxo = await _connection.QueryAsync<Fluxo>($"select * from fluxos f where f.id_empresa =1 and f.ativo =1 and f.palavras = ''");
                return fluxo.First();
            }
            else
            {
                return fluxoTexto.First();
            }
        }

        public async Task<Pergunta> ConsultarPergunta(int idPergunta)
        {
            var sql = $"select * from perguntas h where h.id = {idPergunta}";
            var perguntas = await _connection.QueryAsync<Pergunta>(sql);
            return perguntas.First();
        }
        public async Task<List<Opcao>> ConsultarRespostasPorPergunta(int idPergunta)
        {
            var sql = $"select * from opcoes h where h.pergunta_id = {idPergunta}";
            var opcoes = await _connection.QueryAsync<Opcao>(sql);
            return opcoes.ToList();
        }

        public async Task<Opcao> ConsultarResposta(int idPergunta)
        {
            var sql = $"select * from opcoes h where h.id = {idPergunta}";
            var opcao = await _connection.QueryAsync<Opcao>(sql);
            return opcao.First();
        }
    }
}
