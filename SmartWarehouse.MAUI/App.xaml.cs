namespace SmartWarehouse.MAUI;

public partial class App : Application
{
    [Obsolete("Obsolete")]
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }
}