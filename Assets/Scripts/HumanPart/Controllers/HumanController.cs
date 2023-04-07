using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
    public enum HumanPart {
        LeftHand,
        RightHand
    }

    public class HumanController 
    {
        HumanHandController leftHandController;
        HumanHandController rightHandController;

        public HumanController() {
            rightHandController = new HumanHandController(HumanPart.RightHand, HumanPartSide.Right);
            leftHandController = new HumanHandController(HumanPart.LeftHand, HumanPartSide.Left);
        }
        public List<Tuple<HumanPart, HumanPartTrigger>> GetEmittingTriggers() {
            var triggers = new List<Tuple<HumanPart, HumanPartTrigger>>();
            triggers.AddRange(leftHandController.GetEmittingTriggers());
            triggers.AddRange(rightHandController.GetEmittingTriggers());
            return triggers;
        }

        public void UpdateHandsLandmarks(NormalizedLandmarkList landmarkList, HumanPartSide side) {
            if (side == HumanPartSide.Left) {
                leftHandController.UpdateLandmarks(landmarkList);
            } else {
                rightHandController.UpdateLandmarks(landmarkList);
            }

        }
    }
}