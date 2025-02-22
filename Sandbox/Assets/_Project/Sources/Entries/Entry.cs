using _Project.Sources.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Sources.Entries
{
    public class Entry : MonoBehaviour
    {
        private async void Awake()
        {
            await new AuthenticationWrapper().Initialize();
            SceneManager.LoadSceneAsync("LobbyScene", LoadSceneMode.Single);
        }
    }
}