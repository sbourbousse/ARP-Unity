using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe;
using Mediapipe.Unity;
using UnityEditor;
using LandmarkType = Mediapipe.Unity.PoseLandmarkListAnnotation.PoseLandMarkModelPoint;

public class SkeletonController : MonoBehaviour
{    


    [SerializeField] float landMarkSpacing = 3f;
    
    //Skeleton head
    //Upper part
    public GameObject head;
    public GameObject neck;
    public GameObject shoulderLeft;
    public GameObject shoulderRight;
    public GameObject upperArmLeft;
    public GameObject upperArmRight;
    public GameObject lowerArmLeft;
    public GameObject lowerArmRight;
    public GameObject handLeft;
    public GameObject handRight;
    
    //Middle part
    public GameObject spine;
    public GameObject hips;
    
    //Lower part
    public GameObject upperLegLeft;
    public GameObject upperLegRight;
    public GameObject lowerLegLeft;
    public GameObject lowerLegRight;
    public GameObject footLeft;
    public GameObject footRight;

    public List<GameObject> bodyParts;
    
    
    

    
    public Vector3[] poseLandmarkListPositions;
    public Vector3[] skeletonLandmarkListPositions;

    public List<Tuple<GameObject, Vector3>> initialBodyPartRotations;
    
    int updateIteration = 0;

    private Vector3 unityToMediapipeCoordinateDifference;

    // Start is called before the first frame update
    void Start()
    {
        unityToMediapipeCoordinateDifference = Vector3.zero;
        
        poseLandmarkListPositions = new Vector3[33];
        skeletonLandmarkListPositions = new Vector3[33];
        initialBodyPartRotations = new List<Tuple<GameObject, Vector3>>();
        
        // Set null to all the skeleton poseLandmarkListPositions
        for (int i = 0; i < poseLandmarkListPositions.Length; i++)
        {
            poseLandmarkListPositions[i] = Vector3.zero;
        }
        
        // Set null to all the skeleton skeletonLandmarkListPositions
        for (int i = 0; i < skeletonLandmarkListPositions.Length; i++)
        {
            skeletonLandmarkListPositions[i] = Vector3.zero;
        }
        


        //Initialize skeleton
        
        
        //Upper part
        
        head = GameObject.Find("mixamorig:Head");
        neck = GameObject.Find("mixamorig:Neck");
        
        shoulderLeft = GameObject.Find("mixamorig:LeftShoulder");
        shoulderRight = GameObject.Find("mixamorig:RightShoulder");
        
        upperArmLeft = GameObject.Find("mixamorig:LeftArm");
        upperArmRight = GameObject.Find("mixamorig:RightArm");

        lowerArmLeft = GameObject.Find("mixamorig:LeftForeArm");
        lowerArmRight = GameObject.Find("mixamorig:RightForeArm");

        handLeft = GameObject.Find("mixamorig:LeftHand");
        handRight = GameObject.Find("mixamorig:RightHand");
        
        //Middle part
        spine = GameObject.Find("mixamorig:Spine");
        hips = GameObject.Find("mixamorig:Hips");
        
        //Lower part
        upperLegLeft = GameObject.Find("mixamorig:LeftUpLeg");
        upperLegRight = GameObject.Find("mixamorig:RightUpLeg");
        
        lowerLegLeft = GameObject.Find("mixamorig:LeftLeg");
        lowerLegRight = GameObject.Find("mixamorig:RightLeg");
        
        footLeft = GameObject.Find("mixamorig:LeftFoot");
        footRight = GameObject.Find("mixamorig:RightFoot");
        
        bodyParts = new List<GameObject>();
        bodyParts.Add(shoulderLeft);
        bodyParts.Add(shoulderRight);
        bodyParts.Add(upperArmLeft);
        bodyParts.Add(upperArmRight);
        bodyParts.Add(lowerArmLeft);
        bodyParts.Add(lowerArmRight);
        bodyParts.Add(upperLegLeft);
        bodyParts.Add(upperLegRight);
        bodyParts.Add(lowerLegLeft);
        bodyParts.Add(lowerLegRight);
        

        foreach (var bodyPart in bodyParts)
        {
            initialBodyPartRotations.Add(new Tuple<GameObject, Vector3>(bodyPart, bodyPart.transform.rotation.eulerAngles));
        }
    }

