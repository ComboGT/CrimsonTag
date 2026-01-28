using easyInputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMonkey : MonoBehaviour
{
    public EasyHand RightHandController;
    public EasyHand LeftHandController;
    public Transform Head;
    public Rigidbody gorillaPlayer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (EasyInputs.GetGripButtonDown(LeftHandController))
        {
            if (EasyInputs.GetTriggerButtonDown(LeftHandController))
            {
                if (EasyInputs.GetGripButtonDown(RightHandController))
                {
                    if (EasyInputs.GetTriggerButtonDown(RightHandController))
                    {

                        Vector3 forceDirection = Head.transform.forward;
                        Vector3 force = 20f * forceDirection;
                        gorillaPlayer.linearVelocity = 10 * forceDirection;
                    }
                }

            }
        }
    }
}