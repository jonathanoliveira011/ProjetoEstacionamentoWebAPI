using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjetoEstacionamentoWebAPI.Data;

//var context = new CustomAssemblyLoadContext();
//context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libs", "libwkhtmltox.dll"));

var builder = WebApplication.CreateBuilder(args);

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection))
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
