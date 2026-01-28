using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowGravity : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Physics.gravity = new Vector3(0f, -3f, 0f);
    }

    // Update is called once per frame
    void OnDisable()
    {
        Physics.gravity = new Vector3(0f, -9.81f, 0f);
    }
}
