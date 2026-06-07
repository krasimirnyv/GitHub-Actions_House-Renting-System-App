using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Data.Entities;

namespace HouseRentingSystem.Tests.UnitTests;

[TestFixture]
public class AgentServiceTests : UnitTestsBase
{
    private IAgentService agentService;

    [OneTimeSetUp]
    public void SetUp()
        => agentService = new AgentService(data);

    [Test]
    public void GetAgentId_ShouldReturnCorrectUserId()
    {
        // Arrange

        // Act: invoke the service method with valid id
        int resultAgentId = agentService.GetAgentId(Agent.UserId);

        // Assert a correct id is returned
        Assert.AreEqual(Agent.Id, resultAgentId);
    }

    [Test]
    public void ExistsById_ShouldReturnTrue_WithValidId()
    {
        // Arrange

        // Act: invoke the service method with valid agent id
        bool result = agentService.ExistsById(Agent.UserId);

        // Assert the method result is true
        Assert.IsTrue(result);
    }

       
    [Test]
    public void AgentWithPhoneNumberExists_ShouldReturnTrue_WithValidData()
    {
        // Arrange

        // Act: invoke the service method with valid agent phone num
        bool result = agentService
            .AgentWithPhoneNumberExists(Agent.PhoneNumber);

        // Assert the method result is true
        Assert.IsTrue(result);
    }

        
    [Test]
    public void CreateAgent_ShouldWorkCorrectly()
    {
        // Arrange: get all agents' current count
        int agentsCountBefore = data.Agents.Count();

        // Act: invoke the service method with valid data
        agentService.Create(Agent.UserId, Agent.PhoneNumber);

        // Assert the agents' count has increased by 1
        int agentsCountAfter = data.Agents.Count();
        Assert.AreEqual(agentsCountBefore + 1, agentsCountAfter);

        // Assert a new agent was created in the db with correct data
        int newAgentId = agentService.GetAgentId(Agent.UserId);
        Agent? newAgentInDb = data.Agents.Find(newAgentId);
        Assert.IsNotNull(newAgentInDb);
        Assert.AreEqual(Agent.UserId, newAgentInDb.UserId);
        Assert.AreEqual(Agent.PhoneNumber, newAgentInDb.PhoneNumber);
    }
}