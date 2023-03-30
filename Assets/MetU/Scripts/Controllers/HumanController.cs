using UnityEngine;

namespace Mediapipe.Unity
{
    public class HumanController 
    {
        HumanHandController leftHandController;
        HumanHandController rightHandController;

        public HumanController() {
            rightHandController = new HumanHandController(HumanPartSide.Right);
            leftHandController = new HumanHandController(HumanPartSide.Left);
        }
        public void UpdateHands() {
            rightHandController.OnUpdateLandmarks();
            leftHandController.OnUpdateLandmarks();
        }
    }
}