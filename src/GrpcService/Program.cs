using GrpcService.Services;
using SharedLogic.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITestService, TestService>();
// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<TestItemGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
