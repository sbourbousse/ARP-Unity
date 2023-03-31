using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField] public Camera _avatarCamera2;
    [SerializeField] public Camera _avatarCamera;
    [SerializeField] public Camera _uICamera;

    // Start is called before the first frame update
    void Start()
    {
        ShowUI();
    }

    // Update is called once per frame
    void Update()
    {
        // If the spacebar is pressed, toggle the camera
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleCamera();
        }
    }
    
    //Toggle between the two cameras
    public void ToggleCamera()
    {
        if (_avatarCamera.enabled)
        {
            ShowAvatar2();
        }
        else if (_avatarCamera2.enabled)
        {
            ShowUI();
        }
        else if (_uICamera.enabled)
        {
            ShowAvatar();
        }
    }
    
    
    public void ShowAvatar()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Debug.Log("ShowAvatar");
        DisableAll();
        _avatarCamera.enabled = true;
    }

    public void ShowAvatar2()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Debug.Log("ShowAvatar2");
        DisableAll();
        _avatarCamera2.enabled = true;
    }
    
    public void ShowUI()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("ShowUI");
        DisableAll();
        _uICamera.enabled = true;
    }

    public void DisableAll() 
    {
        _avatarCamera.enabled = false;
        _avatarCamera2.enabled = false;
        _uICamera.enabled = false;
    }

}
