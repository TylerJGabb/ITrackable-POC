using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MINTEC_MSSO.Utilities.TabTracking
{
    /// <summary>
    /// Used to facilitate the tracking of movement between forms in MSSO
    /// </summary>
    public static class TrackableController
    {

        private static ITrackable _prev = null;
        private static bool _blockEntrance = false;
        private static List<ITrackable> _watchedITrackables = new List<ITrackable>();

        /// <summary>
        /// Adds event handlers and tracks the passed ITrackable. Best practice is to call in the constructor of an ITrackableForm
        /// </summary>
        /// <param name="t"></param>
        public static void Register(ITrackable t)
        {
            if (t == null || t.IsBeingWatched()) return; //watched -> registered
            t.Enter += Entered;
            t.FormClosed += Closed;
            t.VisibleChanged += WatchIfVisible;
            Watch(t);
        }

        /// <summary>
        /// Removes event handlers from the passed ITrackable and removes from tracking. 
        /// </summary>
        /// <param name="t"></param>
        public static void UnRegister(ITrackable t)
        {
            if (t == null) return; //Don't need to check if Watched, just UnRegister
            t.Enter -= Entered;
            t.FormClosed -= Closed;
            t.VisibleChanged -= WatchIfVisible;
            Ignore(t);
        }

        /// <summary>
        /// Watches the ITrackable if it is visible, otherwise the ITrackable is ignored
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void WatchIfVisible(object sender, EventArgs e)
        {
            var casted = sender as ITrackable;
            if (casted != null && casted.Visible) Watch(casted);
            else Ignore(casted);
        }

        /// <summary>
        /// Facilitates control over what tab is being entered, and whether it is okay to leave the last one.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Entered(object sender, EventArgs e)
        {
            if (_blockEntrance) return;
            var casted = _prev as ITrackableWithSaving; //try to upcast to imply saveability
            var current = sender as ITrackable;
            if (casted == null || !casted.IsBeingWatched()  || !casted.HasUnsavedChanges || _prev == current)
            {
                _prev = current;
                return;
            }

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

        /// <summary>
        /// Attempts to UnRegister the ITrackable when closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Closed(object sender, FormClosedEventArgs e)
        {
            UnRegister(sender as ITrackable);
        }

        /// <summary>
        /// Leaves registered handlers intact but actions related to tracking will be ignored.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool Ignore(ITrackable t)
        {
            return _watchedITrackables.Remove(t);
        }

        /// <summary>
        /// Restores the execution of actions related to tracking
        /// </summary>
        /// <param name="t"></param>
        private static void Watch(ITrackable t)
        {
            if (t == null || t.IsBeingWatched()) return;
            _watchedITrackables.Add(t);
        }

        /// <summary>
        /// Shows a standardized dialogue asking the user what they want to do when attempting to leave an ITrackableWithSaving that has
        /// unsaved changes
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static RequestExitResult RequestExit(this ITrackable t)
        {
            var dr = MessageBox.Show(
                "You have unsaved changes in "+t.Text+". Do you wish to save?",
                "Save "+t.Text,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning); 

            switch(dr)
            {
                case DialogResult.Yes:
                    return RequestExitResult.AllowAndSave;
                case DialogResult.No:
                    return RequestExitResult.AllowWithoutSave;
                default:
                    return RequestExitResult.Deny;
            }
        }

        /// <summary>
        /// Tests whether or not the passed ITrackable is in the _watched List. Also an extension of ITrackable
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsBeingWatched(this ITrackable t)
        {
            return _watchedITrackables.Contains(t);
        }

    }

    public enum RequestExitResult
    {
        /// <summary>
        /// Granting permission to exit the ITrackable after saving
        /// </summary>
        AllowAndSave,

        /// <summary>
        /// Granting permission to exit the ITrackable without saving
        /// </summary>
        AllowWithoutSave,

        /// <summary>
        /// Denying permission to exit the ITrackable
        /// </summary>
        Deny
    }
}
