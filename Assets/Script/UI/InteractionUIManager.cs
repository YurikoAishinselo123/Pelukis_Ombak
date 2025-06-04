using System.Collections.Generic;
using UnityEngine;

public class InteractionUIManager : MonoBehaviour
{
    public static InteractionUIManager Instance;

    [Header("References")]
    [SerializeField] private GameObject interactionUIPrefab;
    [SerializeField] private Transform interactionContainer;

    private Dictionary<IInteractable, InteractionUI> activeUIMap = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowInteraction(IInteractable interactable)
    {
        if (interactable == null || activeUIMap.ContainsKey(interactable)) return;

        GameObject uiGO = Instantiate(interactionUIPrefab, interactionContainer);
        InteractionUI ui = uiGO.GetComponent<InteractionUI>();

        if (ui != null)
        {
            ui.Setup(interactable.InteractionKey, interactable.InteractionIcon, interactable.InteractionText);
            activeUIMap[interactable] = ui;
        }
        else
        {
            Debug.LogWarning("Missing InteractionUI script on prefab.");
            Destroy(uiGO);
        }
    }

    public void HideInteraction(IInteractable interactable)
    {
        if (activeUIMap.TryGetValue(interactable, out var ui))
        {
            Destroy(ui.gameObject);
            activeUIMap.Remove(interactable);
        }
    }

    public void HideAllInteractions()
    {
        foreach (var ui in activeUIMap.Values)
        {
            if (ui != null)
                Destroy(ui.gameObject);
        }

        activeUIMap.Clear();
    }
}
