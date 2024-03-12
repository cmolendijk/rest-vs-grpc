using Gateway;
using Gateway.Clients;
using Grpc.Core;
using GrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpClient<IRestApiClient, RestApiClient>(c => c.BaseAddress = new Uri("https://localhost:7138"));
builder.Services.AddGrpcClient<TestItemEndpoint.TestItemEndpointClient>(o => o.Address = new Uri("https://localhost:7109"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/restapi/test-items", async (IRestApiClient restApi) =>
{
    return await restApi.Get_test_itemAsync();
})
.WithName("GetTestItemsWithRestApi")
.Produces<ICollection<TestItem>>()
.WithOpenApi();

app.MapPost("/restapi/test-items", async (IRestApiClient restApi, string name) =>
{
    return await restApi.Create_test_itemAsync(name);
})
.WithName("PostTestItemsWithRestApi")
.Produces<TestItem>()
.WithOpenApi();

app.MapGet("/grpc/test-items", async (TestItemEndpoint.TestItemEndpointClient grpcClient) =>
{
    List<TestItemReply> testItems = new List<TestItemReply>();
    var response = grpcClient.GetTestItems(new EmptyParameters());
    while (await response.ResponseStream.MoveNext())
    {
        testItems.Add(response.ResponseStream.Current);
    }
    return testItems;
})
    .WithName("GetTestItemsWithGrpc")
    .Produces<ICollection<TestItemReply>>()
    .WithOpenApi();

app.MapPost("/grpc/test-items", async (TestItemEndpoint.TestItemEndpointClient grpcClient, string name) =>
{
    return await grpcClient.CreateTestItemAsync(new CreateTestItemRequest { Name = name });
})
    .WithName("PostTestItemsWithGrpc")
    .Produces<TestItemReply>()
    .WithOpenApi();

app.Run();

