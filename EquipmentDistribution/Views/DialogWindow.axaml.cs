using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace EquipmentDistribution.Views;

public partial class DialogWindow : Window
{
    public DialogWindow()
    {
        InitializeComponent();
    }

    public DialogWindow(string message, bool receiveInput = false) : this()
    {
        Message.Text = message;
        Input.IsVisible = receiveInput;
    }

    public string ReceivedInput => Input.Text ?? "";
}