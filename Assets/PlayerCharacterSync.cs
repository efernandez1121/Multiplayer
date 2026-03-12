using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerCharacterSync : NetworkBehaviour
{
    [Header("Assign in Inspector (size 6)")]
    public Sprite[] characterSprites; // index 0-5 matches your selection

    private SpriteRenderer sr;

    // This is what replicates to everyone
    private NetworkVariable<int> characterId = new NetworkVariable<int>(
        -1,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    // Server keeps track of taken characters
    private static HashSet<int> taken = new HashSet<int>();

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public override void OnNetworkSpawn()
    {
        characterId.OnValueChanged += OnCharacterIdChanged;

        // If we already have a value (late join), apply it
        if (characterId.Value != -1)
            ApplySprite(characterId.Value);

        // Owner sends chosen character to server
        if (IsOwner)
        {
            int chosen = SelectionData.SelectedCharacterId;
            SubmitCharacterChoiceServerRpc(chosen);
        }
    }

    public override void OnNetworkDespawn()
    {
        characterId.OnValueChanged -= OnCharacterIdChanged;

        if (IsServer && characterId.Value != -1)
        {
            taken.Remove(characterId.Value);
        }
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

    [ServerRpc(RequireOwnership = true)]
    private void SubmitCharacterChoiceServerRpc(int requestedId)
    {
        int finalId = GetAvailableCharacter(requestedId);

        // Mark taken and set network variable
        taken.Add(finalId);
        characterId.Value = finalId;
    }

    private int GetAvailableCharacter(int requested)
    {
        // If requested is valid and not taken, accept it
        if (requested >= 0 && requested < characterSprites.Length && !taken.Contains(requested))
            return requested;

        // Otherwise give first free
        for (int i = 0; i < characterSprites.Length; i++)
        {
            if (!taken.Contains(i))
                return i;
        }

        // If somehow all taken, fallback
        return 0;
    }
}