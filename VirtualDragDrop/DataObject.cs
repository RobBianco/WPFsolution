namespace VirtualDragDrop;

/// <summary>
/// Class representing the result of a SetData call.
/// </summary>
public class DataObject
{
    /// <summary>
    /// FORMATETC structure for the data.
    /// </summary>
    public FORMATETC FORMATETC { get; set; }

    /// <summary>
    /// Func returning the data as an IntPtr and an HRESULT success code.
    /// </summary>
    public Func<(IntPtr, int)>? GetData { get; set; }
}