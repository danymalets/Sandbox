using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine.SceneManagement;

namespace _Project.Sources.Services
{
    public class MatchService
    {
        public void RunServer(ushort port)
        {
            World serverWorld = ClientServerBootstrap.CreateServerWorld("ServerWorld");

            World.DefaultGameObjectInjectionWorld ??= serverWorld;
            
            SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);

            var networkStreamDriver = serverWorld.EntityManager
                .CreateEntityQuery(typeof(NetworkStreamDriver))
                .GetSingletonRW<NetworkStreamDriver>();

            networkStreamDriver.ValueRW.Listen(NetworkEndpoint.AnyIpv4.WithPort(port));
        }
        
        public void RunClient(NetworkEndpoint networkEndpoint)
        {
            World clientWorld = ClientServerBootstrap.CreateClientWorld("ClientWorld");

            World.DefaultGameObjectInjectionWorld ??= clientWorld;
            
            SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
            
            var networkStreamDriver = clientWorld.EntityManager
                .CreateEntityQuery(typeof(NetworkStreamDriver))
                .GetSingletonRW<NetworkStreamDriver>();
            networkStreamDriver.ValueRW.Connect(clientWorld.EntityManager, networkEndpoint);
        }

        public void ExitToClientLobby()
        {
            SceneManager.LoadSceneAsync("LobbyScene", LoadSceneMode.Single);
        }
        
        public void ExitToServerLobby()
        {
            SceneManager.LoadSceneAsync("ServerLobbyScene", LoadSceneMode.Single);
        }
    }
}