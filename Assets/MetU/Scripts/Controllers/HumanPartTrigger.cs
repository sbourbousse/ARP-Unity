using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
    public class HumanPartTrigger
    {
        private DateTime? startTime;
        private DateTime? endTime;
        private bool emiting;
        public string TriggerName;
        public bool Triggered = false;
        public TimeSpan? Duration;
        private int triggerCount;
        private float minTriggerBysecond;
        public HumanPartTrigger(string triggerName, int duration, float minimalTriggerBysecond = 1)
        {
            TriggerName = triggerName;
            Duration = TimeSpan.FromMilliseconds(duration);
            startTime = null;
            endTime = null;
            emiting = false;
            triggerCount = 0;
            minTriggerBysecond = minimalTriggerBysecond;
        }
        
        public void Activate() {
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
            DateTime currentTime = DateTime.Now;
            Triggered = true;
            triggerCount++;
            if(startTime == null)
                StartClock();
            if(endTime == null && GetCurrentDuration() > Duration)
            {
                Debug.Log("ACTIVATE EMITTER");
                EndClock();
                ActivateEmitter();
                return;
            }
        }

        private TimeSpan? GetCurrentDuration() {
            var duration = DateTime.Now - startTime;
            if (duration < TimeSpan.Zero) 
                return null;
            return duration;
        }

        private void StartClock()
        {
            Debug.Log("New trigger " + TriggerName + " with duration " + Duration + "ms");
            startTime = DateTime.Now;
        }

        private void EndClock()
        {
            endTime = DateTime.Now;
        }


        private void ActivateEmitter() 
        {
            Deactivate();
            emiting = true;
        }

        public bool GetEmitter()
        {
            if(!emiting) return false;
            Reset();
            return true;
        }

        public void Reset() {
            emiting = false;
            startTime = null;
            endTime = null;
            triggerCount = 0;
            Triggered = false;
        }
    }
}