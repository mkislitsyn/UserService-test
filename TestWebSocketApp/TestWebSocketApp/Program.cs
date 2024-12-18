using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using TestWebSocketApp;
using TestWebSocketApp.Enums;
using TestWebSocketApp.Models;

string apiUrl = Environment.GetEnvironmentVariable("API_URL") ?? "wss://localhost:7059/ws/userwebsocket/connect";

string[] predefinedActions = Environment.GetEnvironmentVariable("PREDEFINED_ACTIONS")?.Split(',') ?? ["3", "4"];
var actionQueue = new Queue<string>(predefinedActions);

bool useAuto = predefinedActions.Count() > 2;
var serverUri = new Uri(apiUrl);

using var client = new ClientWebSocket();

try
{
    // Connect to the WebSocket server
    Console.WriteLine($"Connecting to the WebSocket server url {serverUri}...");
    await client.ConnectAsync(serverUri, CancellationToken.None);
    Console.WriteLine("Connected to WebSocket server!");
    
    var receiveTask = ReceiveMessagesAsync(client);
    var sendTask = useAuto ? AautoTest.SendMessagesAsync(client, actionQueue) : SendMessagesAsync(client);

    await Task.WhenAll(sendTask, receiveTask);

}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    // Close the WebSocket connection
    await CloseWebSocketAsync(client, "Closing");
}

static async Task SendMessagesAsync(ClientWebSocket client)
{
    while (client.State == WebSocketState.Open)
    {
        Console.WriteLine("\nChoose an action: 1. AddUser  2. UpdateUser  3. GetAll  4.Exit");
        var choice = Console.ReadLine();

        if (choice == "4")
        { 
            await CloseWebSocketAsync(client, "Client closing");
            break;
        }

        string message = choice switch
        {
            "1" => AddUser(),
            "2" => UpdateUser(),
            "3" => GetAll(),
            _ => "Invalid choice"
        };

        if (message == "Invalid choice")
        {
            Console.WriteLine("Invalid choice please try again");
        }
        else
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}

// Method to receive messages from the WebSocket server
static async Task ReceiveMessagesAsync(ClientWebSocket client)
{
    var buffer = new byte[1024 * 4];
    try
    {
        while (client.State == WebSocketState.Open)
        {
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                Console.WriteLine("Server closed connection.");
                await CloseWebSocketAsync(client, "Closing");
            }
            else
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received from server: {message}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Receive error: {ex.Message}");
    }
}

static async Task CloseWebSocketAsync(ClientWebSocket client, string reason)
{
    if (client.State == WebSocketState.Open)
    {
        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, reason, CancellationToken.None);
        Console.WriteLine("WebSocket connection closed.");
    }
}

static string AddUser()
{
    Console.Write("Enter user name: ");
    var name = Console.ReadLine();

    Console.Write("Enter user email: ");
    var email = Console.ReadLine();

    Console.Write("Choose user role: 1. Admin  2. User: ");
    var userRole = Console.ReadLine();

    var role = userRole switch
    {
        "1" => UserRole.Admin,
        "2" => UserRole.User,
        _ => UserRole.User
    };

    Console.Write("Enter user Password: ");
    var password = Console.ReadLine();

    var userAction = new UserAction { ActionType = ActionType.Create, User = new UserDto { Name = name, Email = email, Password = password, Role = role } };
    return JsonSerializer.Serialize(userAction);
}

static string UpdateUser()
{
    Console.Write("Enter user ID: ");
    if (!long.TryParse(Console.ReadLine(), out var userId))
    {
        Console.WriteLine("Invalid user ID. Please enter a numeric value.");
        return string.Empty;
    }

    Console.Write("Choose user role: 1. Admin  2. User: ");
    var userRole = Console.ReadLine();

    var role = userRole switch
    {
        "1" => UserRole.Admin,
        "2" => UserRole.User,
        _ => UserRole.User
    };

    var userAction = new UserAction { ActionType = ActionType.Update, User = new UserDto { UserId = userId, Role = role } };
    return JsonSerializer.Serialize(userAction);
}







static string GetAll()
{
    var userAction = new UserAction { ActionType = ActionType.GetAll };
    return JsonSerializer.Serialize(userAction);
}