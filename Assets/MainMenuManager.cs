using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Text statusText;

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        statusText.text = "Status: Hosting...";
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        statusText.text = "Status: Connecting...";
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsHost)
        {
            SceneManager.LoadScene("CharacterSelect");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}