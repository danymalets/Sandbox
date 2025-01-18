using System.Collections;
using System.Linq;
using TMPro;
using Unity.Entities;
using Unity.Services.Core;
using UnityEngine;

namespace _Project.Sources.Ui
{
    public class ServerView : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI _portText;
        
        private async void Start()
        {

            await UnityServices.InitializeAsync();
            var commandLineArgs = System.Environment.GetCommandLineArgs();

            var index = commandLineArgs.ToList().IndexOf("-port");

            ushort port = index == -1 ? (ushort)7979 : ushort.Parse(commandLineArgs[index + 1]);
            _portText.text = port.ToString();
            

            // var config = MultiplayerService.Instance.ServerConfig;

            Application.targetFrameRate = 60;
            Debug.Log($"Start server port:{port}");
            World.DisposeAllWorlds();
            NetcodeUtils.RunServer(port);
        }
    }
}