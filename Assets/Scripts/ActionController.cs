using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
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
    
    
    //Lister spacebar click 
    public void OnSpacebarClick()
    {
        ToggleCamera();
    }
    
    //Toggle between the two cameras
    public void ToggleCamera()
    {
        if (_avatarCamera.enabled)
        {
            ShowUI();
        }
        else
        {
            ShowAvatar();
        }
    }
    
    
    public void ShowAvatar()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        
        _avatarCamera.enabled = true;
        _uICamera.enabled = false;
    }
    
    public void ShowUI()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        _avatarCamera.enabled = false;
        _uICamera.enabled = true;
    }
}
