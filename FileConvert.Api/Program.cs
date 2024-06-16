using FileConvert.Domain.Interfaces;
using FileConvert.Domain.IServices;
using FileConvert.Domain.Services;
using FileConvert.Infra.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Repository
builder.Services.AddScoped<IConverter, ConverterRepository>();
#endregion

#region Service
builder.Services.AddScoped<IConverterService, ConverterService>();
#endregion

var app = builder.Build();

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
