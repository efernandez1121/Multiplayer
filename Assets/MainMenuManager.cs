using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Text statusText;

    private void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager.Singleton is NULL. Make sure a NetworkManager exists in this scene.");
            return;
        }

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    public void StartHost()
    {
        Debug.Log("StartHost button clicked.");

        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager.Singleton is NULL.");
            return;
        }

        bool started = NetworkManager.Singleton.StartHost();
        Debug.Log("StartHost result: " + started);

        if (statusText != null)
            statusText.text = "Status: Hosting...";
        else
            Debug.LogWarning("statusText is NOT assigned in the Inspector.");

        if (NetworkManager.Singleton.SceneManager != null)
        {
            Debug.Log("Loading CharacterSelect...");
            NetworkManager.Singleton.SceneManager.LoadScene("CharacterSelect", LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("NetworkManager.SceneManager is NULL. Make sure Enable Scene Management is checked on NetworkManager.");
        }
    }

    public void StartClient()
    {
        Debug.Log("StartClient button clicked.");

        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager.Singleton is NULL.");
            return;
        }

        bool started = NetworkManager.Singleton.StartClient();
        Debug.Log("StartClient result: " + started);

        if (statusText != null)
            statusText.text = "Status: Connecting...";
        else
            Debug.LogWarning("statusText is NOT assigned in the Inspector.");
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client connected: {clientId}");

        if (NetworkManager.Singleton != null &&
            clientId == NetworkManager.Singleton.LocalClientId &&
            statusText != null)
        {
            statusText.text = NetworkManager.Singleton.IsHost
                ? "Status: Hosting..."
                : "Status: Connected!";
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client disconnected: {clientId}");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }
}