    // Update skeleton body parts local positions 
    void Update()
    {
        //Upper part
        head.transform.position = poseLandmarkListPositions[(int)LandmarkType.NOSE];
        neck.transform.position = (poseLandmarkListPositions[(int)LandmarkType.LEFT_SHOULDER] +
                                   poseLandmarkListPositions[(int)LandmarkType.RIGHT_SHOULDER]) / 2f;

        
        // Set the shoulder positions
        shoulderLeft.transform.position = poseLandmarkListPositions[(int)LandmarkType.LEFT_SHOULDER];
        shoulderRight.transform.position = poseLandmarkListPositions[(int)LandmarkType.RIGHT_SHOULDER];

        // Set the upper arm positions and rotations
        upperArmLeft.transform.position = (poseLandmarkListPositions[(int)LandmarkType.LEFT_SHOULDER] +
                                            poseLandmarkListPositions[(int)LandmarkType.LEFT_ELBOW]) / 2f;
        upperArmRight.transform.position = (poseLandmarkListPositions[(int)LandmarkType.RIGHT_SHOULDER] +
                                             poseLandmarkListPositions[(int)LandmarkType.RIGHT_ELBOW]) / 2f;

        upperArmLeft.transform.rotation = Quaternion.FromToRotation(Vector3.right, 
                                poseLandmarkListPositions[(int)LandmarkType.LEFT_ELBOW] -
                                poseLandmarkListPositions[(int)LandmarkType.LEFT_SHOULDER]);
        upperArmRight.transform.rotation = Quaternion.FromToRotation(Vector3.right, 
                                poseLandmarkListPositions[(int)LandmarkType.RIGHT_ELBOW] -
                                poseLandmarkListPositions[(int)LandmarkType.RIGHT_SHOULDER]);

        // Set the lower arm positions and rotations
        lowerArmLeft.transform.position = (poseLandmarkListPositions[(int)LandmarkType.LEFT_ELBOW] +
                                            poseLandmarkListPositions[(int)LandmarkType.LEFT_WRIST]) / 2f;
        lowerArmRight.transform.position = (poseLandmarkListPositions[(int)LandmarkType.RIGHT_ELBOW] +
                                             poseLandmarkListPositions[(int)LandmarkType.RIGHT_WRIST]) / 2f;

        lowerArmLeft.transform.rotation = Quaternion.FromToRotation(Vector3.right, 
                                poseLandmarkListPositions[(int)LandmarkType.LEFT_WRIST] -
                                poseLandmarkListPositions[(int)LandmarkType.LEFT_ELBOW]);
        lowerArmRight.transform.rotation = Quaternion.FromToRotation(Vector3.right, 
                                poseLandmarkListPositions[(int)LandmarkType.RIGHT_WRIST] -
                                poseLandmarkListPositions[(int)LandmarkType.RIGHT_ELBOW]);

        // Set the hand positions
        handLeft.transform.position = poseLandmarkListPositions[(int)LandmarkType.LEFT_WRIST];
        handRight.transform.position = poseLandmarkListPositions[(int)LandmarkType.RIGHT_WRIST];

        
        //Middle part
        spine.transform.position = (poseLandmarkListPositions[(int)LandmarkType.LEFT_HIP] +
                                    poseLandmarkListPositions[(int)LandmarkType.RIGHT_HIP]) / 2f;
        hips.transform.position = (poseLandmarkListPositions[(int)LandmarkType.LEFT_HIP] +
                                   poseLandmarkListPositions[(int)LandmarkType.RIGHT_HIP]) / 2f;
        
        
        //Lower part
        // Set the upper leg positions and rotations
        upperLegLeft.transform.position = (poseLandmarkListPositions[(int)LandmarkType.LEFT_HIP] +
                                            poseLandmarkListPositions[(int)LandmarkType.LEFT_KNEE]) / 2f;
        upperLegRight.transform.position = (poseLandmarkListPositions[(int)LandmarkType.RIGHT_HIP] +
                                             poseLandmarkListPositions[(int)LandmarkType.RIGHT_KNEE]) / 2f;
        upperLegLeft.transform.rotation = Quaternion.FromToRotation(Vector3.up, 
            poseLandmarkListPositions[(int)LandmarkType.LEFT_KNEE] -
            poseLandmarkListPositions[(int)LandmarkType.LEFT_HIP]);
        upperLegRight.transform.rotation = Quaternion.FromToRotation(Vector3.up, 
            poseLandmarkListPositions[(int)LandmarkType.RIGHT_KNEE] -
            poseLandmarkListPositions[(int)LandmarkType.RIGHT_HIP]);

        // Set the lower leg positions and rotations
        lowerLegLeft.transform.position = (poseLandmarkListPositions[(int)LandmarkType.LEFT_KNEE] +
                                           poseLandmarkListPositions[(int)LandmarkType.LEFT_ANKLE]) / 2f;
        lowerLegRight.transform.position = (poseLandmarkListPositions[(int)LandmarkType.RIGHT_KNEE] +
                                            poseLandmarkListPositions[(int)LandmarkType.RIGHT_ANKLE]) / 2f;

        lowerLegLeft.transform.rotation = Quaternion.FromToRotation(Vector3.up, 
            poseLandmarkListPositions[(int)LandmarkType.LEFT_ANKLE] -
            poseLandmarkListPositions[(int)LandmarkType.LEFT_KNEE]);
        lowerLegRight.transform.rotation = Quaternion.FromToRotation(Vector3.up, 
            poseLandmarkListPositions[(int)LandmarkType.RIGHT_ANKLE] -
            poseLandmarkListPositions[(int)LandmarkType.RIGHT_KNEE]);

        // Set the foot positions
        footLeft.transform.position = poseLandmarkListPositions[(int)LandmarkType.LEFT_ANKLE];
        footRight.transform.position = poseLandmarkListPositions[(int)LandmarkType.RIGHT_ANKLE];
        

        OrientBodyPart(head, neck);
        // OrientBodyPart(neck, spine);
        // OrientBodyPart(spine, hips);
        OrientBodyPart(hips, upperLegLeft);
        OrientBodyPart(hips, upperLegRight);
        OrientBodyPart(upperLegLeft, lowerLegLeft);
        OrientBodyPart(upperLegRight, lowerLegRight);
        OrientBodyPart(lowerLegLeft, footLeft);
        OrientBodyPart(lowerLegRight, footRight);
        OrientBodyPart(shoulderLeft, upperArmLeft);
        OrientBodyPart(shoulderRight, upperArmRight);
        OrientBodyPart(upperArmLeft, lowerArmLeft);
        OrientBodyPart(upperArmRight, lowerArmRight);
        OrientBodyPart(lowerArmLeft, handLeft);
        OrientBodyPart(lowerArmRight, handRight);
        

        // updateIteration++;
        // if (updateIteration == 100)
        // {
        //     unityToMediapipeCoordinateDifference = ComputeUnityToMediapipeCoordinateDifference();
        //     updateIteration = 0;
        // }
        //
        // foreach(var bodyPart in bodyParts)
        // {
        //
        //     bodyPart.transform.eulerAngles = new Vector3(
        //         bodyPart.transform.eulerAngles.x - unityToMediapipeCoordinateDifference.x,
        //         bodyPart.transform.eulerAngles.y - unityToMediapipeCoordinateDifference.y,
        //         bodyPart.transform.eulerAngles.z - unityToMediapipeCoordinateDifference.z);
        //
        // }
        
        foreach(var bodyPart in bodyParts)
        {

            bodyPart.transform.eulerAngles = new Vector3(
                bodyPart.transform.eulerAngles.x + (-90.548f),
                bodyPart.transform.eulerAngles.y + (-22.526f),
                bodyPart.transform.eulerAngles.z + (2.772995f));

        }
    }

