using SharedLogic.Entity;

namespace SharedLogic.Services
{
    public interface ITestService
    {
        TestItem CreateTestItem(string name);
        List<TestItem> GetTestItems();
    }
}