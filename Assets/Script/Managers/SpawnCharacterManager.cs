using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnCharacterManager : MonoBehaviour
{
    public static SpawnCharacterManager Instance;

    [SerializeField] private GameObject character; // Reference to the existing character in the scene

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
        if (character != null)
        {
            var controller = character.GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            character.transform.position = spawnPosition;

            if (controller != null) controller.enabled = true;

            Debug.Log("Character moved to: " + character.transform.position);
        }
        else
        {
            Debug.LogWarning("Character not assigned!");
        }
    }
}
