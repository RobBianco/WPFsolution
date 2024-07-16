using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using VisualStudioStarter.ObjectModels;
using Path = System.IO.Path;

namespace VisualStudioStarter.Business;

public static class SolutionManager
{
    public static string SavePath
    {
        get
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var savePath = Path.Combine(appDataPath, "VsStarter.json");
            return savePath;
        }
    }

    public static List<Solution> GetSolutions() => File.Exists(SavePath)
        ? JsonSerializer.Deserialize<List<Solution>>(File.ReadAllText(SavePath), JsonSerializerOptions.Default) ?? []
        : [];

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

    public static void SaveSolutions(List<Solution> solutions)
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }

        var json = JsonSerializer.Serialize(
            solutions, new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(SavePath, json);
    }
}