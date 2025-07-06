using Dapper;
using GerenciadorData;
using GerenciadorModel;
using GerenciadorModel.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MainDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainDB"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Declarando dependências da entidade Carne no projeto
builder.Services.AddTransient<ICarneData, CarneData>();

// Declarando dependências da entidade Comprador no projeto
builder.Services.AddTransient<ICompradorData, CompradorData>();

// Declarando dependências da entidade Pedido no projeto
builder.Services.AddTransient<IPedidoData, PedidoData>();

// Declarando dependências da entidade ItemPedido no projeto
builder.Services.AddTransient<IItemPedidoData, ItemPedidoData>();

var app = builder.Build();

#region Criando as tabelas e preenchendo elas com alguns dados
using (var scope = app.Services.CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    var scriptFolder = Path.Combine(Directory.GetCurrentDirectory(), "Scripts");

    var tablesScriptPath = Path.Combine(scriptFolder, "tables.sql");
    var dataScriptPath = Path.Combine(scriptFolder, "tables_data.sql");

    using var connection = new SqlConnection(connectionString);

    if (File.Exists(tablesScriptPath))
    {
        var tablesScript = File.ReadAllText(tablesScriptPath);
        await connection.ExecuteAsync(tablesScript);
    }

    if (File.Exists(dataScriptPath))
    {
        var dataScript = File.ReadAllText(dataScriptPath);
        await connection.ExecuteAsync(dataScript);
    }
}
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
