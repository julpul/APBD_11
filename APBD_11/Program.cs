using APBD_11.Data;
using APBD_11.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(optins =>
    optins.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);


builder.Services.AddScoped<IDbService, DbService>();

var app = builder.Build();

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
