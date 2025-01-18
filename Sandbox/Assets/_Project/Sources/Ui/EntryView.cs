using System;
using System.Linq;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Sources.Ui
{
    public class EntryView : MonoBehaviour
    {
        private static ushort Port = 7979;
        
        [SerializeField] private Button _startServerButton;
        [SerializeField] private Button _startClientButton;
        [SerializeField] private Button _startBothButton;

        private void Awake()
        {
            World.DisposeAllWorlds();
            
            _startServerButton.onClick.AddListener(OnStartServerButtonClicked);
            _startClientButton.onClick.AddListener(OnStartClientButtonClicked);
            _startBothButton.onClick.AddListener(OnStartBothButtonClicked);
        }

        private void OnStartServerButtonClicked()
        {
            NetcodeUtils.RunServer(Port);
        }
        
        private void OnStartClientButtonClicked()
        {
            Debug.Log($"start entry");
            
            NetcodeUtils.RunClient(NetworkEndpoint.Parse("20.33.78.226",9000));
        }

        private void OnStartBothButtonClicked()
        {
            NetcodeUtils.RunServer(Port);
            NetcodeUtils.RunClient(NetworkEndpoint.LoopbackIpv4.WithPort(Port));
        }
    }
}