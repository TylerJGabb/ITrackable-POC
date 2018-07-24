using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINTEC_MSSO.Utilities.TabTracking
{
    /// <summary>
    /// Exposes methods for saving and checking unsaved changes.
    /// The ability to upcast to this interface from an ITrackableForm
    /// implies that this instance can have unsaved changes. 
    /// </summary>
    public interface ITrackableWithSaving : ITrackable
    {
        bool HasUnsavedChanges { get; }
        void SaveChanges();
    }
}
