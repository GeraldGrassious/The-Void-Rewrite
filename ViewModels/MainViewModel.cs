using CommunityToolkit.Mvvm.ComponentModel;
using TheVoidRewrite.Models;

namespace TheVoidRewrite.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Greeting { get; set; } = "Welcome to Avalonia!";

    MessageHandler messageHandler = new();
}
