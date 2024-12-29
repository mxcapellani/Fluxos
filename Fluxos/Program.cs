using Fluxos.Repository;
using Fluxos.Service;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Obtém o ambiente de execução
var env = builder.Environment;

// Configurações do banco de dados PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

// Registra IDbConnection para PostgreSQL no DI
builder.Services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(connectionString));

// Registra o PerguntasService no DI
builder.Services.AddScoped<PerguntasService>();

// Adiciona serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<PerguntasRepository>();
builder.Services.AddScoped<PerguntasService>();
builder.Services.AddScoped<WppService>();



var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
