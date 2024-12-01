using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform playerSpawnPosition;
    [SerializeField] private GameObject playerPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instantiate(playerPrefab, playerSpawnPosition.position, playerSpawnPosition.rotation);
    }

    public void Respawn()
    {
        Instantiate(playerPrefab, playerSpawnPosition.position, playerSpawnPosition.rotation);
    }
    
}
