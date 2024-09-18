namespace VirtualDragDrop;

public class FileDescriptor
{
    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the (optional) length of the file.
    /// </summary>
    public ulong? Length { get; set; }

    /// <summary>
    /// Gets or sets the (optional) change time of the file.
    /// </summary>
    public DateTime? ChangeTimeUtc { get; set; }

    /// <summary>
    /// Gets or sets an Action that returns the contents of the file.
    /// </summary>
    public Action<Stream>? StreamContents { get; set; }
}