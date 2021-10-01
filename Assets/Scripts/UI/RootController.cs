using UnityEngine;

namespace KNRPG.UI
{
    /// <summary>
    /// Root controller to manage other controllers.
    /// </summary>
    public abstract class RootController : MonoBehaviour
    {
        protected virtual void Start()
        {
            SetRootOnControllers();
        }

        /// <summary>
        /// Sets root to this instance on all used controllers.
        /// </summary>
        protected abstract void SetRootOnControllers();
    }
}