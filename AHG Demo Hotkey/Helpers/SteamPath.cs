using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace AHG_Demo_Hotkey.Helpers;


public class SteamHelper {
    /// <summary>
    /// Gets the file path to Garry's Mod folder
    /// Modified from https://gist.github.com/moritzuehling/7f1c512871e193c0222f
    /// </summary>
    /// <returns>String of GMod path if found. Null if unable</returns>
    public string? GetGModPath() {
        string SteamPath = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Valve\\Steam", "SteamPath", "").ToString().Replace("/", "\\");
        string LibraryVDKFolders = Path.Combine(SteamPath, "steamapps", "libraryfolders.vdf");

        if (!File.Exists(LibraryVDKFolders)) {
            MessageBox.Show("Unable to find Steam libraries (libraryfolders.vdf cannot be found)");

            return null;
        }

        List<string> libraries = new List<string>();
        libraries.Add(Path.Combine(SteamPath));

        var PathVDF = File.ReadAllLines(LibraryVDKFolders);

        // Okay, this is not a full vdf-parser, but it seems to work pretty much, since the 
        // vdf-grammar is pretty easy. Hopefully it never breaks. I'm too lazy to write a full vdf-parser though. 
        Regex PathRegex = new Regex(@"\""(([^\""]*):\\([^\""]*))\""");

        foreach (var line in PathVDF) {
            if (PathRegex.IsMatch(line)) {
                string match = PathRegex.Matches(line)[0].Groups[1].Value;

                // De-Escape vdf. 
                libraries.Add(match.Replace("\\\\", "\\"));
            }
        }

        foreach (var library in libraries) {
            string GModPath = Path.Combine(library, "steamapps\\common\\GarrysMod\\garrysmod");
            if (Directory.Exists(GModPath)) {
                return GModPath;
            }
        }

        return null;
    }
}
