using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;

public class CallModerator : GorillaPressableButton
{
    public string webhook_link;
    public float buttonFadeTime = 0.25f;

    public override void ButtonActivation()
    {
        {
            StartCoroutine(SendWebhook(webhook_link, "# **A MODERATOR WAS CALLED!**\n- *Room Name: " + PhotonNetwork.CurrentRoom.Name + "*\n- *CallerName: " + PhotonNetwork.LocalPlayer.NickName + "*\n- *PlayerId: " + PhotonNetwork.LocalPlayer.UserId + "*\n\n@everyone"));
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

    IEnumerator SendWebhook(string link, string message)
    {
        WWWForm form = new WWWForm();
        form.AddField("content", message);
        using (UnityWebRequest www = UnityWebRequest.Post(link, form))
        {
            yield return www.SendWebRequest();
        }
    }
}
