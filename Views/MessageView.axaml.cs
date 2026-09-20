using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace TheVoidRewrite.Views;

public partial class MessageView : UserControl
{
    public MessageView()
    {
        InitializeComponent();

        MessageInput.AddHandler(
            KeyDownEvent,  // Detects key down event
            MessageInput_KeyDown,  // Function to call when event is fired
            RoutingStrategies.Tunnel,  // Sends event before TextBox processes it
            true  // Allows handled events to be processed (Needed to work for some reason)
        );
    }

    private void MessageInput_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        if (e.KeyModifiers.HasFlag(KeyModifiers.Shift))
        {
            return;
        }

        e.Handled = true;
    }
}