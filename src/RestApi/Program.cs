using SharedLogic.Entity;
using SharedLogic.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITestService, TestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/test-items", async (ITestService testService) =>
{
    var items = testService.GetTestItems();

    return Results.Ok(items);
})
.WithName("Get test item")
.Produces<List<TestItem>>()
.WithOpenApi();

app.MapPost("/test-items", async (string Name, ITestService testService) =>
{
    var item = testService.CreateTestItem(Name);

    return Results.Ok(item);
})
.WithName("Create test item")
.Produces<TestItem>()
.WithOpenApi();

app.Run();