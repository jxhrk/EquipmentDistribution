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

    public DialogWindow(string message) : this()
    {
        Message.Text = message;
    }
}