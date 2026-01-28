using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WackyHead : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
				if (PhotonNetwork.InRoom)
				{
					GorillaTagger.Instance.myVRRig.head.rigTarget.eulerAngles = new Vector3((float)Random.Range(0, 360), (float)Random.Range(0, 360), (float)Random.Range(0, 360));
					return;
				}
				GorillaTagger.Instance.offlineVRRig.head.rigTarget.eulerAngles = new Vector3((float)Random.Range(0, 360), (float)Random.Range(0, 180), (float)Random.Range(0, 180));
    }
}
