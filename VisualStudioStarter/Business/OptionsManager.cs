using System.Text.Json;
using VisualStudioStarter.ObjectModels;

namespace VisualStudioStarter.Business;

internal static class OptionsManager
{
    public static string SavePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "VsStarter", "VsStartOptions.json");

    public static VsStarterOptions GetOptions() => File.Exists(SavePath)
        ? JsonSerializer.Deserialize<VsStarterOptions>(File.ReadAllText(SavePath), JsonSerializerOptions.Default) ?? VsStarterOptions.Default
        : VsStarterOptions.Default;

    public static void SaveOptions(VsStarterOptions type)
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }

        var json = JsonSerializer.Serialize(
            type, options: new()
            {
                WriteIndented = true
            });

        if (!Directory.Exists(Path.GetDirectoryName(SavePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SavePath)!);
        }

        File.WriteAllText(SavePath, json);
    }
}