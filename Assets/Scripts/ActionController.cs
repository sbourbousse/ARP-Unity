using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField] public GameObject _mainCameraContainer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // Move camera down
            _mainCameraContainer.transform.position = new Vector3(_mainCameraContainer.transform.position.x, _mainCameraContainer.transform.position.y - 0.1f, _mainCameraContainer.transform.position.z);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // Move camera up
            _mainCameraContainer.transform.position = new Vector3(_mainCameraContainer.transform.position.x, _mainCameraContainer.transform.position.y + 0.1f, _mainCameraContainer.transform.position.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Move camera left
            _mainCameraContainer.transform.position = new Vector3(_mainCameraContainer.transform.position.x - 0.1f, _mainCameraContainer.transform.position.y, _mainCameraContainer.transform.position.z);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // Move camera right
            _mainCameraContainer.transform.position = new Vector3(_mainCameraContainer.transform.position.x + 0.1f, _mainCameraContainer.transform.position.y, _mainCameraContainer.transform.position.z);
        }

        //Handle pressing key

    }
    
   

}
