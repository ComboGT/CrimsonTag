using System.Collections;
using GorillaNetworking;
using UnityEngine;
using System.Collections.Generic;

public class BananaOSMenuButton2 : GorillaPressableButton
{
[Header("zox made this :) give creds")]
    public List<GameObject> ObjectsToDisable;
    public List<GameObject> ObjectsToEnable;
    public float buttonFadeTime = 0.25f;
    public float activationDelay = 0.3f;

    [Header("Audio Settings")]
    public AudioClip clickSound; // Your custom sound
    private AudioSource audioSource;

    private void Start()
    {
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Optional: Configure it for 3D VR use
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
    }

    public override void ButtonActivation()
    {
        //  Play the sound immediately on press
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        StartCoroutine(DelayedButtonActivation());
    }

    private IEnumerator DelayedButtonActivation()
    {
        yield return new WaitForSeconds(activationDelay);

        foreach (GameObject obj in ObjectsToDisable)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in ObjectsToEnable)
        {
            obj.SetActive(true);
        }

        base.ButtonActivation();
        StartCoroutine(ButtonColorUpdate());
    }

    private IEnumerator ButtonColorUpdate()
    {
        buttonRenderer.material = pressedMaterial;
        yield return new WaitForSeconds(buttonFadeTime);
        buttonRenderer.material = unpressedMaterial;
    }
}
