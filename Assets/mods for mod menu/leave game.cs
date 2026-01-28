using UnityEngine;
using Photon.Pun; // Required if using PUN
using UnityEngine.SceneManagement;

public class DisconnectButton : MonoBehaviour
{
    public void DisconnectPlayer()
    {
        // Disconnects from Photon Server
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        // Load your main menu scene
        SceneManager.LoadScene("MainMenuSceneName");
    }
}
