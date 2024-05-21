using lib;
using ws;

namespace Tests;

[TestFixture]
public class Tests
{
    [SetUp]
    public void Setup()
    {
        Startup.startup(null);
    }

    [Test]
    public async Task Test1()
    {
        var ws = await new WebSocketTestClient().ConnectAsync();
        
        await ws.DoAndAssert(new GetServerStatusDto() { eventType = "GetServerStatus" }, response => response.Count == 1);
    }
}