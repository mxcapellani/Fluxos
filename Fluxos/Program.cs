using Fluxos.Repository;
using Fluxos.Service;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Obt�m o ambiente de execu��o
var env = builder.Environment;

// Configura��es do banco de dados PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

// Registra IDbConnection para PostgreSQL no DI
builder.Services.AddScoped<IDbConnection>(sp => new NpgsqlConnection(connectionString));

// Registra o PerguntasService no DI
builder.Services.AddScoped<PerguntasService>();

// Adiciona servi�os ao cont�iner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<PerguntasRepository>();
builder.Services.AddScoped<PerguntasService>();
builder.Services.AddScoped<WppService>();



var app = builder.Build();

// Configura��o do pipeline de requisi��es HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
