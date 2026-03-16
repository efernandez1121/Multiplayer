using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance;

    public Transform hostSpawnPoint;
    public Transform clientSpawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GetSpawnPosition(ulong clientId)
    {
        // Host is always clientId 0 in Netcode
        if (clientId == 0)
            return hostSpawnPoint.position;

        return clientSpawnPoint.position;
    }
}
