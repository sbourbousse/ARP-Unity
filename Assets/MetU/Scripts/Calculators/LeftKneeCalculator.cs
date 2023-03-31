using UnityEngine;

namespace Mediapipe.Unity
{
    public class LeftKneeCalculator: LeftLimbJointCalculator
    {   
        public LeftKneeCalculator (Transform t) : base(t) {}

        public override void Calc () 
        {
            if (_landmarkList == null) return;

            Refresh();

            obj.Rotate(
                Quaternion.FromToRotation(-obj.right, v_knee_ankle).eulerAngles,
                Space.World
            );

            var norm_x = Vector3.Cross(v_ankle_index, v_ankle_heel);
            obj.Rotate(
                Quaternion.FromToRotation(initialObj.forward, norm_x).eulerAngles,
                Space.World
            );
        }
    }
};