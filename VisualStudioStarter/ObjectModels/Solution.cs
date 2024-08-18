using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;

namespace VisualStudioStarter.ObjectModels;

public class Solution : INotifyPropertyChanged
{
    private string _path = "";
    private FileInfo _fileinfo;
    private bool _isPinned;

    public bool IsPinned
    {
        get => _isPinned;
        set
        {
            if (SetField(ref _isPinned, value)) OnPropertyChanged(nameof(PinnedImage));
        }
    }

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

    [JsonIgnore]
    public ImageSource? PinnedImage => IsPinned ? Application.Current.FindResource("UnPinPNG") as ImageSource : Application.Current.FindResource("PinPNG") as ImageSource;


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