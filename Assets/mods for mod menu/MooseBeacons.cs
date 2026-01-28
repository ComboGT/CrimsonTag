using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MooseBeacons : MonoBehaviour
{
    public bool Beacons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()


    {
        if (Beacons)
        {
            Material material = new Material(Shader.Find("GUI/Text Shader"));
            material.color = new Color32(0, 151, 255, 1);


            foreach (VRRig vrrig in (VRRig[])UnityEngine.Object.FindObjectsOfType(typeof(VRRig)))
            {
                if (!vrrig.isOfflineVRRig && !vrrig.isMyPlayer && !vrrig.photonView.IsMine)
                {
                    GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                    gameObject.transform.rotation = Quaternion.identity;
                    gameObject.GetComponent<MeshRenderer>().material = vrrig.mainSkin.material;
                    gameObject.transform.localScale = new Vector3(0.1f, 100f, 0.1f);
                    gameObject.transform.position = vrrig.headMesh.transform.position;
                    UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
                }
            }
        }



    }


}
