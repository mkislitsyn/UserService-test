using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using TestWebSocketApp.Enums;
using TestWebSocketApp.Models;

namespace TestWebSocketApp
{
    internal static class AautoTest
    {

        internal static async Task SendMessagesAsync(ClientWebSocket client, Queue<string> actionQueue)
        {

            int userCounter = DateTime.Now.Minute * 1000 + new Random().Next(1000, 9999);

            while (client.State == WebSocketState.Open && actionQueue.Count > 0)
            {
                var choice = actionQueue.Dequeue(); // Use predefined actions
                Console.WriteLine($"\nExecuting action: {choice}");

                if (choice == "4")
                {
                    await CloseWebSocketAsync(client, "Client closing");
                    break;
                }

                string message = choice switch
                {
                    "1" => AddUser(++userCounter),
                    "2" => UpdateUser(),
                    "3" => GetAll(),
                    _ => "Invalid choice"
                };

                if (message == "Invalid choice")
                {
                    Console.WriteLine("Invalid choice. Skipping...");
                }
                else
                {
                    var bytes = Encoding.UTF8.GetBytes(message);
                    await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                    Console.WriteLine($"Message sent for action: {choice}");
                }
                await Task.Delay(TimeSpan.FromSeconds(1));
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

        static string AddUser(int index)
        {
            var userAction = new UserAction
            {
                ActionType = ActionType.Create,
                User = new UserDto
                {
                    Name = $"AutomatedUser{index}",
                    Email = $"automateduser{index}@example.com",
                    Password = "password123",
                    Role = UserRole.User
                }
            };
            return JsonSerializer.Serialize(userAction);
        }

        static string UpdateUser()
        {
            var userAction = new UserAction
            {
                ActionType = ActionType.Update,
                User = new UserDto
                {
                    UserId = 1,
                    Role = UserRole.Admin
                }
            };
            return JsonSerializer.Serialize(userAction);
        }

        static string GetAll()
        {
            var userAction = new UserAction { ActionType = ActionType.GetAll };
            return JsonSerializer.Serialize(userAction);
        }
    }
}
