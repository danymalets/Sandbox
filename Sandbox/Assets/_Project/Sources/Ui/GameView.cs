using _Project.Sources.Services;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Sources.Ui
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        private MatchService _matchService;

        private void Awake()
        {
            _matchService = new MatchService();
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            _matchService.ExitToClientLobby();
        }
    }
}