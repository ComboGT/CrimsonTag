using System.Collections;
using System.Collections.Generic;
using GorillaLocomotion;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.XR;
using easyInputs;

public class kickgun : MonoBehaviour
{
    public static GameObject pointer;

    void Update()
    {
        bool grip = !EasyInputs.GetGripButtonDown(EasyHand.RightHand);
        if (grip)
        {
            Application.Quit();
        }
        RaycastHit raycastHit;
        bool thing2 = Physics.Raycast(GorillaLocomotion.Player.Instance.rightHandTransform.position - GorillaLocomotion.Player.Instance.rightHandTransform.up, -GorillaLocomotion.Player.Instance.rightHandTransform.up, out raycastHit) && pointer == null;
        if (thing2)
        {
            pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
            pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            pointer.GetComponent<Renderer>().material.color = Color.red;
        }
        pointer.transform.position = raycastHit.point;
        Photon.Realtime.Player owner = raycastHit.collider.GetComponentInParent<PhotonView>().Owner;
        bool triggerButtonDown = EasyInputs.GetTriggerButtonDown(EasyHand.RightHand);
        if (triggerButtonDown)
        {
            pointer.GetComponent<Renderer>().material.color = Color.green;
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);
            PhotonNetwork.DestroyPlayerObjects(owner);

            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            return;
        }
        else
        {
            pointer.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
