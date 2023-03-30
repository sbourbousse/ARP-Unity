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

		private int _animationNb;
		private double _avgUpdateByRefresh;
		private int _refreshCounter;
		private int _updateCounter;

		private GameObject _mainCamera;
		private GameObject _secondaryCamera;

		private void Start()
		{      
			_anim = GetComponent<Animator>();
			jointFactory = new HumanJointFactory(_anim);
			calculators = jointFactory.Generate();
			humanController = new HumanController();
			avatarActionController = GameObject.FindWithTag("Solution").GetComponent<AvatarActionController>();
			_animationNb = 1;
			_mainCamera = GameObject.FindWithTag("MainCamera");
			_secondaryCamera = GameObject.FindWithTag("SecondaryCamera");
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

			_updateCounter++;
			UpdateRefreshRate();
			UpdateAnimation();
		}

		public void Refresh(LandmarkList target, bool reverseCoordinates = false)
		{	
			if(target == null || target.Landmark == null)
				return;

			if (reverseCoordinates)
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

			_refreshCounter++;

		}
		public void RefreshHand(NormalizedLandmarkList landmarkList, bool rightHand = true) 
		{
			HumanPartSide side = rightHand ? HumanPartSide.Right : HumanPartSide.Left;
			humanController.UpdateHandsLandmarks(landmarkList, side);
		}

		public void UpdateRefreshRate() {
			if(_updateCounter % 250 == 0) 
			{
				int avg = _refreshCounter <= 0 ? 1 : _refreshCounter;
				_avgUpdateByRefresh = _updateCounter / avg;
			} 
			if (_updateCounter % 1000 == 0) 
			{
				_refreshCounter = 0;
				_updateCounter = 0;
			}
		}

		public void UpdateAnimation() {
			if(_updateCounter % 250 != 0) return;

			if(_avgUpdateByRefresh <= 200) 
			{
				ToggleAnimations(false);
				return;
			}

			if(_animationNb == 1) 
			{
				ToggleAnimations(true);
				return;
			}

			if(_updateCounter % 1000 == 0)
			{
				ToggleAnimations(true);
			} 
		}

		public void ToggleAnimations (bool toggle) {
			_anim.enabled = toggle;
			if(toggle) 
			{
				_secondaryCamera.SetActive(true);
				if(_animationNb % 2 == 0)
					_anim.SetBool("Back", true);
				else 	
					_anim.SetBool("Next", true);

				_animationNb++;
			} else {
				_secondaryCamera.SetActive(false);
				_animationNb = 1;
			}
		}
    }
} 	