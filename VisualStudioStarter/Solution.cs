using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace VisualStudioStarter;

public class Solution : INotifyPropertyChanged
{
    private String _path = "";

    public String Path
    {
        get => _path;
        set
        {
            if (SetField(ref _path, value))
            {
                OnPropertyChanged(nameof(Fileinfo));
            }
        }
    }

    [JsonIgnore]
    public FileInfo? Fileinfo => File.Exists(_path) ? new FileInfo(_path) : null;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] String? propertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

    protected Boolean SetField<T>(ref T field, T value, [CallerMemberName] String? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}