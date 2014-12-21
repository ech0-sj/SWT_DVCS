using Controller;
using System.Windows;

namespace ViewModel
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainController maincontroller = new MainController();
            MainViewModel mainViewModel = new MainViewModel(maincontroller);
            MainWindow mainwindow = new MainWindow();
            mainwindow.DataContext = mainViewModel;
            mainwindow.Show();
        }
    }
}