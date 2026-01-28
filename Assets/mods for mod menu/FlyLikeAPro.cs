using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using easyInputs;

public class FlyLikeAPro : MonoBehaviour
{
    [Header("FLY MOD FOR YOUR GAME")]
    [Header("put gorilla player in, not gorilla rig.")]
    public Rigidbody gorillaPlayer;
    [Header("the hand you want.")]
    public EasyHand hand;
    [Header("speed, i would recommend around 15-25 :thumbsup:")]
    public float speed = 15.0f;
    [Header("controller, if you put in hand left, put in your left hand controller.")]
    public GameObject Controller;

    void Update()
    {
        if (EasyInputs.GetTriggerButtonDown(hand))
        {
             Vector3 forceDirection = Controller.transform.forward;
             Vector3 force = speed * forceDirection;
            gorillaPlayer.linearVelocity = speed * forceDirection;
        }
    }
}
