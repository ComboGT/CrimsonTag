//mod vent manager - Pholectify
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class ModVentManager : MonoBehaviour
{
    [Header("BGCH Tutorials - Mod Vents\nMade by aspect")]

    public Collider[] ventColliders;

    public void CheckYesAccess()
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "CanDoModVent",
            GeneratePlayStreamEvent = false
        };
        PlayFabClientAPI.ExecuteCloudScript(request, OnCloudScriptSuccess, OnCloudScriptError);
    }

    private void OnCloudScriptSuccess(ExecuteCloudScriptResult result)
    {
        bool canDo = false;
        string json = result.FunctionResult.ToString();
        try
        {
            Outcome resp = JsonUtility.FromJson<Outcome>(json);
            canDo = resp != null && resp.canDo;
            Debug.Log("random data for testing ignore\n" + resp);
        }
        catch
        {
            canDo = json.Contains("\"canDo\":true");
        }

        if (canDo)
        {
            foreach (var c in ventColliders)
            {
                c.enabled = false;
            }
        }
        else
        {
            return;
        }
    }

    private void OnCloudScriptError(PlayFabError errtoe)
    {
        Debug.LogError("for whatever reason the mod vent shit didnt work.. here's why:\n" + errtoe.GenerateErrorReport());
    }

    [System.Serializable]
    private class Outcome
    {
        public bool canDo;
    }
}