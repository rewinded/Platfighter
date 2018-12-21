using System;
using System.Collections;
using MANAGERS;
using NETWORKING;
using UnityEngine;
using UnityEngine.Serialization;
using Types = DATA.Types;

namespace PLAYER
{
    public class InputSender : MonoBehaviour
    {
        public bool[] Inputs;

        protected bool[] PrevInputs;
        protected int[] InputFramesHeld;
        
        protected PlayerData PlayerData { get; set; }
        
        protected virtual void Awake()
        {
            Inputs = new bool[Enum.GetNames(typeof(Types.Input)).Length];
            PrevInputs = new bool[Inputs.Length];
            InputFramesHeld = new int[Inputs.Length];
            PlayerData = GetComponent<PlayerData>();
        }
        
        private void Update()
        {
            if (GameManager.Instance.MatchType == Types.MatchType.OnlineMultiplayer && !P2PHandler.Instance.LatencyCalculated)
                return;
            
            Inputs.CopyTo(PrevInputs, 0);
            
            InputUpdate();
            
            for (var i = 0; i < Inputs.Length; i++)
            {
                // if input is held from previous frame increase frames held counter
                if (Inputs[i] == PrevInputs[i])
                {
                    InputFramesHeld[i]++;
                }
                else if (Inputs[i] != PrevInputs[i])
                {
                    // if input change is a release
                    if (!Inputs[i])
                    {
                        ReleaseEvent(i);
                        InputFramesHeld[i] = 0;
                    }
                    else
                    {
                        PressEvent(i);  
                    }
                }
            }
            
            InputLateUpdate();
        }

        // called after PrevInputs reset, before InputFramesHeld increased
        protected virtual void InputUpdate() { }
        protected virtual void InputLateUpdate() { }
        protected virtual void ReleaseEvent(int index) { }
        protected virtual void PressEvent(int index) { }


    }
}