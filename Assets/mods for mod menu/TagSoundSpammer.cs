using GorillaLocomotion;
using System.Collections.Generic;
using UnityEngine;
using GorillaNetworking;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;
using easyInputs;
using ExitGames.Client.Photon;

public class TagSoundSpammer : MonoBehaviour
{
    [Header("SCRIPT IS FROM FONKEY, NO CREDIT NEEDED.")]
    [Header("-----------------")]
    [Header("MAKE SURE YOU HAVE EASYINPUTS FROM MY DISCORD SERVER OR ELSE THIS WONT WORK")]

    public bool SpamTagSound;

    private static VRRig[] GetAllVRRigs()
    {
        return UnityEngine.Object.FindObjectsOfType<VRRig>();
    }

    void Start()
    {

    }


    void Update()
    {
        if (SpamTagSound)
        {
            if (PhotonNetwork.InRoom && GorillaTagger.Instance.myVRRig != null)
            {
                PhotonView.Get(GorillaTagger.Instance.myVRRig).RPC("PlayTagSound", RpcTarget.All, new object[]
                {
                   2,
                   90f
                });
            }
        }
    }
}
       