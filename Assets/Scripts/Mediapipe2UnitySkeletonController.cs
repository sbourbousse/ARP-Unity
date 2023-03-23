using Mediapipe;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Mediapipe.Unity
{

	public class Mediapipe2UnitySkeletonController : MonoBehaviour
	{
		private HumanController humanController;
		private AvatarActionController avatarActionController;
		private NormalizedLandmarkList prevHandLandmarks;
		private HumanJointFactory jointFactory;
		private HashSet<HumanJointCalculator> calculators;
		private Animator _anim;

		private void Start()
		{      
			_anim = GetComponent<Animator>();

			jointFactory = new HumanJointFactory(_anim);
			calculators = jointFactory.Generate();
			humanController = new HumanController();
			avatarActionController = GameObject.FindWithTag("Solution").GetComponent<AvatarActionController>();
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
					avatarActionController.PutNextShirtOn();
				if(trigger.Item2.TriggerName == "close-hand" && trigger.Item1 == HumanPart.RightHand)
					avatarActionController.PutPreviousShirtOn();
					
			}
		}

		public void Refresh(LandmarkList target, bool reverseCoordinates = false)
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
} 	