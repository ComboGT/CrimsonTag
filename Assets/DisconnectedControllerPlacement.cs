using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DisconnectedControllerPlacement : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The target object where disconnected controllers will be placed")]
    public Transform targetObject;
    
    [Tooltip("Offset position from the target object")]
    public Vector3 positionOffset = Vector3.zero;
    
    [Tooltip("Offset rotation from the target object")]
    public Vector3 rotationOffset = Vector3.zero;
    
    [Header("Controller References")]
    [Tooltip("Left hand controller model")]
    public GameObject leftControllerModel;
    
    [Tooltip("Right hand controller model")]
    public GameObject rightControllerModel;
    
    private bool wasLeftControllerConnected = true;
    private bool wasRightControllerConnected = true;
    
    // Cached transforms for optimization
    private Transform leftControllerTransform;
    private Transform rightControllerTransform;
    
    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("No target object assigned for disconnected controllers!");
            enabled = false;
            return;
        }
        
        if (leftControllerModel != null)
            leftControllerTransform = leftControllerModel.transform;
            
        if (rightControllerModel != null)
            rightControllerTransform = rightControllerModel.transform;
    }
    
    void Update()
    {
        CheckLeftController();
        CheckRightController();
    }
    
    void CheckLeftController()
    {
        bool isConnected = IsControllerConnected(XRNode.LeftHand);
        
        // If controller has just disconnected
        if (wasLeftControllerConnected && !isConnected)
        {
            if (leftControllerModel != null)
            {
                PlaceAtTarget(leftControllerTransform);
                Debug.Log("Left controller disconnected - placed at target");
            }
        }
        
        wasLeftControllerConnected = isConnected;
    }
    
    void CheckRightController()
    {
        bool isConnected = IsControllerConnected(XRNode.RightHand);
        
        // If controller has just disconnected
        if (wasRightControllerConnected && !isConnected)
        {
            if (rightControllerModel != null)
            {
                PlaceAtTarget(rightControllerTransform);
                Debug.Log("Right controller disconnected - placed at target");
            }
        }
        
        wasRightControllerConnected = isConnected;
    }
    
    void PlaceAtTarget(Transform controllerTransform)
    {
        if (targetObject != null && controllerTransform != null)
        {
            controllerTransform.position = targetObject.position + positionOffset;
            controllerTransform.rotation = targetObject.rotation * Quaternion.Euler(rotationOffset);
        }
    }
    
    bool IsControllerConnected(XRNode node)
    {
        List<XRNodeState> nodeStates = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodeStates);
        
        foreach (XRNodeState state in nodeStates)
        {
            if (state.nodeType == node)
            {
                return state.tracked;
            }
        }
        
        return false;
    }
    
    // Call this method from other scripts if you need to manually place controllers
    public void ManuallyPlaceControllers()
    {
        if (leftControllerModel != null)
            PlaceAtTarget(leftControllerTransform);
            
        if (rightControllerModel != null)
            PlaceAtTarget(rightControllerTransform);
    }
}