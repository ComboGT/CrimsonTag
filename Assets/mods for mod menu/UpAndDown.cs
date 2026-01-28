using easyInputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EasyInputs.GetTriggerButtonDown(EasyHand.RightHand))
        {
            GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 50f, 0f), ForceMode.Acceleration);
        }

        if (EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand))
        {
            GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -50f, 0f), ForceMode.Acceleration);
        }
    }
}

