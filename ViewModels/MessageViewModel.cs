using System.Collections.ObjectModel;
using TheVoidRewrite.Models;

namespace TheVoidRewrite.ViewModels;

public class MessageData(string data)
{
    public string Data { get; } = data;
}

public class MessageInfo(string sender, string senderNameColour, string time, string data) : MessageData(data)
{
    public string Sender { get; } = sender;
    public string SenderNameColour { get; } = senderNameColour;
    public string Time { get; } = time;
}

public class MessageViewModel : ViewModelBase
{
    private readonly MessageHandler messageHandler;
    private string previousSender;
    private bool loadHistory;
    private ObservableCollection<MessageData> history;
    public ObservableCollection<MessageData> Messages { get; set; }

    public MessageViewModel(MessageHandler messageHandler)
    {
        this.messageHandler = messageHandler;
        this.messageHandler.MessageReceived += MessageReceived;

        previousSender = "";
        loadHistory = true;
        history = [];
        Messages = [];
    }

    private void MessageReceived(object? sender, MessageEventArgs e)
    {
        if (e.Type == "History" && loadHistory)
        {
            history.Add(Utils.AddMessage(Messages, e, previousSender));
            previousSender = e.Sender;
            return;
        }

        if (e.Type == "History Done" && loadHistory)
        {
            loadHistory = false;
            Messages = new(history);
            return;
        }

        if (loadHistory || e.Type != "Chat")
        {
            return;
        }

        Messages.Add(Utils.AddMessage(Messages, e, previousSender));
        previousSender = e.Sender;
    }
}