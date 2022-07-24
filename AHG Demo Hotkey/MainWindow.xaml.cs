using AHG_Demo_Hotkey.Helpers;
using System;
using System.Windows;
using System.Windows.Input;

namespace AHG_Demo_Hotkey {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        KeyboardHook GlobalKeyHook = new KeyboardHook();
        DemoInput DemoHelper = new DemoInput();
        SteamHelper SteamHelper = new SteamHelper();

        Key curKey; // Currently selected key

        public MainWindow() {
            InitializeComponent();

            if (SteamHelper.GetGModPath() == null) {
                System.Environment.Exit(0);
            }

            Properties.Settings.Default.Save();
            HotkeyInput.Text = Enum.GetName(typeof(Key), Properties.Settings.Default.HotkeyKey);

            GlobalKeyHook.hook();
            GlobalKeyHook.KeyDown += new KeyEventHandler(GlobalHotKey_KeyDown);
        }

        /// <summary>
        /// Sets selected key and updates text box but does not save to program's properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HotkeyInput_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeyInput.Text = e.Key.ToString();
            curKey = (e.Key);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e) {
            GlobalKeyHook.HookedKeys.Clear();
            GlobalKeyHook.HookedKeys.Add(curKey);

            Properties.Settings.Default.HotkeyKey = (int)curKey;
            Properties.Settings.Default.Save();

            MessageBox.Show("Hotkey updated and saved", "Updated", MessageBoxButton.OK);
        }

        private void GlobalHotKey_KeyDown(object sender, KeyEventArgs e) {
            DemoHelper.SimulateConsoleInput(e, SteamHelper);
        }

        /// <summary>
        /// Used for showing and hiding tray icon. Run when WindowState is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_StateChanged(object sender, EventArgs e) {
            if (this.WindowState == WindowState.Minimized) {
                this.Hide();
                ((App)Application.Current).TrayIcon.Visibility = Visibility.Visible;
            } else {
                ((App)Application.Current).TrayIcon.Visibility = Visibility.Collapsed;
            }
        }
    }
}
