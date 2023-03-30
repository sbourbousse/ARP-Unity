using UnityEngine;

namespace Mediapipe.Unity
{
    public class HumanJointCalculator
    {
        public Transform obj;
        public NormalizedLandmarkList _landmarkList;

        public HumanJointCalculator (Transform t)
        {
            obj = t;
        }
        
        public void Refresh (NormalizedLandmarkList landmarkList) 
        {
            _landmarkList = landmarkList;
        }
        
        public virtual void Calc () {}
    }
};