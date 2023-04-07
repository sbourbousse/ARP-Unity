using UnityEngine;
using System.Collections.Generic;
using System;

namespace Mediapipe.Unity
{
    public enum HumanPartSide {
        Left,
        Right
    }
    public class HumanHandController : HumanPartController
    {
        public HumanPartSide Side;

        public HumanHandController(HumanPart humanPart, HumanPartSide side) : base(humanPart)
        {
            Side = side;
            var triggers = new List<HumanPartTrigger>() {
                new HumanPartTrigger("close-hand", 1000)
            };
            SetTriggers(triggers);
        }

        public override void OnUpdateLandmarks() {
            if(CheckHandIsClosed()) {
                var closeHandEmitter = GetTrigger("close-hand");
				closeHandEmitter.Activate();
            }
        }
		
		public List<Tuple<HumanPart, HumanPartTrigger>> GetEmittingTriggers() {
			var emittingTriggers = new List<Tuple<HumanPart, HumanPartTrigger>>();
			foreach(var trigger in triggerList) {
				if(trigger.GetEmitter()) {
					emittingTriggers.Add(new Tuple<HumanPart, HumanPartTrigger>(HumanPart, trigger));
				}
			}
			return emittingTriggers;
		}


        public bool CheckHandIsClosed() {
            if (landmarkList == null || landmarkList.Landmark.Count == 0)
				{
					return false;
				}
				// Get the palm landmark
				var palmLandmark = landmarkList.Landmark[0];

				// Get the tip of the thumb
				var thumbTip = landmarkList.Landmark[4];

				// Get the tip of the index finger
				var indexTip = landmarkList.Landmark[8];

				// Get the tip of the middle finger
				var middleTip = landmarkList.Landmark[12];

				// Get the tip of the ring finger
				var ringTip = landmarkList.Landmark[16];

				// Get the tip of the pinky finger
				var pinkyTip = landmarkList.Landmark[20];

				var thumbPalm = landmarkList.Landmark[3];
				var indexPalm = landmarkList.Landmark[5];
				var middlePalm = landmarkList.Landmark[9];
				var ringPalm = landmarkList.Landmark[13];
				var pinkyPalm = landmarkList.Landmark[17];



				// Calculate the distance between the tip and the base of each finger
				var thumbTipDistance = Vector3.Distance(new Vector3(thumbTip.X, thumbTip.Y, thumbTip.Z), new Vector3(palmLandmark.X, palmLandmark.Y, palmLandmark.Z));
				var indexTipDistance = Vector3.Distance(new Vector3(indexTip.X, indexTip.Y, indexTip.Z), new Vector3(palmLandmark.X, palmLandmark.Y, palmLandmark.Z));
				var middleTipDistance = Vector3.Distance(new Vector3(middleTip.X, middleTip.Y, middleTip.Z), new Vector3(palmLandmark.X, palmLandmark.Y, palmLandmark.Z));
				var ringTipDistance = Vector3.Distance(new Vector3(ringTip.X, ringTip.Y, ringTip.Z), new Vector3(palmLandmark.X, palmLandmark.Y, palmLandmark.Z));
				var pinkyTipDistance = Vector3.Distance(new Vector3(pinkyTip.X, pinkyTip.Y, pinkyTip.Z), new Vector3(palmLandmark.X, palmLandmark.Y, palmLandmark.Z));

				var thumbPalmDistance = Vector3.Distance(new Vector3(thumbPalm.X, thumbPalm.Y, thumbPalm.Z), new Vector3(palmLandmark.X, palmLandmark.Y, palmLandmark.Z));
				var indexPalmDistance = Vector3.Distance(new Vector3(indexPalm.X, indexPalm.Y, indexPalm.Z), new Vector3(palmLandmark.X, palmLandmark.Y, palmLandmark.Z));
				var middlePalmDistance = Vector3.Distance(new Vector3(middlePalm.X, middlePalm.Y, middlePalm.Z), new Vector3(palmLandmark.X, palmLandmark.Y, palmLandmark.Z));
				var ringPalmDistance = Vector3.Distance(new Vector3(ringPalm.X, ringPalm.Y, ringPalm.Z), new Vector3(palmLandmark.X, palmLandmark.Y, palmLandmark.Z));
				var pinkyPalmDistance = Vector3.Distance(new Vector3(pinkyPalm.X, pinkyPalm.Y, pinkyPalm.Z), new Vector3(palmLandmark.X, palmLandmark.Y, palmLandmark.Z));

				// Is hand Open Or Closed
				var totalTipDistance = thumbTipDistance + indexTipDistance + middleTipDistance + ringTipDistance + pinkyTipDistance;
				totalTipDistance /= 5;
				var totalPalmDistance = thumbPalmDistance + indexPalmDistance + middlePalmDistance + ringPalmDistance + pinkyPalmDistance;
				totalPalmDistance /= 5;

				var openHand = totalTipDistance > totalPalmDistance;
                return !openHand;
        }
    }


}