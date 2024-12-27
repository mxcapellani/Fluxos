using System.Data;
using Dapper;
using Fluxos.Domain;

public class PerguntasService
{
    private readonly IDbConnection _connection;

    public PerguntasService(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Pergunta> GetPerguntaAsync(string fluxo)
    {
        // Tente converter o fluxo para inteiro antes de fazer a consulta
        if (!int.TryParse(fluxo, out int fluxoId))
        {
            throw new ArgumentException("O fluxo fornecido deve ser um número válido.", nameof(fluxo));
        }

        // Consulta SQL ajustada
        var pergunta = await _connection.QueryFirstOrDefaultAsync<Pergunta>(
            "SELECT id, texto AS PerguntaTexto FROM perguntas WHERE id = @FluxoId",
            new { FluxoId = fluxoId }
        );

        if (pergunta != null)
        {
            pergunta.Opcoes = (await _connection.QueryAsync<Opcao>(
                "SELECT id, texto, fluxo_destino AS FluxoDestino, pergunta_id AS PerguntaId " +
                "FROM opcoes WHERE pergunta_id = @PerguntaId",
                new { PerguntaId = pergunta.Id }
            )).ToList();
        }

        return pergunta;
    }

    public async Task<int> InsertHistoricoAsync(Historico historico)
    {
        const string sql = @"
        INSERT INTO historico (data, telefone, id_pergunta, id_resposta)
        VALUES (NOW(), @Telefone, @IdPergunta, @IdResposta)
        RETURNING id;";

        // Retorna o ID do registro inserido
        return await _connection.ExecuteScalarAsync<int>(sql, new
        {
            historico.Telefone,
            historico.IdPergunta,
            historico.IdResposta
        });
    }

    public async Task<IEnumerable<Historico>> GetHistoricoAsync(string telefone)
    {
        const string sql = @"
        SELECT id, data, telefone, id_pergunta AS IdPergunta, id_resposta AS IdResposta
        FROM historico
        WHERE telefone = @Telefone";

        return await _connection.QueryAsync<Historico>(sql, new { Telefone = telefone });
    }
}
