using Unity.Cinemachine;
using Unity.VisualScripting;
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
            GameObject character2 = GameObject.FindGameObjectWithTag("Player");
            var controller = character2.GetComponent<CharacterController>();
            controller.enabled = false;

            character2.transform.position = spawnPosition;

            controller.enabled = true;

            Debug.Log("Character moved to: " + character.transform.position);
        }
        else if(character == null)
        {
            Debug.LogWarning("Character not assigned!");
        }
    }

    public void SpawnPositionOnStart(Vector3 spawnStartPosition)
    {
        GameObject character2 = GameObject.FindGameObjectWithTag("Player");
        if (character2 != null)
        {
            MoveCharacterToPosition(spawnStartPosition);
        }
        else
        {
            Instantiate(character, spawnStartPosition, Quaternion.identity);
        }
    }
}
