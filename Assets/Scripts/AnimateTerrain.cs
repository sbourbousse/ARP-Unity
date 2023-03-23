using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class AnimateTerrain : MonoBehaviour
{
    public List<GameObject> LineDecorators = new List<GameObject>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        LineDecorators = GenerateLineDecorators(12);
    }

    // Update is called once per frame
    void Update()
    {
        // Make LineDecorators Spin around Z axis
        AnimateLineDecorators();
    }

    public List<GameObject> GenerateLineDecorators(int count)
    {
        GameObject lineDecorator = GameObject.Find("LineDecorator");
        List<GameObject> lineDecorators = new List<GameObject>();
        for(int i = 0; i < count; i++)
        {
            // Clone LineDecorator 
            GameObject lineDecoratorClone = Instantiate(lineDecorator, new Vector3(0, 0, 0), Quaternion.identity);
            lineDecorators.Add(lineDecoratorClone);
        }
        // Move LineDecorators 1 by one on z axis spaced by 20
        for (int i = 0; i < lineDecorators.Count; i++)
        {
            lineDecorators[i].transform.position = new Vector3(0, 0, (i + 1) * 20);
        }
        
        return lineDecorators;
    }
    
    void AnimateLineDecorators()
    {
        for (int i = 0; i < LineDecorators.Count; i++)
        {
            float rotationDirection = 1;
            if(i % 2 == 0) rotationDirection = -1;
            LineDecorators[i].transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 0, rotationDirection), 0.05f);
            
            //Rotate at slow speed
        }
    }
}
