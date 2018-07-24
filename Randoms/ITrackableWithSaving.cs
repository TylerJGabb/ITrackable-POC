using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randoms
{
    //Exposes methods for saving and checking saved changes
    public interface ITrackableWithSaving : ITrackable
    {
        bool HasUnsavedChanges { get; }
        void SaveChanges();
    }
}
