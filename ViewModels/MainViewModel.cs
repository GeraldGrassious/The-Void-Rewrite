using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TheVoidRewrite.Models;

namespace TheVoidRewrite.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private MessageHandler messageHandler = new();

    [ObservableProperty]
    private ObservableObject _currentPage;

    public MainViewModel()
    {
        _currentPage = new NameViewModel(messageHandler);
    }

    [RelayCommand]
    private void GoName() => CurrentPage = new NameViewModel(messageHandler);

    [RelayCommand]
    private void GoMessage() => CurrentPage = new MessageViewModel(messageHandler);
}