    Vector3 ComputeUnityToMediapipeCoordinateDifference()
    {
        // Compare to initialBodyPartRotations
        var unityToMediapipeCoordinateDifference = new Vector3();
        float x = 0;
        float y = 0;
        float z = 0;
        
        for (int i = 0; i < initialBodyPartRotations.Count; i++)
        {
            x += bodyParts[i].transform.eulerAngles.x - initialBodyPartRotations[i].Item2.x;
            y += bodyParts[i].transform.eulerAngles.y - initialBodyPartRotations[i].Item2.y;
            z += bodyParts[i].transform.eulerAngles.z - initialBodyPartRotations[i].Item2.z;
        }
        
        x /= initialBodyPartRotations.Count;
        y /= initialBodyPartRotations.Count;
        z /= initialBodyPartRotations.Count;
        
        return new Vector3(x, y, z);
    }

    void OrientBodyPart(GameObject bodyPart, GameObject bodyPartTo) 
    {
        //Joint transform
        bodyPart.transform.LookAt(bodyPartTo.transform.position);   
        Vector3 currentRotationInDegrees = bodyPart.transform.eulerAngles;
        // bodyPart.transform.eulerAngles = new Vector3(currentRotationInDegrees.x - (-90.548f), currentRotationInDegrees.y - (-22.526f), currentRotationInDegrees.z - (2.772995f));
    }
    
    
    
    
    
