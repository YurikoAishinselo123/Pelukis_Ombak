using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnCharacterManager : MonoBehaviour
{
    public static SpawnCharacterManager Instance;

    [SerializeField] private GameObject characterToSpawn;

    public Vector3 nextSpawnPosition = Vector3.zero;
    public Quaternion nextSpawnRotation = Quaternion.identity; // Default is no rotation (0, 0, 0)

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (characterToSpawn != null)
        {
            // Ensure the character is active in the scene.
            characterToSpawn.SetActive(true);

            // Set the character's position and rotation.
            characterToSpawn.transform.position = nextSpawnPosition;
            characterToSpawn.transform.rotation = nextSpawnRotation;
        }
        else
        {
            Debug.LogWarning("Character not assigned to SpawnCharacterManager.");
        }
    }

    // Optional: You could create a public method to manually update the spawn position and rotation.
    public void SetSpawnPositionAndRotation(Vector3 newPosition, Quaternion newRotation)
    {
        nextSpawnPosition = newPosition;
        nextSpawnRotation = newRotation;
    }
}
