using System.Collections;
using System.Collections.Generic;
using GorillaLocomotion;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.XR;
using easyInputs;

public class TagGun : MonoBehaviour
{
    private static GameObject pointer; // JTMC HERE. THIS IS A PRIVATE GAMEOBJECT THAT IS SET IN LINE 26. BASICALLY IGNORE THIS

    void Update()
    {
        bool grip = !EasyInputs.GetGripButtonDown(EasyHand.RightHand);
        if (grip)
        {
            UnityEngine.Object.Destroy(pointer);
            return;
        }
        RaycastHit raycastHit;
        bool thing2 = Physics.Raycast(GorillaLocomotion.Player.Instance.rightHandTransform.position - GorillaLocomotion.Player.Instance.rightHandTransform.up, -GorillaLocomotion.Player.Instance.rightHandTransform.up, out raycastHit) && pointer == null;
        if (thing2)
        {
            pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere); // JTMC HERE. this creates the pointer
            UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
            pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            pointer.GetComponent<Renderer>().material.color = Color.white;
        }
        pointer.transform.position = raycastHit.point;
        Photon.Realtime.Player owner = raycastHit.collider.GetComponentInParent<PhotonView>().Owner;
        VRRig possibly = raycastHit.collider.GetComponentInParent<VRRig>();
        PhotonView possibly3 = raycastHit.collider.GetComponentInParent<PhotonView>();
        Photon.Realtime.Player possibly4 = possibly3.Owner;
        bool triggerButtonDown = EasyInputs.GetTriggerButtonDown(EasyHand.RightHand);
        if (triggerButtonDown)
        {
            pointer.GetComponent<Renderer>().material.color = Color.red;
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                foreach (GorillaTagManager gorillaTagManager in GameObject.FindObjectsOfType<GorillaTagManager>())
                {
                    PhotonView photonView = gorillaTagManager.FindVRRigForPlayer(possibly4);
                    if (!gorillaTagManager.currentInfected.Contains(possibly4))
                    {
                        gorillaTagManager.currentInfected.Add(possibly4);
                        photonView.RPC("PlayTagSound", RpcTarget.All, 0, 0.25f);
                    }
                }
            }
            else
            {
                PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            }
        }
        else
        {
            pointer.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}