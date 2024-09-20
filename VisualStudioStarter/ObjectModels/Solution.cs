using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using VisualStudioStarter.Utils;

namespace VisualStudioStarter.ObjectModels;

public class Solution : INotifyPropertyChanged
{
    #region FIELDS

    private string _path = "";
    private FileInfo? _fileinfo;
    private bool _isPinned;

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

    public string Name
    {
        get => _name ?? Fileinfo?.Name ?? System.IO.Path.GetDirectoryName(Path) ?? "Error retriving path"; 
        set => SetField(ref _name, value);
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

    private VisualStudioVersion _defaultVersion = VisualStudioVersion.None;
    private string? _name;

    public VisualStudioVersion DefaultVersion
    {
        get => _defaultVersion;
        set
        {
            if (SetField(ref _defaultVersion, value))
            {
                OnPropertyChanged(nameof(DefaultVSImage));
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

    [JsonIgnore] public string Directory => System.IO.Path.GetDirectoryName(Fileinfo?.FullName ?? "") ?? "";
    [JsonIgnore] public bool DirectoryExist => System.IO.Directory.Exists(Directory);

    [JsonIgnore]
    public ImageSource? PinnedImage => IsPinned
        ? Application.Current.FindResource("UnPinPNG") as ImageSource
        : Application.Current.FindResource("PinPNG") as ImageSource;

    [JsonIgnore]
    public ImageSource? Image =>
        Fileinfo?.Extension switch
        {
            ".sln" => (ImageSource)Application.Current.FindResource("SlnPNG")!,
            ".csproj" => (ImageSource)Application.Current.FindResource("CsProjPNG")!,
            _ => null
        };

    [JsonIgnore]
    public ImageSource? DefaultVSImage
    {
        get
        {
            switch (DefaultVersion)
            {
                case VisualStudioVersion.VS2019: return (ImageSource)Application.Current.FindResource("Vs2019PNG")!;
                case VisualStudioVersion.VS2022: return (ImageSource)Application.Current.FindResource("Vs2022PNG")!;
                case VisualStudioVersion.VS2022Pre: return (ImageSource)Application.Current.FindResource("Vs2022PrePNG")!;
                case VisualStudioVersion.None:
                default: return null;
            }
        }
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