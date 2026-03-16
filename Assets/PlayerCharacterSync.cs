using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerCharacterSync : NetworkBehaviour
{
    public Sprite[] characterSprites;

    private SpriteRenderer sr;

    private NetworkVariable<int> characterId = new NetworkVariable<int>(
        -1,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    public override void OnNetworkDespawn()
    {
        characterId.OnValueChanged -= OnCharacterIdChanged;
    }

    private void OnCharacterIdChanged(int oldValue, int newValue)
    {
        ApplySprite(newValue);
    }

    private void ApplySprite(int id)
    {
        if (id < 0 || characterSprites == null || id >= characterSprites.Length) return;
        sr.sprite = characterSprites[id];
    }
    public override void OnNetworkSpawn()
{
    characterId.OnValueChanged += OnCharacterIdChanged;

    if (IsServer)
    {
        int chosenId = PlayerSelectionStore.GetSelection(OwnerClientId);
        characterId.Value = chosenId;

        Vector3 spawnPos = PlayerSpawnManager.Instance.GetSpawnPosition(OwnerClientId);
        transform.position = spawnPos;
    }

    if (characterId.Value != -1)
        ApplySprite(characterId.Value);
}
}