using CommunityToolkit.Mvvm.ComponentModel;
using TheVoidRewrite.Models;

namespace TheVoidRewrite.ViewModels;

public class MessageViewModel(MessageHandler messageHandler) : ViewModelBase
{
    private readonly MessageHandler messageHandler = messageHandler;
}