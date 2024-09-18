using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using IDataObject = System.Windows.IDataObject;

namespace DragDropExample;
public static class CustomDataFormats
{
    public static readonly string DownloadFileFormat = "DownloadFileFormat";
}
public class CustomDataObject : IDataObject
{
    private readonly Action<Stream> _streamAction;

    // Costruttore che riceve un'azione per scrivere dati nello stream
    public CustomDataObject(Action<Stream> streamAction)
    {
        DependencyObject
        _streamAction = streamAction;
    }

    // Implementazioni per IDataObject
    public void SetData(string format, object data, bool autoConvert) { /* No-op */ }
    public void SetData(string format, object data) { /* No-op */ }
    public void SetData(Type format, object data) { /* No-op */ }
    public void SetData(object data) { /* No-op */ }

    public object GetData(string format, bool autoConvert) { return null; }
    public object GetData(string format) { return null; }
    public object GetData(Type format) { return null; }
    public bool GetDataPresent(string format, bool autoConvert) { return format == CustomDataFormats.DownloadFileFormat; }
    public bool GetDataPresent(string format) { return format == CustomDataFormats.DownloadFileFormat; }
    public bool GetDataPresent(Type format) { return format == typeof(Stream); }
    public string[] GetFormats(bool autoConvert) { return new string[] { CustomDataFormats.DownloadFileFormat }; }
    public string[] GetFormats() { return new string[] { CustomDataFormats.DownloadFileFormat }; }

    public void GetDataHere(ref FORMATETC format, ref STGMEDIUM medium)
    {
        if (format.cfFormat == (short)DataFormats.GetDataFormat(CustomDataFormats.DownloadFileFormat).Id)
        {
            medium.tymed = TYMED.TYMED_ISTREAM;
            var stream = new MemoryStream();
            _streamAction(stream);
            medium.unionmember = Marshal.AllocHGlobal((int)stream.Length);
            Marshal.Copy(stream.ToArray(), 0, medium.unionmember, (int)stream.Length);
        }
        else
        {
            throw new NotSupportedException("Formato non supportato.");
        }
    }

    public int DAdvise(ref FORMATETC pFormatetc, ADVF advf, IAdviseSink adviseSink, out int connection)
    {
        connection = 0;
        return 1; // E_NOTIMPL
    }

    public void DUnadvise(int connection) { /* No-op */ }
    public int EnumDAdvise(out IEnumSTATDATA enumAdvise) { enumAdvise = null; return 1; } // OLE_E_ADVISENOTSUPPORTED
    public IEnumFORMATETC EnumFormatEtc(DATADIR direction) { return null; }
    public int QueryGetData(ref FORMATETC format) { return format.cfFormat == (short)DataFormats.GetDataFormat(CustomDataFormats.DownloadFileFormat).Id ? 0 : -2147221404; } // DV_E_FORMATETC
    public void SetData(ref FORMATETC format, ref STGMEDIUM medium, bool release) { /* No-op */ }
}
