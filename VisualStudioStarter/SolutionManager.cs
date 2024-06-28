using System.IO;
using System.Text.Json;

namespace VisualStudioStarter
{
    public static class SolutionManager
    {
        public static String SavePath
        {
            get
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                var savePath = Path.Combine(appDataPath, "VsStarter.json");
                return savePath;
            }
        }

        public static List<WorkSpace> GetWorkSpaces() => File.Exists(SavePath)
            ? JsonSerializer.Deserialize<List<WorkSpace>>(File.ReadAllText(SavePath), JsonSerializerOptions.Default) ?? []
            : [];

        public static void SaveWorkspaces(List<WorkSpace> workspaces)
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }

            var json = JsonSerializer.Serialize(
                workspaces, JsonSerializerOptions.Default);

            File.WriteAllText(SavePath, json);
        }

        public static List<Solution> GetSolutions(String pathSolutions)
        {
            var res = new List<Solution>();

            if (Directory.Exists(pathSolutions))
            {
                var directories = Directory.EnumerateDirectories(pathSolutions);
                res.AddRange(
                    from directory in directories
                    from file in Directory.EnumerateFiles(directory).Select(x => new FileInfo(x))
                    where file.Extension == ".sln"
                    select new Solution { Path = file.FullName });
            }

            return res;
        }
    }
}
