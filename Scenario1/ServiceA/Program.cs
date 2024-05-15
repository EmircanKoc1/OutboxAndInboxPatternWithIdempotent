using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceA.Context;
using ServiceA.Outboxes;
using ServiceA.ViewModels;
using ServiceB.Consumers;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ServiceADbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});



var app = builder.Build();


app.MapPost("create-message", async ([FromServices] ServiceADbContext _context, [FromBody] MessageViewModel model, [FromServices] ILogger<Program> _logger) =>
{
    var idempotentId = Guid.NewGuid();
    _logger.LogInformation("Idempotent Token Created");

    MessageSendedEvent @event = new(idempotentId)
    {
        Content = model.Content,
        MessageId = Guid.NewGuid(),
        Title = model.Title
    };

    Outbox entity = new(idempotentId)
    {
        EventType = typeof(MessageSendedEvent).Name,
        Payload = @event.Serialize(),
        Sended = false,
        WritedDate = null
    };


    await _context.Outbox.AddAsync(entity);
    await _context.SaveChangesAsync();
    _logger.LogInformation("Otubox entity Added");


});


app.UseSwagger();
app.UseSwaggerUI();


app.Run();
