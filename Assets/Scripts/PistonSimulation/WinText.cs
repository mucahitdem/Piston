using System;
using UnityEngine;
using UnityEngine.UI;

namespace PistonSimulation
{
    public class WinText : MonoBehaviour
    {
        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
            EnableText(false);
        }


        private void OnEnable()
        {
            GameManager.Instance.ReplaceManager.onAllPiecesReplaced += EnableText;
        }
        
        
        private void OnDisable()
        {
            GameManager.Instance.ReplaceManager.onAllPiecesReplaced -= EnableText;
        }


        private void EnableText(bool isEnabled)
        {
            _text.enabled = isEnabled;
        }
    }
}