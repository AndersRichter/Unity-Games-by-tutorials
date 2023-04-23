using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Components
{
    public class CheatControllerComponent : MonoBehaviour
    {
        [SerializeField] private float inputTimeToLive;
        [SerializeField] private CheatItem[] cheats;
        
        private string _inputString;
        private float _inputTime;

        private void Awake()
        {
            Keyboard.current.onTextInput += OnTextInput;
        }

        private void OnDestroy()
        {
            Keyboard.current.onTextInput -= OnTextInput;
        }

        private void OnTextInput(char input)
        {
            _inputString += input;
            _inputTime = inputTimeToLive;

            ApplyCheat();
        }

        private void ApplyCheat()
        {
            foreach (var cheat in cheats)
            {
                if (_inputString.Contains(cheat.Combination))
                {
                    cheat.Action.Invoke();
                    _inputString = string.Empty;
                    break;
                }
            }
        }

        private void Update()
        {
            if (_inputTime < 0)
            {
                _inputString = string.Empty;
            }
            else
            {
                _inputTime -= Time.deltaTime;
            }
        }
    }
}

[Serializable]
public class CheatItem
{
    public string Name;
    public string Combination;
    public UnityEvent Action;
}
