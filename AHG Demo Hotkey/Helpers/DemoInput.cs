using System;
using System.Diagnostics;
using System.Windows.Input;
using System.IO;
using WindowsInput;

namespace AHG_Demo_Hotkey.Helpers;
public class DemoInput {
    GetActiveWindow ActiveWindowHelper = new GetActiveWindow();
    public void SimulateConsoleInput(KeyEventArgs e, SteamHelper SteamHelper) {
        string ActiveProcessName = ActiveWindowHelper.GetProcessName();
        string GModPath = SteamHelper.GetGModPath();

        if (ActiveProcessName == "gmod" || ActiveProcessName == "hl2") {
            DateTime DateTime = DateTime.Now;
            string AHGDemosPath = Path.Combine(GModPath, "demos", "AHG");

            // If our AHG dir doesn't exist, create it
            if (!Directory.Exists(AHGDemosPath)) {
                Directory.CreateDirectory(AHGDemosPath);
            }

            // Now check if our month, date, and hour directory exist. God forgive me for this
            if (!Directory.Exists(Path.Combine(AHGDemosPath, DateTime.Year.ToString()))) {
                Directory.CreateDirectory(Path.Combine(AHGDemosPath, DateTime.Year.ToString()));
            }

            if (!Directory.Exists(Path.Combine(AHGDemosPath, DateTime.Year.ToString(), DateTime.ToString("MMMM")))) {
                Directory.CreateDirectory(Path.Combine(AHGDemosPath, DateTime.Year.ToString(), DateTime.ToString("MMMM")));
            }

            if (!Directory.Exists(Path.Combine(AHGDemosPath, DateTime.Year.ToString(), DateTime.ToString("MMMM"), DateTime.Day.ToString()))) {
                Directory.CreateDirectory(Path.Combine(AHGDemosPath, DateTime.Year.ToString(), DateTime.ToString("MMMM"), DateTime.Day.ToString()));
            }

            if (!Directory.Exists(Path.Combine(AHGDemosPath, DateTime.Year.ToString(), DateTime.ToString("MMMM"), DateTime.Day.ToString(), DateTime.Hour.ToString()))) {
                Directory.CreateDirectory(Path.Combine(AHGDemosPath, DateTime.Year.ToString(), DateTime.ToString("MMMM"), DateTime.Day.ToString(), DateTime.Hour.ToString()));
            }

            string DemoPath = Path.Combine("demos", "AHG", DateTime.Year.ToString(), DateTime.ToString("MMMM"), DateTime.Day.ToString(), DateTime.Hour.ToString()).Replace("\\", "/");

            var KeySimulator = new InputSimulator();

            KeySimulator.Keyboard.TextEntry("record " + DemoPath + "/" + DateTime.ToString("hh-mm-sstt"));
            KeySimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            e.Handled = true;
        }
    }
}