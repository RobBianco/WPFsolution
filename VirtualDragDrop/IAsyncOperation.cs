namespace VirtualDragDrop;

/// <summary>
/// Definition of the IAsyncOperation COM interface.
/// </summary>
/// <remarks>
/// Pseudo-public because VirtualFileDataObject implements it.
/// </remarks>
[ComImport]
[Guid("3D8B0590-F691-11d2-8EA9-006097DF5BD4")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IAsyncOperation
{
    void SetAsyncMode([In] int fDoOpAsync);
    void GetAsyncMode([Out] out int pfIsOpAsync);
    void StartOperation([In] IBindCtx pbcReserved);
    void InOperation([Out] out int pfInAsyncOp);
    void EndOperation([In] int hResult, [In] IBindCtx pbcReserved, [In] uint dwEffects);
}