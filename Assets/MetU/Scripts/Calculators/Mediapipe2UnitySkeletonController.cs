using Mediapipe;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Mediapipe.Unity
{
  public class Mediapipe2UnitySkeletonController : MonoBehaviour
  {
    private HumanJointFactory jointFactory;
    private HashSet<HumanJointCalculator> calculators;
    
    private Animator _anim;

    private void Start()
    {      
      _anim = GetComponent<Animator>();

      jointFactory = new HumanJointFactory(_anim);
      calculators = jointFactory.Generate();
    }

    private void Update()
    {
      foreach (var calculator in calculators)
      {
        calculator.Calc();
      }

      jointFactory.ComputeOrientation();
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
  }
}