using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KNRPG.UI
{
    public class View : MonoBehaviour
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Resets view to starting values.
        /// </summary>
        public virtual void Reload()
        {

        }
    }
}