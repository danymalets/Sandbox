using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine.SceneManagement;

namespace _Project.Sources
{
    public static class NetcodeUtils
    {
        public static void RunServer(ushort port)
        {
            World serverWorld = ClientServerBootstrap.CreateServerWorld("ServerWorld");

            World.DefaultGameObjectInjectionWorld ??= serverWorld;
            
            SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);

            var networkStreamDriver = serverWorld.EntityManager
                .CreateEntityQuery(typeof(NetworkStreamDriver))
                .GetSingletonRW<NetworkStreamDriver>();

            networkStreamDriver.ValueRW.Listen(NetworkEndpoint.AnyIpv4.WithPort(port));

        }
        
        public static void RunClient(NetworkEndpoint networkEndpoint)
        {
            World clientWorld = ClientServerBootstrap.CreateClientWorld("ClientWorld");

            World.DefaultGameObjectInjectionWorld ??= clientWorld;
            
            SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
            
            var networkStreamDriver = clientWorld.EntityManager
                .CreateEntityQuery(typeof(NetworkStreamDriver))
                .GetSingletonRW<NetworkStreamDriver>();
            networkStreamDriver.ValueRW.Connect(clientWorld.EntityManager, networkEndpoint);
        }
    }
}