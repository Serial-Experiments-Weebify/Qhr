using Microsoft.EntityFrameworkCore;
using Qhr.Server;
using Qhr.Server.Services;
using Qhr.Server.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QhrContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("db")));

builder.Services.AddControllers();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<AuthFilter>();
builder.Services.AddSingleton<IJobPluginService, JobPluginService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<QhrContext>();
    dbContext.Database.EnsureCreated();

    var auth = scope.ServiceProvider.GetRequiredService<IAuthService>();
    await auth.CreateDefaultUser();
}

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
