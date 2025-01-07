using System;
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
        [SerializeField] private Button _startServerButton;
        [SerializeField] private Button _startClientButton;
        [SerializeField] private Button _startBothButton;

        private void Awake()
        {
            _startServerButton.onClick.AddListener(OnStartServerButtonClicked);
            _startClientButton.onClick.AddListener(OnStartClientButtonClicked);
            _startBothButton.onClick.AddListener(OnStartBothButtonClicked);
        }

        private void OnStartServerButtonClicked()
        {
            
        }
        
        private void OnStartClientButtonClicked()
        {
            World clientWorld = ClientServerBootstrap.CreateClientWorld("ClientWorld");


            foreach (var world in World.All)
            {
                Debug.Log($"world {world.Flags}");
            }

            foreach (var world in World.All)
            {
                if (world.Flags == WorldFlags.Game)
                {
                    world.Dispose();
                    break;
                }
            }

            if (World.DefaultGameObjectInjectionWorld == null)
            {
                World.DefaultGameObjectInjectionWorld = clientWorld;
            }

            SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);


            ushort port = 7979;
            string ip = "192.168.100.21";

            //server

            
            //client
            NetworkEndpoint networkEndpoint = NetworkEndpoint.Parse(ip, port);
            var networkStreamDriver = clientWorld.EntityManager
                .CreateEntityQuery(typeof(NetworkStreamDriver))
                .GetSingletonRW<NetworkStreamDriver>();
            networkStreamDriver.ValueRW.Connect(clientWorld.EntityManager, networkEndpoint);
        }
        
        private void OnStartBothButtonClicked()
        {
            World serverWorld = ClientServerBootstrap.CreateServerWorld("ServerWorld");
            World clientWorld = ClientServerBootstrap.CreateClientWorld("ClientWorld");


            foreach (var world in World.All)
            {
                Debug.Log($"world {world.Flags}");
            }

            foreach (var world in World.All)
            {
                if (world.Flags == WorldFlags.Game)
                {
                    world.Dispose();
                    break;
                }
            }

            if (World.DefaultGameObjectInjectionWorld == null)
            {
                World.DefaultGameObjectInjectionWorld = serverWorld;
            }

            SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);


            ushort port = 7979;
            string ip = "192.168.100.21";


            
            //server
            var networkStreamDriver = serverWorld.EntityManager
                .CreateEntityQuery(typeof(NetworkStreamDriver))
                .GetSingletonRW<NetworkStreamDriver>();

            networkStreamDriver.ValueRW.Listen(NetworkEndpoint.AnyIpv4.WithPort(port));

            
            //client
            NetworkEndpoint networkEndpoint = NetworkEndpoint.Parse(ip, port);
            networkStreamDriver = clientWorld.EntityManager
                .CreateEntityQuery(typeof(NetworkStreamDriver))
                .GetSingletonRW<NetworkStreamDriver>();
            networkStreamDriver.ValueRW.Connect(clientWorld.EntityManager, networkEndpoint);
        }
    }
}