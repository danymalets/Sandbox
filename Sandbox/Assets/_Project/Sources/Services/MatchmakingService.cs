using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Networking.Transport;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

namespace _Project.Sources.Services
{
    public class MatchmakingService
    {
        private readonly LogService<MatchmakingService> _logService;

        public MatchmakingService()
        {
            _logService = new LogService<MatchmakingService>();
        }

        public async UniTask RunMatchmaking(Action<float> inProgress, Action<NetworkEndpoint> onSuccess, Action<string> onFailed)
        {
            var players = new List<Player>
            {
                new (UnityEngine.Random.Range(0, 1_000_000_000).ToString(), new Dictionary<string, object>())
            };

            var options = new CreateTicketOptions(
                "DefaultQueue", 
                new Dictionary<string, object>());

            var ticketResponse = await MatchmakerService.Instance.CreateTicketAsync(players, options);

            _logService.Log($"Ticket {ticketResponse.Id}");

            bool ok = false;
            MultiplayAssignment assignment = null;
            bool gotAssignment = false;
            int timer = 0;
            do
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                timer++;
                
                inProgress?.Invoke(timer);
                
                var ticketStatus = await MatchmakerService.Instance.GetTicketAsync(ticketResponse.Id);
                if (ticketStatus == null)
                {
                    continue;
                }

                if (ticketStatus.Type == typeof(MultiplayAssignment))
                {
                    assignment = ticketStatus.Value as MultiplayAssignment;
                }


                switch (assignment?.Status)
                {
                    case MultiplayAssignment.StatusOptions.Found:
                        _logService.Log("Found ...");

                        ok = true;
                        gotAssignment = true;
                        break;
                    case MultiplayAssignment.StatusOptions.InProgress:
                        
                        _logService.Log($"Request in progress ... {assignment.AssignmentType} {assignment.Message} {assignment.Ip}:{assignment.Port}");
                        
                        break;
                    case MultiplayAssignment.StatusOptions.Failed:
                        gotAssignment = true;
                        _logService.Log("Failed to get ticket status. Error: " + assignment.Message);
                        break;
                    case MultiplayAssignment.StatusOptions.Timeout:
                        gotAssignment = true;
                        _logService.Log("Failed to get ticket status. Ticket timed out.");
                        break;
                    default:
                        throw new InvalidOperationException();
                }

            } while (!gotAssignment);

            if (ok)
            {
                _logService.Log($"Success mathcmaking {assignment.Ip}:{assignment.Port}");
                
                if (assignment.Port != null)
                {
                    onSuccess?.Invoke(NetworkEndpoint.Parse(assignment.Ip, (ushort)assignment.Port.Value));
                }
                else
                {
                    onFailed?.Invoke($"Failed to get port");
                }
            }
            else
            {
                onFailed?.Invoke($"error");
                
                _logService.Log($"Not Success mathcmaking");
            }
        }
    }
}