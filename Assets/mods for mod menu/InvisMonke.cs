using System.Collections;
using System.Collections.Generic;
using easyInputs;
using UnityEngine;
using UnityEngine.InputSystem;

public class InvisMonke : MonoBehaviour
{
    public List<GameObject> GhostView;
    void Update()
    {
        if (EasyInputs.GetPrimaryButtonDown(EasyHand.LeftHand))  if (Mouse.current.leftButton.isPressed)
        {
            foreach (GameObject obj in GhostView)
            {
                obj.SetActive(true);
            }
            GorillaTagger.Instance.myVRRig.enabled = false;
            GorillaTagger.Instance.myVRRig.transform.position = new Vector3 (99999f, 99999f, 99999f);
            GorillaTagger.Instance.handTapVolume = 0f;
        }
        else
        {
            foreach (GameObject obj in GhostView)
            {
                obj.SetActive(false);
            }
            GorillaTagger.Instance.myVRRig.enabled = true;
            GorillaTagger.Instance.handTapVolume = 0.1f;
        }
    }
}
