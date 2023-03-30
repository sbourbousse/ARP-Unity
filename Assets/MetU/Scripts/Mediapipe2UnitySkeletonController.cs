using Mediapipe;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
// utc now
using System.Diagnostics;

namespace Mediapipe.Unity
{

	public class Mediapipe2UnitySkeletonController : MonoBehaviour
	{


		private HandController rightHandController;
		private HandController leftHandController;


		private HumanController humanController;

		private bool rightHandOpen = false;
		private bool leftHandOpen = false;
		private bool rightHandTriggered = false;
		private bool leftHandTriggered = false;
		private bool rightSwordActive = false;
		private bool leftSwordActive = false;

		private NormalizedLandmarkList prevHandLandmarks;
		private HumanJointFactory jointFactory;
		private HashSet<HumanJointCalculator> calculators;

		LeftWristCalculator leftWristCalculator;
		RightWristCalculator rightWristCalculator;
		private Animator _anim;

		private void Start()
		{      
			_anim = GetComponent<Animator>();

			jointFactory = new HumanJointFactory(_anim);
			calculators = jointFactory.Generate();
			leftWristCalculator = calculators.OfType<LeftWristCalculator>().First();
			rightWristCalculator = calculators.OfType<RightWristCalculator>().First();
			rightHandController = new HandController(Side.Right);
			leftHandController = new HandController(Side.Left);
		}

		private void Update()
		{
			foreach (var calculator in calculators)
			{
				calculator.Calc();
				if(calculator.GetType() == typeof(LeftWristCalculator))
				{
					leftWristCalculator = calculator as LeftWristCalculator;
				} 
				else if(calculator.GetType() == typeof(RightWristCalculator))
				{
					rightWristCalculator = calculator as RightWristCalculator;
				}
			}
			TriggerSwords();
		}

		public void TriggerSwords()
		{
			if (leftHandController.IsTriggered) 
			{
				leftHandController.TriggerSword();
			}
			if (rightHandController.IsTriggered) 
			{
				rightHandController.TriggerSword();
			}
		}
		// public void OnHandOpenCloseChange() 
		// {
		// 	if (rightHandTriggered)
		// 	{
		// 		if(rightSwordActive) 
		// 		{
					
		// 			GameObject.FindWithTag("rightsword").transform.localScale = new Vector3(0f, 0f, 0f);
		// 			GameObject.FindWithTag("rightsword").GetComponent<BoxCollider>().enabled = false;
		// 		}
		// 		else 
		// 		{
		// 			// Hide sword mesh renderer and box collider
		// 			GameObject.FindWithTag("rightsword").transform.localScale = new Vector3(3f, 3f, 3f);
		// 			GameObject.FindWithTag("rightsword").GetComponent<BoxCollider>().enabled = true;
		// 		}
		// 		rightSwordActive = !rightSwordActive;
		// 		rightHandTriggered = false;
		// 	} 
			
		// 	if (leftHandTriggered)
		// 	{
				
		// 		if(leftSwordActive) 
		// 		{
		// 			GameObject.FindWithTag("leftsword").transform.localScale = new Vector3(0f, 0f, 0f);
		// 			GameObject.FindWithTag("leftsword").GetComponent<BoxCollider>().enabled = false;
		// 		}
		// 		else 
		// 		{
		// 			// Hide sword mesh renderer and box collider
		// 			GameObject.FindWithTag("leftsword").transform.localScale = new Vector3(3f, 3f, 3f);
		// 			GameObject.FindWithTag("leftsword").GetComponent<BoxCollider>().enabled = true;
		// 		}
		// 		leftSwordActive = !leftSwordActive;
		// 		leftHandTriggered = false;
		// 	}
		// }

		public void Refresh(NormalizedLandmarkList target, bool reverseCoordinates = false)
		{

			if (reverseCoordinates && target != null && target.Landmark != null)
			{
				for (int i = 0; i < target.Landmark.Count; i++)
				{
				target.Landmark[i].X = target.Landmark[i].X * -1;
				target.Landmark[i].Y = target.Landmark[i].Y * -1;
				target.Landmark[i].Z = target.Landmark[i].Z * -1;
				}
			}

			foreach (var calculator in calculators)
			{
				calculator.Refresh(target);
			}

		}
		public void RefreshHand(NormalizedLandmarkList landmarkList, bool rightHand = true) 
		{
			leftHandController.Refresh(landmarkList);
			rightHandController.Refresh(landmarkList);

			// if (rightHand)
			// {
			// 	rightHandController.Refresh(landmarkList);
			// }
			// else
			// {
			// 	leftHandController.Refresh(landmarkList);
			// }
		}

		private enum Side 
		{
			Left,
			Right
		}

        private class HandController
        {
			const int ColDown = 1000;
			public HandController(Side side)
			{
				Side = side;
				Sword = GameObject.FindWithTag(side == Side.Left ? "leftsword" : "rightsword");
				IsSwordActive = true;
				TimeFromTrigger = new Stopwatch();
			}
			public Side Side { get; }
			public bool IsOpen { get; set; }
			public bool IsTriggered { get; set; }
			Stopwatch TimeFromTrigger {get;set;}
			public bool IsSwordActive { get; set; }
			public GameObject Sword { get; set; }
			public void Refresh(NormalizedLandmarkList landmarkList)
			{
				if(TimeFromTrigger.IsRunning && TimeFromTrigger.ElapsedMilliseconds < ColDown) return;
				if (landmarkList == null || landmarkList.Landmark.Count == 0)
				{
					return;
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
				SetTriggered(openHand);
			}
			private void SetTriggered(bool openHand)
			{
				if(TimeFromTrigger.IsRunning == false) TimeFromTrigger.Start();
				if (openHand != IsOpen)
				{
					IsTriggered = true;
					IsOpen = openHand;
					TimeFromTrigger.Stop();
					TimeFromTrigger.Reset();
				}
			}
			public string GetTagBySide(Side side)
			{
				switch (side)
				{
					case Side.Left:
						return "leftsword";
					case Side.Right:
						return "rightsword";
					default:
						throw new ArgumentOutOfRangeException(nameof(side), side, null);

				}
			}
			public void TriggerSword()
			{
				string gameTag = GetTagBySide(Side);
				if (IsSwordActive)
				{
					// Hide sword mesh renderer and box collider
					HideSword();
				}
				else
				{
					// Show sword mesh renderer and box collider
					ShowSword();
				}
				IsSwordActive = !IsSwordActive;
				IsTriggered = false;
				UnityEngine.Debug.Log("Triggered Sword");
			}

			public void ShowSword()
			{
				Sword.transform.localScale = new Vector3(3f, 3f, 3f);
				Sword.GetComponent<BoxCollider>().enabled = true;
			}

			public void HideSword()
			{
				Sword.transform.localScale = new Vector3(0f, 0f, 0f);
				Sword.GetComponent<BoxCollider>().enabled = false;
			}
		}

		
    }
}