using System.Windows;

namespace MyApp.Support.UI.Units;

public class MyAppWindow : Window
{
    static MyAppWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MyAppWindow),
            new FrameworkPropertyMetadata(typeof(MyAppWindow)));
    }

    protected override void OnStateChanged(EventArgs e)
    {
        base.OnStateChanged(e);
        if (WindowState == WindowState.Maximized)
            MaxHeight = SystemParameters.WorkArea.Height;
        else
            MaxHeight = double.PositiveInfinity;
    }
}
