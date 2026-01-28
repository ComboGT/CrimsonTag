using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using easyInputs;          // <-- brings EasyInputs and EasyHand into scope

public class FlyWithA : MonoBehaviour
{
    [Header("FLY WITH A.")]
    [Header("Gorilla player (Rigidbody).")]
    public Rigidbody gorillaPlayer;

    [Header("Hand to read input from.")]
    public EasyHand hand;               // LeftHand or RightHand

    [Header("Speed (recommended 20).")]
    public float speed = 15.0f;

    [Header("Controller object (the one you hold).")]
    public GameObject controller;       // renamed for clarity

    void Update()
    {
        // 1️⃣ Make sure we have everything we need
        if (gorillaPlayer == null || controller == null) return;

        // 2️⃣ Use the correct EasyInputs method.
        //    The source you provided defines GetTriggerButtonDown, not GetPrimaryButtonDown [1].
        if (EasyInputs.GetTriggerButtonDown(hand))
        {
            // Forward direction of the controller in world space
            Vector3 forceDirection = controller.transform.forward;

            // Apply a simple velocity change (no physics forces needed for a “fly” feel)
            gorillaPlayer.linearVelocity = speed * forceDirection;
        }
    }
}