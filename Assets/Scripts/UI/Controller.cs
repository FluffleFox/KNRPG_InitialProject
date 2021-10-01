using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KNRPG.UI
{
    /// <summary>
    /// Base class for UI controllers.
    /// </summary>
    public abstract class Controller : MonoBehaviour
    {
        public RootController RootController { get; private set; }

        /// <summary>
        /// Is controller currently enabled?
        /// </summary>
        public bool IsEnabled { get; protected set; }

        public virtual void Enable()
        {
            IsEnabled = true;
        }

        public virtual void Disable()
        {
            IsEnabled = false;
        }

        /// <summary>
        /// Sets provided RootController as controller's root controller if previous was null.
        /// </summary>
        public void SetRootController(RootController rootController)
        {
            if (RootController != null)
                return;

            RootController = rootController;
        }
    }

    /// <summary>
    /// Base class for UI controllers with specified View type.
    /// </summary>
    public abstract class Controller<TView> : Controller where TView : View
    {
        [field: SerializeField]
        public TView View { get; protected set; }

        public override void Enable()
        {
            base.Enable();

            View.Reload();
            View.Show();
        }

        public override void Disable()
        {
            View.Hide();

            base.Disable();
        }
    }
}