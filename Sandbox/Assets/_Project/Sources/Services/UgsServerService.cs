#if UNITY_STANDALONE

using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.Entities;
using Unity.Services.Core;
using Unity.Services.Matchmaker.Models;
using Unity.Services.Multiplay;
using Unity.Services.Multiplayer;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Sources.Services
{
    public class UgsServerService
    {
        private readonly MatchService _matchService;
        private readonly IMultiplayService _multiplayService;

        private const ushort k_DefaultMaxPlayers = 10;
        private const string k_DefaultServerName = "MyServerExample";
        private const string k_DefaultGameType = "MyGameType";
        private const string k_DefaultBuildId = "MyBuildId";
        private const string k_DefaultMap = "MyMap";

        public ushort _currentPlayers;
        private IServerQueryHandler _serverQueryHandler;
        private readonly LogService<UgsServerService> _logService;
        private readonly ServerConfig _serverConfig;

        public UgsServerService()
        {
            _logService = new LogService<UgsServerService>();
            _matchService = new MatchService();
            _multiplayService = MultiplayService.Instance;
            _serverConfig = _multiplayService.ServerConfig;
        }

        public async void StartUgsServer()
        {
            Application.targetFrameRate = 60;
            
            var multiplayEventCallbacks = new MultiplayEventCallbacks();
            multiplayEventCallbacks.Allocate += OnAllocate;
            multiplayEventCallbacks.Deallocate += OnDeallocate;
            multiplayEventCallbacks.Error += OnError;
            multiplayEventCallbacks.SubscriptionStateChanged += OnSubscriptionStateChanged;

            _logService.Log($"Start Ugs Server");

            await MultiplayService.Instance.SubscribeToServerEventsAsync(multiplayEventCallbacks);

            _logService.Log($"SubscribeToServerEventsAsync");
            
            World.DisposeAllWorlds();

            await RunGameMatch();
            
            Application.Quit();
        }

        private async UniTask Update()
        {
            while (true)
            {
                if (_serverQueryHandler == null)
                {
                    return;
                }
                _serverQueryHandler.UpdateServerCheck();
                await UniTask.Yield();
            }
        }

        private async UniTask RunGameMatch()
        {
            
            _logService.Log($"Start ReadyServerForPlayersAsync");

            await _multiplayService.ReadyServerForPlayersAsync();

            _logService.Log($"ReadyServerForPlayersAsync ok");

            _matchService.RunServer(_serverConfig.Port);

            _logService.Log($"Server Runned");

            var payloadAllocation =
                await _multiplayService.GetPayloadAllocationFromJsonAs<MatchmakingResults>();

            _logService.Log(
                $"Payload {payloadAllocation.MatchId} {payloadAllocation.QueueName} {payloadAllocation.MatchProperties.Players.Count}");

            await _multiplayService.UnreadyServerAsync();
            
            _serverQueryHandler = await MultiplayService.Instance.StartServerQueryHandlerAsync(k_DefaultMaxPlayers,
                k_DefaultServerName, k_DefaultGameType, k_DefaultBuildId, k_DefaultMap);

            Update().Forget();
            
            for (int i = 0; i < 3; i++)
            {
                _logService.Log($"Game {i} minutes");
                await UniTask.Delay(TimeSpan.FromSeconds(60));
            }

            _logService.Log($"Game end, trying unready");

            await MultiplayService.Instance.UnreadyServerAsync();
        }

        /// <summary>
        /// Handler for receiving the allocation multiplay server event.
        /// </summary>
        /// <param name="allocation">The allocation received from the event.</param>
        private void OnAllocate(MultiplayAllocation allocation)
        {
            _logService.Log($"OnAllocate");

            // Here is where you handle the allocation.
            // This is highly dependent on your game, however this would typically be some sort of setup process.
            // Whereby, you spawn NPCs, setup the map, log to a file, or otherwise prepare for players.
            // After you the allocation has been handled, you can then call ReadyServerForPlayersAsync()!
        }

        /// <summary>
        /// Handler for receiving the deallocation multiplay server event.
        /// </summary>
        /// <param name="deallocation">The deallocation received from the event.</param>
        private void OnDeallocate(MultiplayDeallocation deallocation)
        {
            _logService.Log($"OnDeallocate");

            // Here is where you handle the deallocation.
            // This is highly dependent on your game, however this would typically be some sort of teardown process.
            // You might want to deactivate unnecessary NPCs, log to a file, or perform any other cleanup actions.
        }

        /// <summary>
        /// Handler for receiving the error multiplay server event.
        /// </summary>
        /// <param name="error">The error received from the event.</param>
        private void OnError(MultiplayError error)
        {
            _logService.Log($"OnError {error.Reason} {error.Detail}");

            // Here is where you handle the error.
            // This is highly dependent on your game. You can inspect the error by accessing the error.Reason and error.Detail fields.
            // You can change on the error.Reason field, log the error, or otherwise handle it as you need to.
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        private void OnSubscriptionStateChanged(MultiplayServerSubscriptionState state)
        {
            _logService.Log($"OnSubscriptionStateChanged {state}");

            switch (state)
            {
                case MultiplayServerSubscriptionState.Unsubscribed
                    : /* The Server Events subscription has been unsubscribed from. */ break;
                case MultiplayServerSubscriptionState.Synced
                    : /* The Server Events subscription is up to date and active. */ break;
                case MultiplayServerSubscriptionState.Unsynced
                    : /* The Server Events subscription has fallen out of sync, the subscription tries to automatically recover. */
                    break;
                case MultiplayServerSubscriptionState.Error
                    : /* The Server Events subscription has fallen into an errored state and won't recover automatically. */
                    break;
                case MultiplayServerSubscriptionState.Subscribing
                    : /* The Server Events subscription is trying to sync. */ break;
            }
        }
    }
}

#endif