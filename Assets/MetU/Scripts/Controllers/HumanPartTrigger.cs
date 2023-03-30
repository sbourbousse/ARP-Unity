using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
    public class HumanPartTrigger
    {
        private int startTime;
        private int endTime;
        private bool emiting;
        public string TriggerName;
        public bool Triggered = false;
        public int Duration;
        private int triggerCount;
        private float minTriggerBysecond;
        public HumanPartTrigger(string triggerName, int duration, float minimalTriggerBysecond = 1)
        {
            TriggerName = triggerName;
            Duration = duration;
            startTime = 0;
            endTime = 0;
            emiting = false;
            triggerCount = 0;
            minTriggerBysecond = minimalTriggerBysecond;
        }
        
        public void Activate() {
            if(Triggered) {
                Debug.Log("Trigger " + TriggerName + " is already active");
                return;
            }
            Trigger();
        }

        public void Deactivate() {
            if(!Triggered) {
                Debug.Log("Trigger " + TriggerName + " is already inactive");
                return;
            }
            Triggered = false;
        }

        public void Trigger()
        {
            Triggered = true;
            triggerCount++;
            int currentTime = DateTime.Now.Millisecond;
            if(startTime == 0)
                startTime = currentTime;

            if(currentTime - startTime > Duration)
            {
                Triggered = false;
                endTime = currentTime;
            }
        }

        private void ActivateEmitter() 
        {
            emiting = true;
        }

        public bool GetEmitter()
        {
            if(!emiting) return false;
            emiting = false;
            return true;
        }
    }
}