using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualStudioStarter.Utils
{
    internal  static class RegeditUtils
    {
        internal static string GetVSCodePath()
        {
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            // Concatenare il resto del percorso
            var vscodePath = Path.Combine(localAppDataPath, "Programs", "Microsoft VS Code","Code.exe");
            if (File.Exists(vscodePath))
            {
                return vscodePath;
            }

            // Percorso nel registro dove viene salvato l'installer di VSCode
            var registryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";
            var displayName = "Microsoft Visual Studio Code";
            using var key = Registry.LocalMachine.OpenSubKey(registryKeyPath);
            if (key != null)
            {
                foreach (var subkeyName in key.GetSubKeyNames())
                {
                    using var subkey = key.OpenSubKey(subkeyName);
                    // Controlla se il valore "DisplayName" contiene "Visual Studio Code"
                    if (subkey.GetValue("DisplayName") != null &&
                        subkey.GetValue("DisplayName").ToString().Contains(displayName))
                    {
                        // Recupera il percorso di installazione
                        return subkey.GetValue("InstallLocation")?.ToString();
                    }
                }
            }

            return null;
        }
    }
}
