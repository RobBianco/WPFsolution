using System.Text.Json;
using VisualStudioStarter.ObjectModels;

namespace VisualStudioStarter.Business;

public class OptionsManager
{
    private static OptionsManager? _instance;

    public static string SavePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "VsStarter", "VsStartOptions.json");

    public static OptionsManager Instance => _instance ??= new();
    public VsStarterOptions Options { get; set; }
    public static VsStarterOptions GetOptions() => File.Exists(SavePath)
        ? JsonSerializer.Deserialize<VsStarterOptions>(File.ReadAllText(SavePath), JsonSerializerOptions.Default) ?? VsStarterOptions.Default
        : VsStarterOptions.Default;

    public OptionsManager()
    {
        VsStarterOptions.OnOptionsChanged += (_, _, _) => SaveOptions(Options);
    }

    public static bool CanSave { get; set; }

    public static void LoadOptions() => Instance.Options = GetOptions();

    public static void SaveOptions(VsStarterOptions? options)
    {
        if (options == null || !CanSave) return;
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }

        var json = JsonSerializer.Serialize(
            options, options: new()
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