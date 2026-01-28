using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

// This name MUST match the filename "VisibilityManager.cs"
public class VisibilityManager : MonoBehaviour
{
    // The ID of the item the player needs to own
    public string requiredItemId = "Special_Access_Key";

    void Start()
    {
        // Make sure the player is logged in before checking
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            CheckAccess();
        }
        else
        {
            Debug.LogWarning("Player not logged in. Hiding item by default.");
            this.gameObject.SetActive(false);
        }
    }

    public void CheckAccess()
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "checkItemOwnership",
            FunctionParameter = new { itemId = requiredItemId }
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnSuccess, OnError);
    }

    void OnSuccess(ExecuteCloudScriptResult result)
    {
        // Safely extract the boolean from PlayFab's response
        if (result.FunctionResult != null)
        {
            var jsonResult = (System.Collections.Generic.Dictionary<string, object>)result.FunctionResult;
            if (jsonResult.ContainsKey("hasAccess"))
            {
                bool hasAccess = (bool)jsonResult["hasAccess"];
                this.gameObject.SetActive(hasAccess);
                return;
            }
        }
        this.gameObject.SetActive(false);
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError("PlayFab Error: " + error.GenerateErrorReport());
        this.gameObject.SetActive(false);
    }
}
