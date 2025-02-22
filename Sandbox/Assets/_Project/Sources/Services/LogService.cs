using System;
using System.Globalization;
using UnityEngine;

namespace _Project.Sources.Services
{
    public class LogService<T> where T : class
    {
        private readonly float _deltaHour;

        public LogService(bool isLocalTime = false)
        {
            _deltaHour = isLocalTime ? (float)(DateTime.Now - DateTime.UtcNow).TotalHours : 0f;
        }

        public void Log(string message) => Debug.Log(GetLogText(message));
        public void LogWarning(string message) => Debug.LogWarning($"[**WARNING**] {GetLogText(message)}");
        public void LogError(string message) => Debug.LogWarning($"[****ERROR****] {GetLogText(message)}");

        private string GetLogText(string message) => 
            $"[{DateTime.UtcNow.AddHours(_deltaHour).ToString(@"MM\/dd\/yyyy HH:mm:ss.fff",CultureInfo.InvariantCulture)} UTC+{_deltaHour}] <{typeof(T).Name}> {message}";
    }
}