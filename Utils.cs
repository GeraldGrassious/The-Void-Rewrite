using Avalonia.Controls;
using TheVoidRewrite.ViewModels;
using System.Collections.ObjectModel;
using TheVoidRewrite.Models;


namespace TheVoidRewrite;

public static class Utils
{
    public static void ScrollToBottomListBox(ListBox listBox)
    {
        var bottomItem = listBox.Items[listBox.Items.Add("")];

        if (bottomItem is not null)
        {
            listBox.ScrollIntoView(bottomItem);
            listBox.Items.Remove(bottomItem);
        }
    }

    public static MessageData AddMessage(ObservableCollection<MessageData> messageCollection, MessageEventArgs e, string previousSender)
    {
        if (e.Sender == previousSender)
        {
            return new MessageData(e.Data);
        }

        return new MessageInfo(e.Sender, e.SenderNameColour, e.Time, e.Data);
    }
}