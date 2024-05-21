using Fleck;

namespace api;

public static class CurrentConnections
{
    public static List<IWebSocketConnection> Connections = new List<IWebSocketConnection>();

}