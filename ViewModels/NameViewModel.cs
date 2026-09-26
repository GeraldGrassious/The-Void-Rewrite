using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Controls.Converters;

using TheVoidRewrite.Models;

namespace TheVoidRewrite.ViewModels;

public partial class NameViewModel(MessageHandler messageHandler) : ViewModelBase
{
    private readonly MessageHandler messageHandler = messageHandler;

    [ObservableProperty]
    private string _nameInputText = "";

    [ObservableProperty]
    private Avalonia.Media.Color _nameColour;

    [RelayCommand]
    public void SubmitName()
    {
        if (string.IsNullOrWhiteSpace(NameInputText) || string.IsNullOrEmpty(NameInputText))
        {
            return;
        }

        messageHandler.MessageLoop(NameInputText, ColorToHexConverter.ToHexString(NameColour, 0, false, true));
    }

}
