namespace VirtualDragDrop;

/// <summary>
/// Contains the methods for generating visual feedback to the end user and for canceling or completing the drag-and-drop operation.
/// </summary>
public class DropSource : NativeMethods.IDropSource
{
    /// <summary>
    /// Determines whether a drag-and-drop operation should continue.
    /// </summary>
    /// <param name="fEscapePressed">Indicates whether the Esc key has been pressed since the previous call to QueryContinueDrag or to DoDragDrop if this is the first call to QueryContinueDrag. A TRUE value indicates the end user has pressed the escape key; a FALSE value indicates it has not been pressed.</param>
    /// <param name="grfKeyState">The current state of the keyboard modifier keys on the keyboard. Possible values can be a combination of any of the flags MK_CONTROL, MK_SHIFT, MK_ALT, MK_BUTTON, MK_LBUTTON, MK_MBUTTON, and MK_RBUTTON.</param>
    /// <returns>This method returns S_OK/DRAGDROP_S_DROP/DRAGDROP_S_CANCEL on success.</returns>
    public int QueryContinueDrag(int fEscapePressed, uint grfKeyState)
    {
        var escapePressed = (0 != fEscapePressed);
        var keyStates = (DragDropKeyStates)grfKeyState;
        if (escapePressed)
        {
            return NativeMethods.DRAGDROP_S_CANCEL;
        }
        else if (DragDropKeyStates.None == (keyStates & DragDropKeyStates.LeftMouseButton))
        {
            return NativeMethods.DRAGDROP_S_DROP;
        }
        return NativeMethods.S_OK;
    }

    /// <summary>
    /// Gives visual feedback to an end user during a drag-and-drop operation.
    /// </summary>
    /// <param name="dwEffect">The DROPEFFECT value returned by the most recent call to IDropTarget::DragEnter, IDropTarget::DragOver, or IDropTarget::DragLeave. </param>
    /// <returns>This method returns S_OK on success.</returns>
    public int GiveFeedback(uint dwEffect)
    {
        return NativeMethods.DRAGDROP_S_USEDEFAULTCURSORS;
    }
}