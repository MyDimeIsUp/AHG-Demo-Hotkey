using System;
using System.Windows;
using System.Windows.Input;

namespace AHG_Demo_Hotkey;
/// <summary>
/// Provides bindable properties and commands for the NotifyIcon. In this sample, the
/// view model is assigned to the NotifyIcon in XAML. Alternatively, the startup routing
/// in App.xaml.cs could have created this view model, and assigned it to the NotifyIcon.
/// </summary>
public class TrayIconViewModel {
    /// <summary>
    /// Shows a window, if none is already open.
    /// </summary>
    public ICommand ShowWindowCommand {
        get {
            return new DelegateCommand {
                CanExecuteFunc = () => Application.Current.MainWindow.Visibility == Visibility.Hidden,
                CommandAction = () => {
                    Application.Current.MainWindow.Show();
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                }
            };
        }
    }


    public class DelegateCommand : ICommand {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter) {
            CommandAction();
        }

        public bool CanExecute(object parameter) {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
