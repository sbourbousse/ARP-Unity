using UnityEngine;

namespace Mediapipe.Unity
{
    public class LeftThumbCalculator: LeftLimbJointCalculator
    {   
        public LeftThumbCalculator (Transform t) : base(t) {}
        
        public override void Calc () 
        {
            if (_landmarkList == null) return;

            Refresh();

            obj.Rotate(
                Quaternion.FromToRotation(-obj.right, v_wrist_thumb).eulerAngles,
                Space.World
            );
        }
    }
};