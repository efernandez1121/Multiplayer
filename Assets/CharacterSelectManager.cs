using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    public Button playButton;
    private Button currentlySelected;

    void Start()
    {
        if (playButton != null)
            playButton.interactable = false;
    }

    public void SelectCharacter(Button clickedButton)
    {
        // Turn off old selection outline
        if (currentlySelected != null)
        {
            Outline oldOutline = currentlySelected.GetComponent<Outline>();
            if (oldOutline != null)
                oldOutline.enabled = false;
        }

        // Set new selection
        currentlySelected = clickedButton;

        Outline newOutline = currentlySelected.GetComponent<Outline>();
        if (newOutline != null)
            newOutline.enabled = true;

        // SAVE the selected character immediately
        SelectionData.SelectedCharacterId = currentlySelected.name switch
        {
            "Enchantress" => 0,
            "Musketeer" => 1,
            "Knight" => 2,
            "Swordsman" => 3,
            "Wizard" => 4,
            "Archer" => 5,
            _ => -1
        };

        Debug.Log("Selected character ID = " + SelectionData.SelectedCharacterId);

        // Enable play button
        if (playButton != null)
            playButton.interactable = true;
    }

    public void StartGame()
    {
        if (currentlySelected == null)
            return;

        // Only host starts the scene load
        if (NetworkManager.Singleton.IsHost)
        {
            int connectedCount = NetworkManager.Singleton.ConnectedClientsList.Count;
            Debug.Log("Connected clients count: " + connectedCount);

            if (connectedCount < 2)
            {
                Debug.LogWarning("Need both host and client connected before starting Level1.");
                return;
            }

            NetworkManager.Singleton.SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("Client selected character. Waiting for host to start...");
        }
    }
}