// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine;

using mplt = Mediapipe.LocationData.Types;

namespace Mediapipe.Unity
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public class PointListAnnotation : ListAnnotation<PointAnnotation>
  {
    [SerializeField] private Color _color = Color.green;
    [SerializeField] private float _radius = 3.0f;

    private void OnValidate()
    {
      ApplyColor(_color);
      ApplyRadius(_radius);
    }

    public void SetColor(Color color)
    {
      _color = color;
      ApplyColor(_color);
    }

    public void SetRadius(float radius)
    {
      _radius = radius;
      ApplyRadius(_radius);
    }

    public void Draw(IList<Vector3> targets)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target); }
        });
      }
    }

    public void Draw(IList<Landmark> targets, Vector3 scale, bool visualizeZ = true)
    {
      // CategorizeLandmarkPoint();
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null)
          {
            annotation.Draw(target, scale, visualizeZ);
          }
        }); 
      }
      // test2();
    }

    public void CategorizeLandmarkPoint()
    {
      for(int i = 0; i < children.Count; i++)
      {
        children[i].SetPoseLandMarkModelPoint((PoseLandmarkListAnnotation.PoseLandMarkModelPoint)i); 
      }
    }

    // public void test2()
    // {
    //   foreach (var child in children)
    //   {
    //     var tag = PoseLandmarkListAnnotation.GetBodyTageByPoseLandMarkModelPoint(child.poseLandMarkModelPoint);
    //     if (tag == "") continue;
    //     var skeletonPart = GameObject.FindWithTag(tag);
    //     if (skeletonPart)
    //     {
    //       // skeletonPart.transform
    //       
    //       // skeletonPart.transform.localPosition = child.transform.localPosition;
    //       // annotation.transform.SetParent(skeletonPart.transform);
    //       skeletonPart.transform.SetParent(child.transform);
    //       Vector3 localPosition;
    //       Quaternion rotation;
    //       child.transform.GetLocalPositionAndRotation(out localPosition, out rotation);
    //       skeletonPart.transform.SetLocalPositionAndRotation(localPosition, rotation);
    //     }
    //   }
    // }

    public void Draw(LandmarkList targets, Vector3 scale, bool visualizeZ = true)
    {
      Draw(targets.Landmark, scale, visualizeZ);
    }

    public void Draw(IList<NormalizedLandmark> targets, bool visualizeZ = true)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, visualizeZ); }
        });
      }
    }

    public void Draw(NormalizedLandmarkList targets, bool visualizeZ = true)
    {
      Draw(targets.Landmark, visualizeZ);
    }

    public void Draw(IList<AnnotatedKeyPoint> targets, Vector2 focalLength, Vector2 principalPoint, float zScale, bool visualizeZ = true)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, focalLength, principalPoint, zScale, visualizeZ); }
        });
      }
    }

    public void Draw(IList<mplt.RelativeKeypoint> targets, float threshold = 0.0f)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, threshold); }
        });
      }
    }

    public new void Fill(int count)
    {
      int index = 0;
      while (children.Count < count)
      {
        var child = InstantiateChild(false);
        children.Add(child);
        
        var poseLandMarkModelPoint = (PoseLandmarkListAnnotation.PoseLandMarkModelPoint)index;
        var type = PoseLandmarkListAnnotation.GetBodyTagByPoseLandMarkModelPoint(
          (poseLandMarkModelPoint));
        child.SetPoseLandMarkModelPoint(poseLandMarkModelPoint);
      }
    }

    protected PointAnnotation InstantiateChild(bool isActive = true)
    {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetColor(_color);
      annotation.SetRadius(_radius);
      // annotation.gameObject.tag = tag;
      return annotation;
    }

    private void ApplyColor(Color color)
    {
      foreach (var point in children)
      {
        if (point != null) { point.SetColor(color); }
      }
    }

    private void ApplyRadius(float radius)
    {
      foreach (var point in children)
      {
        if (point != null) { point.SetRadius(radius); }
      }
    }

    private void SetTag(string tag)
    {
      gameObject.tag = tag;
    }
  }
}
