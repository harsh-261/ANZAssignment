using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using MoveRobotAssignment.Middleware;
using MoveRobotAssignment.StateMachine;
using MoveRobotAssignment.StateMachine.Interface;
using MoveRobotAssignment.Validator;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<RobotInitializer>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog();
});

builder.Services.AddSingleton(Log.Logger);

builder.Services.AddSingleton<IBoard, Board>();
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<InputPositionValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<RobotInitializer>();
    await initializer.InitAsync();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
