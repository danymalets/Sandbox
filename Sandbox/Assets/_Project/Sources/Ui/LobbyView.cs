using _Project.Sources.Services;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Entities;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Sources.Ui
{
    public class LobbyView : MonoBehaviour
    {
        private static ushort Port = 7979;

        [SerializeField] private Button _startServerButton;
        [SerializeField] private Button _startClientButton;
        [SerializeField] private Button _startBothButton;
        [SerializeField] private Button _matchmakingButton;
        private TextMeshProUGUI _matchmakingText;
        private MatchmakingService _matchmakingService;
        private MatchService _matchService;

        private void Start()
        {
            _matchmakingText = _matchmakingButton.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);

            _matchmakingService = new MatchmakingService();
            _matchService = new MatchService();

            World.DisposeAllWorlds();

            _startServerButton.onClick.AddListener(OnStartServerButtonClicked);
            _startClientButton.onClick.AddListener(OnStartClientButtonClicked);
            _startBothButton.onClick.AddListener(OnStartBothButtonClicked);
            _matchmakingButton.onClick.AddListener(OnMatchmakingButtonClicked);
        }


        private void OnStartServerButtonClicked()
        {
            _matchService.RunServer(Port);
        }

        private void OnStartClientButtonClicked()
        {
            _matchService.RunClient(NetworkEndpoint.Parse("20.33.78.226", 9000));
        }

        private void OnStartBothButtonClicked()
        {
            _matchService.RunServer(Port);
            _matchService.RunClient(NetworkEndpoint.LoopbackIpv4.WithPort(Port));
        }

        private async void OnMatchmakingButtonClicked()
        {
            _matchmakingButton.interactable = false;

            _matchmakingText.text = $"Start Finding";

            _matchmakingService.RunMatchmaking(
                (progress) =>
                {
                    _matchmakingText.text = $"Finding... {progress}s";
                },
                (networkEndpoint) =>
                {
                    _matchService.RunClient(networkEndpoint);
                },
                (errorMessage) =>
                {
                    _matchmakingText.text = $"Error {errorMessage}";
                    _matchmakingButton.interactable = true;
                }).Forget();
        }
    }
}