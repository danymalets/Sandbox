#if UNITY_STANDALONE


using _Project.Sources.Services;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Sources.Entries
{
    public class ServerEntry : MonoBehaviour
    {
        private async void Awake()
        {
            
            await UnityServices.InitializeAsync();
            SceneManager.LoadSceneAsync("ServerLobbyScene", LoadSceneMode.Single);
            new UgsServerService().StartUgsServer();
        }
    }
}

#endif
