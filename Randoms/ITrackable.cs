using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MINTEC_MSSO.Utilities.TabTracking
{
    /// <summary>
    /// Minimal interface without the need to expose methods for saving.
    /// Used for tracking movement between tabs within MSSO
    /// </summary>
    public interface ITrackable
    {
        event EventHandler Enter;
        event FormClosedEventHandler FormClosed;
        event EventHandler VisibleChanged;

        string Text { get; }
        bool Visible { get; }
        bool Focus();

    }
}
