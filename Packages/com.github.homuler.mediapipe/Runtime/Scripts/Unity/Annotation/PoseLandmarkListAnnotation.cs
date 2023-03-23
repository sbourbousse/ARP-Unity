// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public sealed class PoseLandmarkListAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private PointListAnnotation _landmarkListAnnotation;
    [SerializeField] private ConnectionListAnnotation _connectionListAnnotation;
    [SerializeField] private Color _leftLandmarkColor = Color.white;
    [SerializeField] private Color _rightLandmarkColor = Color.black;

    [Flags]
    public enum BodyParts : short
    {
      None = 0,
      Face = 1,
      Torso = 2,
      LeftArm = 4,
      LeftHand = 8,
      RightArm = 16,
      RightHand = 32,
      LowerBody = 64,
      All = 127,
    }
  
    public enum PoseLandMarkModelPoint
    {
      NOSE = 0,
      LEFT_EYE_INNER = 1,
      LEFT_EYE = 2,
      LEFT_EYE_OUTER = 3,
      RIGHT_EYE_INNER = 4,
      RIGHT_EYE = 5,
      RIGHT_EYE_OUTER = 6,
      LEFT_EAR = 7,
      RIGHT_EAR = 8,
      MOUTH_LEFT = 9,
      MOUTH_RIGHT = 10,
      LEFT_SHOULDER = 11,
      RIGHT_SHOULDER = 12,
      LEFT_ELBOW = 13,
      RIGHT_ELBOW = 14,
      LEFT_WRIST = 15,
      RIGHT_WRIST = 16,
      LEFT_PINKY = 17,
      RIGHT_PINKY = 18,
      LEFT_INDEX = 19,
      RIGHT_INDEX = 20,
      LEFT_THUMB = 21,
      RIGHT_THUMB = 22,
      LEFT_HIP = 23,
      RIGHT_HIP = 24,
      LEFT_KNEE = 25,
      RIGHT_KNEE = 26,
      LEFT_ANKLE = 27,
      RIGHT_ANKLE = 28,
      LEFT_HEEL = 29,
      RIGHT_HEEL = 30,
      LEFT_FOOT_INDEX = 31,
      RIGHT_FOOT_INDEX = 32
    }

    private const int _LandmarkCount = 33;

    private static readonly PoseLandMarkModelPoint[] _LeftLandmarks = new PoseLandMarkModelPoint[] {
      PoseLandMarkModelPoint.LEFT_EYE_INNER,
      PoseLandMarkModelPoint.LEFT_EYE,
      PoseLandMarkModelPoint.LEFT_EYE_OUTER,
      PoseLandMarkModelPoint.LEFT_EAR,
      PoseLandMarkModelPoint.LEFT_SHOULDER,
      PoseLandMarkModelPoint.LEFT_ELBOW,
      PoseLandMarkModelPoint.LEFT_WRIST,
      PoseLandMarkModelPoint.LEFT_PINKY,
      PoseLandMarkModelPoint.LEFT_INDEX,
      PoseLandMarkModelPoint.LEFT_THUMB,
      PoseLandMarkModelPoint.LEFT_HIP,
      PoseLandMarkModelPoint.LEFT_KNEE,
      PoseLandMarkModelPoint.LEFT_ANKLE,
      PoseLandMarkModelPoint.LEFT_HEEL,
      PoseLandMarkModelPoint.LEFT_FOOT_INDEX
    };
    private static readonly PoseLandMarkModelPoint[] _RightLandmarks = new PoseLandMarkModelPoint[] {
      PoseLandMarkModelPoint.RIGHT_EYE_INNER,
      PoseLandMarkModelPoint.RIGHT_EYE,
      PoseLandMarkModelPoint.RIGHT_EYE_OUTER,
      PoseLandMarkModelPoint.RIGHT_EAR,
      PoseLandMarkModelPoint.RIGHT_SHOULDER,
      PoseLandMarkModelPoint.RIGHT_ELBOW,
      PoseLandMarkModelPoint.RIGHT_WRIST,
      PoseLandMarkModelPoint.RIGHT_PINKY,
      PoseLandMarkModelPoint.RIGHT_INDEX,
      PoseLandMarkModelPoint.RIGHT_THUMB,
      PoseLandMarkModelPoint.RIGHT_HIP,
      PoseLandMarkModelPoint.RIGHT_KNEE,
      PoseLandMarkModelPoint.RIGHT_ANKLE,
      PoseLandMarkModelPoint.RIGHT_HEEL,
      PoseLandMarkModelPoint.RIGHT_FOOT_INDEX
    };
    private static readonly List<(PoseLandMarkModelPoint, PoseLandMarkModelPoint)> _Connections = new List<(PoseLandMarkModelPoint, PoseLandMarkModelPoint)> {
      // Left Eye
      ((PoseLandMarkModelPoint)0, (PoseLandMarkModelPoint)1),
      ((PoseLandMarkModelPoint)1, (PoseLandMarkModelPoint)2),
      ((PoseLandMarkModelPoint)2, (PoseLandMarkModelPoint)3),
      ((PoseLandMarkModelPoint)3, (PoseLandMarkModelPoint)7),
      // Right Eye
      ((PoseLandMarkModelPoint)0, (PoseLandMarkModelPoint)4),
      ((PoseLandMarkModelPoint)4, (PoseLandMarkModelPoint)5),
      ((PoseLandMarkModelPoint)5, (PoseLandMarkModelPoint)6),
      ((PoseLandMarkModelPoint)6, (PoseLandMarkModelPoint)8),
      // Lips
      ((PoseLandMarkModelPoint)9, (PoseLandMarkModelPoint)10),
      ((PoseLandMarkModelPoint)11, (PoseLandMarkModelPoint)13),
      ((PoseLandMarkModelPoint)13, (PoseLandMarkModelPoint)15),
      // Left Hand
      ((PoseLandMarkModelPoint)15, (PoseLandMarkModelPoint)17),
      ((PoseLandMarkModelPoint)15, (PoseLandMarkModelPoint)19),
      ((PoseLandMarkModelPoint)15, (PoseLandMarkModelPoint)21),
      ((PoseLandMarkModelPoint)17, (PoseLandMarkModelPoint)19),
      ((PoseLandMarkModelPoint)12, (PoseLandMarkModelPoint)14),
      ((PoseLandMarkModelPoint)14, (PoseLandMarkModelPoint)16),
      // Right Hand
      ((PoseLandMarkModelPoint)16, (PoseLandMarkModelPoint)18),
      ((PoseLandMarkModelPoint)16, (PoseLandMarkModelPoint)20),
      ((PoseLandMarkModelPoint)16, (PoseLandMarkModelPoint)22),
      ((PoseLandMarkModelPoint)18, (PoseLandMarkModelPoint)20),
      // Torso
      ((PoseLandMarkModelPoint)11, (PoseLandMarkModelPoint)12),
      ((PoseLandMarkModelPoint)12, (PoseLandMarkModelPoint)24),
      ((PoseLandMarkModelPoint)24, (PoseLandMarkModelPoint)23),
      ((PoseLandMarkModelPoint)23, (PoseLandMarkModelPoint)11),
      // Left Leg
      ((PoseLandMarkModelPoint)23, (PoseLandMarkModelPoint)25),
      ((PoseLandMarkModelPoint)25, (PoseLandMarkModelPoint)27),
      ((PoseLandMarkModelPoint)27, (PoseLandMarkModelPoint)29),
      ((PoseLandMarkModelPoint)29, (PoseLandMarkModelPoint)31),
      // Right Leg
      ((PoseLandMarkModelPoint)24, (PoseLandMarkModelPoint)26),
      ((PoseLandMarkModelPoint)26, (PoseLandMarkModelPoint)28),
      ((PoseLandMarkModelPoint)28, (PoseLandMarkModelPoint)30),
      ((PoseLandMarkModelPoint)30, (PoseLandMarkModelPoint)32)
    };

    public static string GetBodyTagByPoseLandMarkModelPoint(PoseLandMarkModelPoint poseLandMarkModelPoint)
    {
      switch (poseLandMarkModelPoint)
      {
        case PoseLandMarkModelPoint.NOSE:
          return "pose-mouth";
        case PoseLandMarkModelPoint.LEFT_EYE:
          return "pose-left-eye";
        case PoseLandMarkModelPoint.RIGHT_EYE:
          return "pose-right-eye";
        case PoseLandMarkModelPoint.LEFT_SHOULDER:
          return "pose-left-shoulder";
        case PoseLandMarkModelPoint.RIGHT_SHOULDER:
          return "pose-right-shoulder";
        case PoseLandMarkModelPoint.LEFT_ELBOW :
          return "pose-left-elbow";
        case PoseLandMarkModelPoint.RIGHT_ELBOW:
          return "pose-right-elbow";
        case PoseLandMarkModelPoint.LEFT_WRIST:
          return "pose-left-wrist";
        case PoseLandMarkModelPoint.RIGHT_WRIST:
          return "pose-right-wrist";
      }

      return "";
    }


    public override bool isMirrored
    {
      set
      {
        _landmarkListAnnotation.isMirrored = value;
        _connectionListAnnotation.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle
    {
      set
      {
        _landmarkListAnnotation.rotationAngle = value;
        _connectionListAnnotation.rotationAngle = value;
        base.rotationAngle = value;
      }
    }

    public PointAnnotation this[int index] => _landmarkListAnnotation[index];

    private void Start()
    {
      _landmarkListAnnotation.Fill(_LandmarkCount);
      ApplyLeftLandmarkColor(_leftLandmarkColor);
      ApplyRightLandmarkColor(_rightLandmarkColor);
      AssociateLandmarkToPoseLandMarkModelPoint();

      _connectionListAnnotation.Fill(_Connections, _landmarkListAnnotation);
    }
    
    private void AssociateLandmarkToPoseLandMarkModelPoint()
    {
      for (int i = 0; i < _LandmarkCount; i++)
      {
        var tag = GetBodyTagByPoseLandMarkModelPoint((PoseLandMarkModelPoint)i);
        if (tag == "") continue;
        _landmarkListAnnotation[i].tag = tag;
      }
    }

    private void OnValidate()
    {
      ApplyLeftLandmarkColor(_leftLandmarkColor);
      ApplyRightLandmarkColor(_rightLandmarkColor);
    }

    public void SetLeftLandmarkColor(Color leftLandmarkColor)
    {
      _leftLandmarkColor = leftLandmarkColor;
      ApplyLeftLandmarkColor(_leftLandmarkColor);
    }

    public void SetRightLandmarkColor(Color rightLandmarkColor)
    {
      _rightLandmarkColor = rightLandmarkColor;
      ApplyRightLandmarkColor(_rightLandmarkColor);
    }

    public void SetLandmarkRadius(float landmarkRadius)
    {
      _landmarkListAnnotation.SetRadius(landmarkRadius);
    }

    public void SetConnectionColor(Color connectionColor)
    {
      _connectionListAnnotation.SetColor(connectionColor);
    }

    public void SetConnectionWidth(float connectionWidth)
    {
      _connectionListAnnotation.SetLineWidth(connectionWidth);
    }

    public void Draw(IList<Landmark> target, Vector3 scale, bool visualizeZ = false)
    {
      if (ActivateFor(target))
      {
        
        _landmarkListAnnotation.Draw(target, scale, visualizeZ);
        // Draw explicitly because connection annotation's targets remain the same.
        _connectionListAnnotation.Redraw();
      }
    }

    public void Draw(LandmarkList target, Vector3 scale, bool visualizeZ = false)
    {
      Draw(target?.Landmark, scale, visualizeZ);
    }

    public void Draw(IList<NormalizedLandmark> target, BodyParts mask, bool visualizeZ = false)
    {
      if (ActivateFor(target))
      {
        _landmarkListAnnotation.Draw(target, visualizeZ);
        ApplyMask(mask);
        // Draw explicitly because connection annotation's targets remain the same.
        _connectionListAnnotation.Redraw();
      }
    }

    public void Draw(NormalizedLandmarkList target, BodyParts mask, bool visualizeZ = false)
    {
      Draw(target?.Landmark, mask, visualizeZ);
    }

    public void Draw(IList<NormalizedLandmark> target, bool visualizeZ = false)
    {
      Draw(target, BodyParts.All, visualizeZ);
    }

    public void Draw(NormalizedLandmarkList target, bool visualizeZ = false)
    {
      Draw(target?.Landmark, BodyParts.All, visualizeZ);
    }

    private void ApplyLeftLandmarkColor(Color color)
    {
      var annotationCount = _landmarkListAnnotation == null ? 0 : _landmarkListAnnotation.count;
      if (annotationCount >= _LandmarkCount)
      {
        foreach (var index in _LeftLandmarks)
        {
          _landmarkListAnnotation[(int)index].SetColor(color);
        }
      }
    }

    private void ApplyRightLandmarkColor(Color color)
    {
      var annotationCount = _landmarkListAnnotation == null ? 0 : _landmarkListAnnotation.count;
      if (annotationCount >= _LandmarkCount)
      {
        foreach (var index in _RightLandmarks)
        {
          _landmarkListAnnotation[(int)index].SetColor(color);
        }
      }
    }

    private void ApplyMask(BodyParts mask)
    {
      // _landmarkListAnnotation[13].SetTag("leftArm");
      // _landmarkListAnnotation[14].SetTag("rightArm");
      if (mask == BodyParts.All)
      {
        return;
      }

      if (!mask.HasFlag(BodyParts.Face))
      {
        // deactivate face landmarks
        for (var i = 0; i <= 10; i++)
        {
          _landmarkListAnnotation[i].SetActive(false);
        }
      }
      if (!mask.HasFlag(BodyParts.LeftArm))
      {
        // deactivate left elbow to hide left arm
        _landmarkListAnnotation[13].SetActive(false);
      }
      if (!mask.HasFlag(BodyParts.LeftHand))
      {
        // deactive left wrist, thumb, index and pinky to hide left hand
        _landmarkListAnnotation[15].SetActive(false);
        _landmarkListAnnotation[17].SetActive(false);
        _landmarkListAnnotation[19].SetActive(false);
        _landmarkListAnnotation[21].SetActive(false);
      }
      if (!mask.HasFlag(BodyParts.RightArm))
      {
        // deactivate right elbow to hide right arm
        _landmarkListAnnotation[14].SetActive(false);
      }
      if (!mask.HasFlag(BodyParts.RightHand))
      {
        // deactivate right wrist, thumb, index and pinky to hide right hand
        _landmarkListAnnotation[16].SetActive(false);
        _landmarkListAnnotation[18].SetActive(false);
        _landmarkListAnnotation[20].SetActive(false);
        _landmarkListAnnotation[22].SetActive(false);
      }
      if (!mask.HasFlag(BodyParts.LowerBody))
      {
        // deactivate lower body landmarks
        for (var i = 25; i <= 32; i++)
        {
          _landmarkListAnnotation[i].SetActive(false);
        }
      }
    }
  }
}
