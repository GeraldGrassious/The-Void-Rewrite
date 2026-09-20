using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace TheVoidRewrite.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        MaximizeButton.Content = "☐";
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }

    private void MinimizeButton_Clicked(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MaximizeButton_Clicked(object sender, RoutedEventArgs e)
    {
        var maximizeButton = (Button) sender;

        if (WindowState is WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
            maximizeButton.Content = "☐";
        }
        else
        {
            WindowState = WindowState.Maximized;
            maximizeButton.Content = "🗗";
        }
    }


    private void CloseButton_Clicked(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}