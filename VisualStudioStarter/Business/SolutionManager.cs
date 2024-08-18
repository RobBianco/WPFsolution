using Microsoft.WindowsAPICodePack.Dialogs;
using System.Text.Json;
using VisualStudioStarter.ObjectModels;
using Path = System.IO.Path;

namespace VisualStudioStarter.Business;

public static class SolutionManager
{
    public static string SavePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "VsStarter", "VsStarterSolutions.json");
    public static List<Solution> GetSolutions()
    {
        try
        {
            return File.Exists(SavePath)
                ? JsonSerializer.Deserialize<List<Solution>>(File.ReadAllText(SavePath), JsonSerializerOptions.Default) ??
                  []
                : [];
        }
        catch (Exception )
        {
            return [];
        }
    }

    public static List<Solution> FindSolution(bool IsFolderPicker)
    {
        var solutions = new List<Solution>();

        var openFileDialog = new CommonOpenFileDialog
        {
            InitialDirectory = "c:\\",
            RestoreDirectory = true,
            Multiselect = true,
            IsFolderPicker = IsFolderPicker
        };

        var res = openFileDialog.ShowDialog();
        if (res is not CommonFileDialogResult.Ok)
        {
            return solutions;
        }

        foreach (var path in openFileDialog.FileNames)
        {
            if (File.Exists(path))
            {
                solutions.Add(new Solution() { Path = path });
            }

            if (Directory.Exists(path))
            {
                solutions.AddRange(Directory.GetFiles(path, "*.sln", SearchOption.AllDirectories)
                    .Select(file => new Solution() { Path = file }));
            }
        }

        return solutions;
    }

    public static void SaveSolutions(IEnumerable<Solution>? pinnedSolutions, IEnumerable<Solution>? unpinnedSolutions)
    {
        var solutions = new List<Solution>();
        if (pinnedSolutions != null) solutions.AddRange(pinnedSolutions);
        if (unpinnedSolutions != null) solutions.AddRange(unpinnedSolutions);

        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }

        var json = JsonSerializer.Serialize(
            solutions, options: new()
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