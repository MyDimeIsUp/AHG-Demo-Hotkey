using System;
using System.Windows;
using System.Windows.Input;
using Hardcodet.Wpf.TaskbarNotification;
using System.Diagnostics;

namespace AHG_Demo_Hotkey {
    /// <summary>
    /// Simple application. Check the XAML for comments.
    /// </summary>
    public partial class App : Application {
        public TaskbarIcon TrayIcon { get; set; }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            TrayIcon = (TaskbarIcon)FindResource("NotifyIcon");
        }

        protected override void OnExit(ExitEventArgs e) {
            TrayIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }

    /// <summary>
    /// A simple command that displays the command parameter as
    /// a dialog message.
    /// </summary>
    public class ShowMessageCommand : ICommand {
        public void Execute(object parameter) {
            MessageBox.Show(parameter.ToString());
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}