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
        if (currentlySelected != null)
        {
            Outline oldOutline = currentlySelected.GetComponent<Outline>();
            if (oldOutline != null) oldOutline.enabled = false;
        }

        currentlySelected = clickedButton;

        Outline newOutline = currentlySelected.GetComponent<Outline>();
        if (newOutline != null) newOutline.enabled = true;

        if (playButton != null)
            playButton.interactable = true;
    }

    public void StartGame()
    {
        if (currentlySelected == null)
            return;

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

        // ONLY the host should trigger scene loads
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("Client selected character. Waiting for host to start...");
        }
    }
}