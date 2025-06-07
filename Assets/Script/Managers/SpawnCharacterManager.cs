using UnityEngine;

public class SpawnCharacterManager : MonoBehaviour
{
    public static SpawnCharacterManager Instance;

    [SerializeField] private GameObject playerPrefab; // Assign your Player prefab here (used if not found in scene)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MoveCharacterToPosition(Vector3 spawnPosition)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            var controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                player.transform.position = spawnPosition;
                controller.enabled = true;

                Debug.Log("Player moved to: " + spawnPosition);
            }
            else
            {
                Debug.LogWarning("CharacterController not found on Player.");
            }
        }
        else
        {
            Debug.LogWarning("Player with tag 'Player' not found in the scene.");
        }
    }

    public void SpawnPositionOnStart(Vector3 spawnStartPosition)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            MoveCharacterToPosition(spawnStartPosition);
        }
        else
        {
            if (playerPrefab != null)
            {
                Instantiate(playerPrefab, spawnStartPosition, Quaternion.identity);
                Debug.Log("Player instantiated at: " + spawnStartPosition);
            }
            else
            {
                Debug.LogError("Player prefab is not assigned in SpawnCharacterManager.");
            }
        }
    }
}
