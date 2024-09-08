using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddInfrastructureLayer(builder.Configuration)
    .AddDomainLayer(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.UseAuthorization();

app.MapControllers();

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    using var context = serviceScope.ServiceProvider.GetRequiredService<ShowDbContext>();
    context.Database.EnsureCreated();

    if (!context.Show.Any())
    {
        var seedingService = serviceScope.ServiceProvider.GetRequiredService<ISeedingService>();
        var allShows = await seedingService.GetShowsAsync().ToListAsync();

        using (var transaction = context.Database.BeginTransaction())
        {
            await context.Show.AddRangeAsync(allShows);
            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Show] ON;");
            await context.SaveChangesAsync();
            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Show] OFF;");
            await transaction.CommitAsync();
        }
    }
}

app.Run();
