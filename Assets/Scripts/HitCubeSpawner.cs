using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCubeSpawner : MonoBehaviour
{
    private List<GameObject> hitCubes;

    // Start is called before the first frame update
    void Start()
    {
        hitCubes = new List<GameObject>();
        // Spawn cube every 2 seconds
        InvokeRepeating("SpawnCube", 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCube()
    {
        try {
            hitCubes.Add(CreateCube());
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        RemoveOutOfBoundCubes();
    }

    public GameObject CreateCube()
    {
        GameObject cube = null;
        // Create a new cube
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        var initialCubeModel = GameObject.Find("Cube");

        if (initialCubeModel == null)
            throw new System.Exception("Cube model not found");

        try {
            // Clone cube
            cube = Instantiate(initialCubeModel, new Vector3(0, 0, 0), Quaternion.identity); 

            // Set plane as parent of cube to HitCubeSpawner
            // Set the position of the cube to the position of the spawner
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(0.25f, 1.5f);
            cube.transform.position = new Vector3(randomX, randomY, transform.position.z);
            cube.transform.rotation = Quaternion.Euler(0, 0, 0);

            // Active MeshRenderer
            cube.GetComponent<MeshRenderer>().enabled = true;
            // Active Collider
            cube.GetComponent<Collider>().enabled = true;

            // Add a rigidbody to the cube
            cube.AddComponent<Rigidbody>();
            // Add a script to the cube
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

            
        } catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        return cube;
    }

    private void RemoveOutOfBoundCubes()
    {
        hitCubes.ForEach((cube) => {
            if (cube != null && cube.transform.position.z < -10)
                Destroy(cube);
        });

    }
}
