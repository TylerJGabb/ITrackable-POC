using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Randoms
{
    //Minimal interface without the need to expose methods for saving
    public interface ITrackable
    {
        event EventHandler Enter;
        event FormClosedEventHandler FormClosed;
        event EventHandler Shown;

        string Text { get; }
        bool Focus();
    }
}
