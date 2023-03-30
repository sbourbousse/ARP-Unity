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
		private HumanController humanController;

		private AvatarActionController avatarActionController;
		LeftWristCalculator leftWristCalculator;
		RightWristCalculator rightWristCalculator;
		private NormalizedLandmarkList prevHandLandmarks;
		private HumanJointFactory jointFactory;
		private HashSet<HumanJointCalculator> calculators;
		private Animator _anim;

		private void Start()
		{      
			_anim = GetComponent<Animator>();

			jointFactory = new HumanJointFactory(_anim);
			calculators = jointFactory.Generate();
			leftWristCalculator = calculators.OfType<LeftWristCalculator>().First();
			rightWristCalculator = calculators.OfType<RightWristCalculator>().First();
			humanController = new HumanController();
			avatarActionController = new AvatarActionController();
		}

		private void Update()
		{
			foreach (var calculator in calculators)
			{
				calculator.Calc();
			}
			var triggers = humanController.GetEmittingTriggers();
			foreach (var trigger in triggers)
			{
				if(trigger.Item2.TriggerName == "close-hand" && trigger.Item1 == HumanPart.LeftHand)
					avatarActionController.TriggerSword(HumanPartSide.Left);
				if(trigger.Item2.TriggerName == "close-hand" && trigger.Item1 == HumanPart.RightHand)
					avatarActionController.TriggerSword(HumanPartSide.Right);
					
			}
		}

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
			HumanPartSide side = rightHand ? HumanPartSide.Right : HumanPartSide.Left;
			humanController.UpdateHandsLandmarks(landmarkList, side);
		}
    }

	public class AvatarActionController {
		const int ColDown = 1000;
		public AvatarActionController()
		{
			IsSwordActive = true;
			RightSword = GameObject.FindWithTag("rightsword");
			LeftSword = GameObject.FindWithTag("leftsword");

		}
		public bool IsSwordActive { get; set; }
		public GameObject RightSword { get; set; }
		public GameObject LeftSword { get; set; }
		public string GetTagBySide(HumanPartSide side)
		{
			switch (side)
			{
				case HumanPartSide.Left:
					return "leftsword";
				case HumanPartSide.Right:
					return "rightsword";
				default:
					throw new ArgumentOutOfRangeException(nameof(side), side, null);

			}
		}
		public void TriggerSword(HumanPartSide side)
		{
			string gameTag = GetTagBySide(side);
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
		}

		public void ShowSword()
		{
			LeftSword.transform.localScale = new Vector3(3f, 3f, 3f);
			LeftSword.GetComponent<BoxCollider>().enabled = true;
			IsSwordActive = true;
		}

		public void HideSword()
		{
			LeftSword.transform.localScale = new Vector3(0f, 0f, 0f);
			LeftSword.GetComponent<BoxCollider>().enabled = false;
			IsSwordActive = false;
		}
	}
}