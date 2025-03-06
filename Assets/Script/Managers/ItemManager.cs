using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    private bool hasCamera = false;
    private bool hasVacuum = false;
    // private int coinCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CollectCamera()
    {
        if (!hasCamera)
        {
            hasCamera = true;
            Debug.Log("Camera Collected!");
        }
    }

    // public void AddCoin(int amount)
    // {
    //     coinCount += amount;
    //     Debug.Log("Coin Collected: " + coinCount);
    // }

    public void CollectVacuum()
    {
        if (!hasVacuum)
        {
            hasVacuum = true;
            Debug.Log("Vacuum Collected!");
        }

    }

    public bool HasCamera()
    {
        return hasCamera;
    }

    public bool HasVacuum()
    {
        return hasVacuum;
    }
}

