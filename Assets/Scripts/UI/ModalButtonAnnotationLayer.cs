// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity.UI
{
  public class ModalButtonAnnotationLayer : ModalButton
  {
    [SerializeField] private GameObject _annotationLayer;

    // Toggle
    public void ToggleScreen() {
      // Set transparent texture
      _annotationLayer.SetActive(!_annotationLayer.activeSelf);
    }
  }
}
