using CommunityToolkit.Mvvm.ComponentModel;
using TheVoidRewrite.Models;

namespace TheVoidRewrite.ViewModels;

public class NameViewModel(MessageHandler messageHandler) : ViewModelBase
{
    private readonly MessageHandler messageHandler = messageHandler;
}
