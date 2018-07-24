using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Randoms
{
    public static class TrackableController
    {

        private static ITrackable _prev = null;
        private static bool _blockEntrance = false;
        private static List<ITrackable> _tracking = new List<ITrackable>();


        public static void Track(ITrackable t)
        {
            if (t.IsTracked()) return;
            t.Enter += Entered;
            t.Shown += Entered;
            t.FormClosed += Closed;
            _tracking.Add(t);
        }

        private static void UnTrack(ITrackable t)
        {
            if (t == null || !t.IsTracked()) return;
            t.Enter -= Entered;
            t.Shown -= Entered;
            t.FormClosed -= Closed;
            _tracking.Remove(t);
        }

        public static void Entered(object sender, EventArgs e)
        {
            if (_blockEntrance || !(sender is ITrackable current)) return;
            //try to upcast to imply saveability
            if (!(_prev is ITrackableWithSaving casted) || !casted.HasUnsavedChanges || _prev == current)
            {
                _prev = current;
                return;
            }

            //_prev is ITrackableWith Saving and hasUnsavedChanges and != previous
            //If we reach this point, then _prev is ITrackableWithSaving, with unsaved changes, and is diff from previous

            _blockEntrance = true;
            _prev.Focus();
            switch (_prev.RequestExit())
            {
                case RequestExitResult.AllowAndSave:
                    casted.SaveChanges();
                    _prev = current;
                    break;
                case RequestExitResult.AllowWithoutSave:
                    _prev = current;
                    break;
            }
            _prev.Focus();
            _blockEntrance = false;
        }

        private static void Closed(object sender, FormClosedEventArgs e)
        {
            UnTrack(sender as ITrackable);
        }

        private static RequestExitResult RequestExit(this ITrackable t)
        {
            switch (MessageBox.Show($"do u want save {t.Text}", $"do u want save {t.Text}", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning))
            {
                case DialogResult.Yes:
                    return RequestExitResult.AllowAndSave;
                case DialogResult.No:
                    return RequestExitResult.AllowWithoutSave;
                default:
                    return RequestExitResult.Deny;
            }
        }

        private static bool IsTracked(this ITrackable t) => _tracking.Contains(t);
    }

    public enum RequestExitResult
    {
        AllowAndSave,
        AllowWithoutSave,
        Deny
    }
}
