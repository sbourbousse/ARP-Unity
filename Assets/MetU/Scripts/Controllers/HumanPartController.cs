using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
    public class HumanPartController : IHumanPartController
    {
        protected NormalizedLandmarkList landmarkList;
        protected int updateCount;
        protected int checkedUpdateCount;
        protected List<HumanPartTrigger> triggerList;
        public HumanPartController()
        {
            landmarkList = new NormalizedLandmarkList();
            updateCount = 0;
            checkedUpdateCount = 0;
            triggerList = new List<HumanPartTrigger>();
        }
        public bool CheckLandmarkList(NormalizedLandmarkList landmarkList)
        {
            if (landmarkList == null)
                return false;
            else if (landmarkList.Landmark.Count <= 0)
                return false;
            return true;
        }

        public virtual void OnUpdateLandmarks() {
            throw new NotImplementedException();
        }

        public void UpdateLandmarks(NormalizedLandmarkList landmarkList)
        {   
            updateCount++;
            if(!CheckLandmarkList(landmarkList))
                return;
            
            checkedUpdateCount++;
            this.landmarkList = landmarkList;
            OnUpdateLandmarks();
        }

        public HumanPartTrigger GetTrigger(string triggerName)
        {
            return triggerList.Find(x => x.TriggerName == triggerName);
        }

        public void UpdateTriggers()
        {
            foreach(var trigger in triggerList)
            {
                trigger.Triggered = true;
            }
        }
        protected void SetTriggers(List<HumanPartTrigger> triggerList)
        {
            foreach(var trigger in triggerList)
            {
                AddTrigger(trigger.TriggerName);
            }
        }
        protected bool AddTrigger(string triggerName)
        {
            if(CheckTrigger(triggerName)) return false;
            triggerList.Add(new HumanPartTrigger(triggerName, 1000));
            return true;
        }
        public bool CheckTrigger(string triggerName)
        {
            return triggerList.Find(x => x.TriggerName == triggerName) != null;
        }

    }
}