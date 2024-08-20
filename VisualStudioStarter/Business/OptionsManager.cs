using System.Text.Json;
using VisualStudioStarter.ObjectModels;

namespace VisualStudioStarter.Business;

public class OptionsManager
{
    #region FIELDS

    public static string SavePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "VsStarter", "VsStartOptions.json");

    private static OptionsManager? _instance;

    #endregion

    #region PROPS

    public static OptionsManager Instance => _instance ??= new();
    public VsStarterOptions Options { get; set; }

    public static bool CanSave { get; set; }

    #endregion

    #region CTOR

    public OptionsManager()
    {
        VsStarterOptions.OnOptionsChanged += (_, _, _) => SaveOptions(Options);
    }

    #endregion

    #region METHODS

    public static VsStarterOptions GetOptions() => File.Exists(SavePath)
        ? JsonSerializer.Deserialize<VsStarterOptions>(File.ReadAllText(SavePath), JsonSerializerOptions.Default) ?? VsStarterOptions.Default
        : VsStarterOptions.Default;

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

    #endregion
}