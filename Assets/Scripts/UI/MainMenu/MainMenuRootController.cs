using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KNRPG.UI
{
    public class MainMenuRootController : RootController
    {
        public enum MainMenuStage { Actions, Start, Load, Options, Credits, Exit }

        [Header("Controllers")]
        [SerializeField] private MainMenuActionsController _menuActionsController;

        private MainMenuStage _menuStage = MainMenuStage.Actions;

        protected override void SetRootOnControllers()
        {
            _menuActionsController.SetRootController(this);
        }

        /// <summary>
        /// Changes current root controller stage by disabling all controllers, except ones that are specified for this stage.
        /// </summary>
        public void ChangeStage(MainMenuStage mainMenuStage)
        {
            DisableControllers();

            _menuStage = mainMenuStage;

            switch (_menuStage)
            {
                case MainMenuStage.Actions:
                    _menuActionsController.Enable();
                    break;
                case MainMenuStage.Start:
                    break;
                case MainMenuStage.Load:
                    break;
                case MainMenuStage.Options:
                    break;
                case MainMenuStage.Credits:
                    break;
                case MainMenuStage.Exit:
                    break;
                default:
                    break;
            }
        }

        private void DisableControllers()
        {
            _menuActionsController.Disable();
        }
    }
}