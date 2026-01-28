using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEditor;
using GorillaLocomotion;
using UnityEngine.UI;
using UnityEngine.XR;

public class NoclipMod : MonoBehaviour
{
    static bool primaryDown = false;
    static bool no = false;
    static bool yes = true;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
       
            List<InputDevice> list = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller, list);
            list[0].TryGetFeatureValue(CommonUsages.primaryButton, out primaryDown);

            if (primaryDown)
            {
                if (!no)
                {
                    foreach (MeshCollider mc in Resources.FindObjectsOfTypeAll<MeshCollider>())
                        mc.transform.localScale = mc.transform.localScale / 10000;
                    no = true;
                    yes = false;
                }
            }
            else
            {
                if (!yes)
                {
                    foreach (MeshCollider mc in Resources.FindObjectsOfTypeAll<MeshCollider>())
                        mc.transform.localScale = mc.transform.localScale * 10000;
                    yes = true;
                    no = false;
                }
            }
    }
}
