using UnityEngine;

namespace Mediapipe.Unity
{
    public class HumanJointCalculator
    {
        public Transform initialObj;
        public Transform obj;
        public LandmarkList _landmarkList;

        public HumanJointCalculator (Transform t)
        {
            initialObj = t;
            obj = t;
        }
        
        public void Refresh (LandmarkList landmarkList) 
        {
            _landmarkList = landmarkList;
        }
        
        public virtual void Calc () {}
    }
};