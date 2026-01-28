using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GorillaLocomotion;
using easyInputs;

public class TeleporGun : MonoBehaviour
{
    [Header("made by roasty, made for egs devhub")]
    public float tpCooldown;
    public GameObject GorillaPlayer;
    private float nextSpawn;
    private static GameObject sphere;
    private static LineRenderer lineRenderer;

    private void Update()
    {
        bool gdown = EasyInputs.GetGripButtonDown(EasyHand.RightHand);
        bool tdown = EasyInputs.GetTriggerButtonDown(EasyHand.RightHand);
        Transform hand = GorillaLocomotion.Player.Instance.rightHandTransform;

        if (gdown)
        {
            RaycastHit hit;
            if (Physics.Raycast(hand.position, hand.forward, out hit, Mathf.Infinity))
            {
                if (sphere == null)
                {
                    sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    sphere.GetComponent<Renderer>().material.color = Color.white;
                    Destroy(sphere.GetComponent<Collider>());
                    Destroy(sphere.GetComponent<Rigidbody>());
                    Destroy(sphere.GetComponent<BoxCollider>());
                    Destroy(sphere.GetComponent<SphereCollider>());
                    lineRenderer = sphere.AddComponent<LineRenderer>();
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.startWidth = 0.01f;
                    lineRenderer.endWidth = 0.01f;
                    lineRenderer.positionCount = 2;
                    lineRenderer.startColor = Color.white;
                    lineRenderer.endColor = Color.white;
                }
                sphere.transform.position = hit.point;
                lineRenderer.SetPosition(0, hand.position);
                lineRenderer.SetPosition(1, hit.point);
            }
        }
        else
        {
            if (sphere != null)
            {
                Destroy(sphere);
                sphere = null;
            }

            if (lineRenderer != null)
            {
                Destroy(sphere);
                lineRenderer = null;
            }
        }

        if (tdown)
        {
            Vector3 tpPoint = sphere.transform.position;
            if (sphere != null)
            {
                sphere.GetComponent<Renderer>().material.color = Color.red;
                if (Time.time >= nextSpawn)
                {
                    nextSpawn = Time.time + tpCooldown;
                    GorillaPlayer.transform.position = tpPoint;
                }
            }
        }
    }
}