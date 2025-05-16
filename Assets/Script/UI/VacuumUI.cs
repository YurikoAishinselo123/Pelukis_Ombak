using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VacuumUI : MonoBehaviour
{
    public static VacuumUI Instance { get; private set; }

    [SerializeField] private GameObject VacuumObject;

    void Awake()
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

    void Start()
    {
        VacuumObject.SetActive(false);
    }

}
