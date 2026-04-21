using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MyApp.Forms.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [RelayCommand]
    private void ShowMessage()
    {
        MessageBox.Show("Hello!", "MyApp");
    }
}
