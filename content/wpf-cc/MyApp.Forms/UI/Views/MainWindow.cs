using System.Windows;
using System.Windows.Controls;
using MyApp.Support.UI.Units;

namespace MyApp.Forms.UI.Views;

public class MainWindow : MyAppWindow
{
    static MainWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MainWindow),
            new FrameworkPropertyMetadata(typeof(MainWindow)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var minimizeButton = GetTemplateChild("PART_MinimizeButton") as Button;
        if (minimizeButton != null)
            minimizeButton.Click += (s, e) => WindowState = WindowState.Minimized;

        var maximizeButton = GetTemplateChild("PART_MaximizeButton") as Button;
        if (maximizeButton != null)
            maximizeButton.Click += (s, e) =>
                WindowState = WindowState == WindowState.Maximized
                    ? WindowState.Normal
                    : WindowState.Maximized;

        var closeButton = GetTemplateChild("PART_CloseButton") as Button;
        if (closeButton != null)
            closeButton.Click += (s, e) => Close();

        var centerButton = GetTemplateChild("PART_CenterButton") as Button;
        if (centerButton != null)
            centerButton.Click += (s, e) => MessageBox.Show("Hello!", "MyApp");
    }
}
