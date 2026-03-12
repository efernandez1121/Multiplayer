using UnityEngine;

public class SelectionData : MonoBehaviour
{
    public static int SelectedCharacterId;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}