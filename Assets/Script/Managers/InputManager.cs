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

    public Vector2 MoveInput => playerInput.actions["Move"].ReadValue<Vector2>();
    public Vector2 LookInput => playerInput.actions["Look"].ReadValue<Vector2>();
    public bool JumpPressed => playerInput.actions["Jump"].WasPressedThisFrame();
    public bool IsSprinting => playerInput.actions["Sprint"].IsPressed();
    public bool Interact => playerInput.actions["Interact"].IsPressed();
    public bool Action => playerInput.actions["Action"].WasPressedThisFrame();
    public bool IsItemCollectPressed => playerInput.actions["CollectItem"].WasPressedThisFrame();
    public bool GetCapturePhotoInput => playerInput.actions["CapturePhoto"].WasPressedThisFrame();


    public int GetSelectedItemByKey()
    {
        // if (Keyboard.current.digit1Key.wasPressedThisFrame) return 0;
        // if (Keyboard.current.digit2Key.wasPressedThisFrame) return 1;
        // if (Keyboard.current.digit3Key.wasPressedThisFrame) return 2;
        // if (Keyboard.current.digit4Key.wasPressedThisFrame) return 3;
        // return -1;

        //Temp
        if (Keyboard.current.digit1Key.wasPressedThisFrame) return 1;
        if (Keyboard.current.digit2Key.wasPressedThisFrame) return 2;
        if (Keyboard.current.digit3Key.wasPressedThisFrame) return 3;
        if (Keyboard.current.digit4Key.wasPressedThisFrame) return 4;
        return -1;
    }
}
