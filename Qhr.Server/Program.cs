using Microsoft.EntityFrameworkCore;
using Qhr.Server;

using Qhr.Server.Controllers;
using Qhr.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QhrContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("db")));
builder.Services.AddControllers();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<QhrContext>();
    dbContext.Database.EnsureCreated();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();


app.Run();