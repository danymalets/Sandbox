using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Sources.Ui
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;

        private void Awake()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            
        }
    }
}