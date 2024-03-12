using SharedLogic.Entity;
using System.Collections;
using System.Collections.Concurrent;

namespace SharedLogic.Services
{
    public class TestService : ITestService
    {
        private BlockingCollection<TestItem> items = new BlockingCollection<TestItem>();

        public TestItem CreateTestItem(string name)
        {
            int nextId = items.Count + 1;
            var newTestItem = new TestItem(nextId, name, DateTime.UtcNow);
            items.Add(newTestItem);
            return newTestItem;
        }

        public List<TestItem> GetTestItems()
        {
            return items.ToList();
        }
    }
}
