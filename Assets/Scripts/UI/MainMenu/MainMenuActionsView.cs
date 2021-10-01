using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KNRPG.UI
{
    public class MainMenuActionsView : View
    {
        [Header("Buttons")]
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _loadGameButton;
        [SerializeField] private Button _showOptionsButton;
        [SerializeField] private Button _showCreditsButton;
        [SerializeField] private Button _exitGameButton;

        [Header("Events")]
        public UnityEvent StartGameClickedEvent;
        public UnityEvent LoadGameClickedEvent;
        public UnityEvent ShowOptionsClickedEvent;
        public UnityEvent ShowCreditsClickedEvent;
        public UnityEvent ExitGameClickedEvent;

        public override void Show()
        {
            base.Show();

            PrepareEvents();
        }

        public override void Hide()
        {
            ClearEvents();

            base.Hide();
        }

        public override void Reload()
        {
            ClearEvents();

            base.Reload();
        }

        private void PrepareEvents()
        {
            _startGameButton.onClick.AddListener(() => StartGameClickedEvent?.Invoke());
            _loadGameButton.onClick.AddListener(() => LoadGameClickedEvent?.Invoke());
            _showOptionsButton.onClick.AddListener(() => ShowOptionsClickedEvent?.Invoke());
            _showCreditsButton.onClick.AddListener(() => ShowCreditsClickedEvent?.Invoke());
            _exitGameButton.onClick.AddListener(() => ExitGameClickedEvent?.Invoke());
        }

        private void ClearEvents()
        {
            StartGameClickedEvent.RemoveAllListeners();
            LoadGameClickedEvent.RemoveAllListeners();
            ShowOptionsClickedEvent.RemoveAllListeners();
            ShowCreditsClickedEvent.RemoveAllListeners();
            ExitGameClickedEvent.RemoveAllListeners();
        }
    }
}