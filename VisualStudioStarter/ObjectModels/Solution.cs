using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;

namespace VisualStudioStarter.ObjectModels;

public class Solution : INotifyPropertyChanged
{
    #region FIELDS

    private string _path = "";
    private FileInfo _fileinfo;
    private bool _isPinned;
    private string _directory;

    #endregion

    #region PROPS

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
    public FileInfo? Fileinfo
    {
        get => _fileinfo;
        set
        {
            if (SetField(ref _fileinfo, value))
            {
                OnPropertyChanged(nameof(Image));
                OnPropertyChanged(nameof(Directory));
            }
        }
    }

    [JsonIgnore] 
    public string Directory => System.IO.Path.GetDirectoryName(Fileinfo?.FullName ?? "") ?? "";
    [JsonIgnore] 
    public bool DirectoryExist => System.IO.Directory.Exists(Directory);

    [JsonIgnore]
    public ImageSource? PinnedImage => IsPinned
        ? Application.Current.FindResource("UnPinPNG") as ImageSource
        : Application.Current.FindResource("PinPNG") as ImageSource;

    [JsonIgnore]
    public ImageSource? Image =>
        Fileinfo.Extension switch
        {
            ".sln" => Application.Current.FindResource("SlnPNG") as ImageSource,
            ".csproj" => Application.Current.FindResource("CsProjPNG") as ImageSource,
            _ => null
        };

    #endregion

    #region CTOR

    public Solution()
    {
    }

    #endregion

    #region EVENTS

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

    #endregion

    #region METHODS

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion
}