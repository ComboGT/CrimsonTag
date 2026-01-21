using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public string URLToOpen;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
        {
            Application.OpenURL(URLToOpen);
            Debug.Log("Funni Link Opened");
        }
    }
}
