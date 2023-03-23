using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCubeSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Spawn cube every 2 seconds
        InvokeRepeating("SpawnCube", 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnCube()
    {
        Debug.Log("Spawned a cube");

        //Create a new cube
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Set plane as parent of cube to HitCubeSpawner
        cube.transform.parent = transform.parent;
        //Set the position of the cube to the position of the spawner
        float randomX = Random.Range(-1.5f, 1.5f);
        float randomY = Random.Range(0.25f, 1.5f);
        cube.transform.position = new Vector3(randomX, randomY, transform.position.z);
        cube.transform.rotation = Quaternion.Euler(0, 0, 0);

        //Add a rigidbody to the cube
        cube.AddComponent<Rigidbody>();
        //Add a script to the cube
        cube.AddComponent<HitCube>();

        // No gravity
        cube.GetComponent<Rigidbody>().useGravity = false;
        // Freeze rotation
        cube.GetComponent<Rigidbody>().freezeRotation = true;
        // Freeze position y
        cube.GetComponent<Rigidbody>().constraints =
            RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        // Add force to cube
        cube.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -200));

        return cube;
    }
}
