using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using easyInputs;

public class TriggerFly : MonoBehaviour
{
    [Header("Trigger Fly thingy fly by hiker")]
    [Header("put gorilla player in, not gorilla rig.")]
    public Rigidbody gorillaPlayer;
    public EasyHand hand;
    public float speed = 15.0f;
    public GameObject Controller;

    void Update()
    {
        if (EasyInputs.GetTriggerButtonTouched(hand))
        {
             Vector3 forceDirection = Controller.transform.forward;
             Vector3 force = speed * forceDirection;
            gorillaPlayer.linearVelocity = speed * forceDirection;
        }
    }
}