    void LinkLimbs()
    {
        // Link the limbs of the skeleton
        LinkLimbs(head, neck);
        // LinkLimbs(neck, shoulderLeft);
        // LinkLimbs(neck, shoulderRight);
        // LinkLimbs(shoulderLeft, upperArmLeft);
        // LinkLimbs(shoulderRight, upperArmRight);
        LinkLimbs(upperArmLeft, lowerArmLeft);
        LinkLimbs(upperArmRight, lowerArmRight);
        LinkLimbs(lowerArmLeft, handLeft);
        LinkLimbs(lowerArmRight, handRight);
        LinkLimbs(neck, upperLegLeft);
        LinkLimbs(neck, upperLegRight);
        LinkLimbs(upperLegLeft, lowerLegLeft);
        LinkLimbs(upperLegRight, lowerLegRight);
        LinkLimbs(lowerLegLeft, footLeft);
        LinkLimbs(lowerLegRight, footRight);
    }

    void LinkLimbs(GameObject parent, GameObject child)
    {
        // Link the specified child object to the specified parent object
        child.transform.SetParent(parent.transform, false);
    }
    
    // Space coordinates
    public void ApplySpacing()
    {
        for(int i = 0; i < poseLandmarkListPositions.Length; i++)
        {
            poseLandmarkListPositions[i].x *= this.landMarkSpacing;
            poseLandmarkListPositions[i].y *= this.landMarkSpacing;
            poseLandmarkListPositions[i].z *= this.landMarkSpacing;
        }
    }

    // NormalizedLandmarkList is a list of absolute coordinates provided by the mediapipe pose estimation
    public void UpdateLandmarksPositions(NormalizedLandmarkList landmakList)
    {
        if(landmakList is null)
            return;
        
        var landmarks = landmakList.Landmark;
        
        if(landmarks is null || landmarks.Count == 0)
            return;

        Debug.Log(landmarks.Count);
        for (int i = 0; i < landmarks.Count; i++)
        {
            var landmark = landmarks[i];
            poseLandmarkListPositions[i] = new Vector3(landmark.X , landmark.Y * -1, landmark.Z);
        }

        ApplySpacing();

    }
}
