using UnityEngine;
using System;

[System.Serializable]
public class Tool
{
    public string toolsName;
    public GameObject toolsPrefab;
    public Vector3 toolsSpawnPosition;
    public Vector3 toolsSpawnRotation;

    [HideInInspector] public GameObject spawnedObject;
}

public class ItemSpawnManager : MonoBehaviour
{
    public Tool[] tools;

    void Start()
    {
        foreach (var tool in tools)
        {
            if (string.IsNullOrEmpty(tool.toolsName) || tool.toolsPrefab == null)
                continue;

            if (Enum.TryParse(tool.toolsName, out ItemType itemType))
            {
                bool isCollected = ItemManager.Instance.HasItem(itemType);

                if (!isCollected)
                {
                    Quaternion rotation = Quaternion.Euler(tool.toolsSpawnRotation);
                    GameObject instance = Instantiate(tool.toolsPrefab, tool.toolsSpawnPosition, rotation);
                    tool.spawnedObject = instance;
                    Debug.Log($"Spawned {tool.toolsName} at {tool.toolsSpawnPosition} with rotation {tool.toolsSpawnRotation}");
                }
            }
            else
            {
                Debug.LogWarning($"Invalid toolsName: {tool.toolsName} not found in ItemType enum.");
            }
        }
    }
}