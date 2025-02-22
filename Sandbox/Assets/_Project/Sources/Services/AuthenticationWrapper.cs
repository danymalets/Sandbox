using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace _Project.Sources.Services
{
    public class AuthenticationWrapper
    {
        private readonly LogService<AuthenticationWrapper> _logService;
        
        public AuthenticationWrapper()
        {
            _logService = new LogService<AuthenticationWrapper>();
        } 
        
        public async UniTask Initialize()
        {
            var initOptions = new InitializationOptions();
            var profile = UnityEngine.Random.Range(0, 1_000_000_000).ToString();
            _logService.Log($"signing {profile}");
            
            initOptions.SetProfile(profile);
            await UnityServices.InitializeAsync(initOptions);

            SetupEvents();
            
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
        void SetupEvents() {
            AuthenticationService.Instance.SignedIn += () => {
                // Shows how to get a playerID
                _logService.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

                // Shows how to get an access token
                _logService.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
                
            };

            AuthenticationService.Instance.SignInFailed += (err) => {
                _logService.Log(err.Message);
            };

            AuthenticationService.Instance.SignedOut += () => {
                _logService.Log("Player signed out.");
            };

            AuthenticationService.Instance.Expired += () =>
            {
                _logService.Log("Player session could not be refreshed and expired.");
            };
        }
    }
}