using System.Collections;
using GorillaNetworking;
using UnityEngine;
using System.Collections.Generic;

public class BananaOSMenuButton1 : GorillaPressableButton
{
    [System.Serializable]
    public class ToggledObject
    {
        public GameObject gameObject;
        public Material toggledMaterial;
        public Material originalMaterial;
    }

    public List<ToggledObject> ObjectsToToggle;
    public float buttonFadeTime = 0.25f;

    private bool isButtonToggled = false;
    private bool wasButtonToggled = false;

    public override void ButtonActivation()
    {
        wasButtonToggled = isButtonToggled;
        isButtonToggled = !isButtonToggled; // Toggle the button state

        foreach (ToggledObject toggledObject in ObjectsToToggle)
        {
            toggledObject.gameObject.SetActive(isButtonToggled);
            Renderer objectRenderer = toggledObject.gameObject.GetComponent<Renderer>();

            if (objectRenderer != null)
            {
                if (isButtonToggled)
                {
                    objectRenderer.material = toggledObject.toggledMaterial;
                }
                else
                {
                    objectRenderer.material = toggledObject.originalMaterial;
                }
            }
        }

        base.ButtonActivation();
        StartCoroutine(ButtonColorUpdate());
    }

    private IEnumerator ButtonColorUpdate()
    {
        buttonRenderer.material = pressedMaterial;

        if (!isButtonToggled && wasButtonToggled)
        {
            buttonRenderer.material = unpressedMaterial;
        }

        yield return new WaitForSeconds(buttonFadeTime);

        if (!isButtonToggled)
        {
            buttonRenderer.material = unpressedMaterial;
        }
    }
}
