using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SharedLogic.Services;

namespace GrpcService.Services
{
    public class TestItemGrpcService : TestItemEndpoint.TestItemEndpointBase
    {
        private readonly ITestService _testService;

        public TestItemGrpcService(ITestService testService)
        {
            _testService = testService;
        }
        override public async Task GetTestItems(EmptyParameters request, IServerStreamWriter<TestItemReply> responseStream, ServerCallContext context)
        {
            var items = _testService.GetTestItems();
            foreach(var item in items)
            {
                await responseStream.WriteAsync(new TestItemReply
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreatedAt = Timestamp.FromDateTime(item.CreatedAt)
                });
            };
        }

        public override Task<TestItemReply> CreateTestItem(CreateTestItemRequest request, ServerCallContext context)
        {
            var item = _testService.CreateTestItem(request.Name);
            return Task.FromResult(new TestItemReply
            {
                Id = item.Id,
                Name = item.Name,
                CreatedAt = Timestamp.FromDateTime(item.CreatedAt)
            });
        }
    }
}
