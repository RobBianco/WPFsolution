using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace VisualStudioStarter.ObjectModels;

public class Solution : INotifyPropertyChanged
{
    private string _path = "";
    private FileInfo _fileinfo;

    public string Path
    {
        get => _path;
        set
        {
            if (SetField(ref _path, value))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Fileinfo = new FileInfo(value);
                }
            }
        }
    }

    [JsonIgnore]
    public FileInfo Fileinfo
    {
        get => _fileinfo;
        set => SetField(ref _fileinfo, value);
    }

    public Solution(string filePath = "")
    {
        Path  = filePath.Trim();
        if (!string.IsNullOrEmpty(Path))
        {
            Fileinfo = new FileInfo(Path);
        }
    }

    public Solution()
    {
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}