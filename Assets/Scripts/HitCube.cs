using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0.5f, 0.25f, 0.005f);
        
        // Set color of cube red or blue
        if (Random.Range(0, 2) == 0)
            GetComponent<Renderer>().material.color = Color.blue;
        else
            GetComponent<Renderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // If cube collides with plane
        if (collision.gameObject.tag == "character")
        {
            // Destroy cube
            Destroy(gameObject);
        }
        else
        {
            // Destroy cube
            Destroy(gameObject);
        }
    }
}
