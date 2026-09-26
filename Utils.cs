using Avalonia.Controls;
using TheVoidRewrite.ViewModels;
using System.Collections.ObjectModel;
using TheVoidRewrite.Models;
using System;
using System.Globalization;

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

        return new MessageInfo(e.Sender + ' ', e.SenderNameColour, e.Time + '\n', e.Data);
    }

    public static string TimeToString(DateTime localTime)
    {
        int hour = localTime.Hour;
        string month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(localTime.Month)[0..^1];
        string daySection = "AM";

        if (localTime.Hour > 12)
        {
            hour -= 12;
            daySection = "PM";
        }

        string timeString = $"{month} {localTime.Day} {localTime.Year} — {hour}:{(localTime.Minute < 10 ? $"0{localTime.Minute}" : localTime.Minute)}{daySection}";

        return timeString;
    }
}