using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KNRPG.UI
{
    public class MainMenuActionsController : Controller<MainMenuActionsView>
    {
        public override void Enable()
        {
            base.Enable();

            View.StartGameClickedEvent.AddListener(StartGame);
            View.LoadGameClickedEvent.AddListener(LoadGame);
            View.ShowOptionsClickedEvent.AddListener(ShowOptions);
            View.ShowCreditsClickedEvent.AddListener(ShowCredits);
            View.ExitGameClickedEvent.AddListener(ExitGame);
        }

        public override void Disable()
        {
            View.StartGameClickedEvent.RemoveListener(StartGame);
            View.LoadGameClickedEvent.RemoveListener(LoadGame);
            View.ShowOptionsClickedEvent.RemoveListener(ShowOptions);
            View.ShowCreditsClickedEvent.RemoveListener(ShowCredits);
            View.ExitGameClickedEvent.RemoveListener(ExitGame);

            base.Disable();
        }

        public void StartGame()
        {

        }

        public void LoadGame()
        {

        }

        public void ShowOptions()
        {

        }

        public void ShowCredits()
        {

        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}