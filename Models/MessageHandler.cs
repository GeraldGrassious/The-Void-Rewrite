using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using System.IO;

namespace TheVoidRewrite.Models;

public class MessageEventArgs(string type, string sender, string senderNameColour, string time, string data) : EventArgs
{
    public string Type { get; } = type;
    public string Sender { get; } = sender;
    public string SenderNameColour { get; } = senderNameColour;
    public string Time { get; } = time;
    public string Data { get; } = data;

}

public class MessageHandler
{
    private readonly Uri uri = new("wss://the-void.cc");
    private readonly Queue<string> messageQueue = new();
    public event EventHandler<MessageEventArgs> MessageReceived = delegate {};

    private string? username = null;
    private string? usernameColour = null;

    public async void MessageLoop(string username, string usernameColour)
    {
        this.username = username;
        this.usernameColour = usernameColour;

        while (true)
        {
            ClientWebSocket ws = new();
            ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);

            // Continue trying to connect if unable
            while (ws.State != WebSocketState.Open)
            {
                try
                {
                    ws.Dispose();
                    ws = new();
                    await ws.ConnectAsync(uri, default);
                } 
                catch (WebSocketException)
                {
                    await Task.Delay(1000);
                }
            }

            // Does receiving and sending without locking out one of them
            var receiveTask = ReceiveMessages(ws);
            var sendTask = SendMessages(ws);

            await Task.WhenAll(receiveTask, sendTask);
        }
    }

    public void SendChatMessage(string chatMessage)
    {
        messageQueue.Enqueue(ChatToJson(chatMessage));
    }

    private string ChatToJson(string message)
    {
        MessageEventArgs newMessage = new("Chat", username!, usernameColour!, "", message);

        string jsonString = JsonSerializer.Serialize(newMessage);

        return jsonString;
    }

    private async Task SendMessages(ClientWebSocket ws)
    {
        while (ws.State == WebSocketState.Open)
        {
            // Send message to server
            int messageQueueSize = messageQueue.Count;
            for (int i = 0; i < messageQueueSize; i++)
            {
                var buffer = Encoding.UTF8.GetBytes(messageQueue.Dequeue());
                await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);     
            }

            if (ws.State != WebSocketState.Open)
            {
                return;
            }

            await Task.Delay(10);
        }
    }

    private async Task ReceiveMessages(ClientWebSocket ws)
    {
        var receiveBuffer = new byte[1024];
        while (ws.State == WebSocketState.Open)
        {
            try {
                // Listen for messages from the server
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("Server closed the connection.");
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    return;
                }

                // Makes sure the whole message is received
                MemoryStream byteMessage = new();
                byteMessage.Write(receiveBuffer, 0, result.Count);

                while (!result.EndOfMessage)
                {
                    result = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    byteMessage.Write(receiveBuffer, 0, result.Count);
                }

                string receivedMessage = Encoding.UTF8.GetString(byteMessage.ToArray(), 0, (int)byteMessage.Length);
                MessageEventArgs? jsonMessage = JsonSerializer.Deserialize<MessageEventArgs>(receivedMessage);

                if (jsonMessage is null)
                {
                    return;
                }

                MessageReceived?.Invoke(this, jsonMessage);
                    
            }
            catch (WebSocketException)
            {
                return;
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }
}