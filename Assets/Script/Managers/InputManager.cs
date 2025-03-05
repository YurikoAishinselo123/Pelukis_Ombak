using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInput playerInput;

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

        playerInput = GetComponent<PlayerInput>();
    }

    public Vector2 GetMoveInput()
    {
        return playerInput.actions["Move"].ReadValue<Vector2>();
    }

    public bool IsSprinting()
    {
        return playerInput.actions["Sprint"].IsPressed();
    }

    public bool IsItemCollectPressed()
    {
        return playerInput.actions["CollectItem"].WasPressedThisFrame();
    }
    // public bool isInventoryOpen()
    // {
    //     return playerInput.actions("Open Inventory")
    // }
}
