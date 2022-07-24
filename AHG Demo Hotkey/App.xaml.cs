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

            // Create tray icon
            TrayIcon = (TaskbarIcon)FindResource("NotifyIcon");
        }

        protected override void OnExit(ExitEventArgs e) {
            TrayIcon.Dispose(); // Be sure TrayIcon is removed
            base.OnExit(e);
        }
    }
}