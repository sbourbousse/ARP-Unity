// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity.UI
{
  public class ModalButtonCustom : ModalButton
  {
    [SerializeField] private GameObject _annotableScreen;

    private Screen annotableScreenComponent => _annotableScreen.GetComponent<Screen>();

    public void HideScreen() {
      // Set transparent texture
      annotableScreenComponent.ToggleImageSource();
    }
  }
}
