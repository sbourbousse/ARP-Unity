using UnityEngine;


namespace Mediapipe.Unity 
{
    public interface IHumanPartController
    {
        bool CheckLandmarkList(NormalizedLandmarkList landmarkList);
    }
}
