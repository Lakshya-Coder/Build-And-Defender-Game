using UnityEngine.EventSystems;
using UnityEngine;

public class DoNotSelectAnyObject : MonoBehaviour
{    
    [SerializeField] private bool isAllowSelectButtons;
    private EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
        
        if (eventSystem == null || isAllowSelectButtons)
            Destroy(this);
    }

    private void Update() =>eventSystem.SetSelectedGameObject(null);
}
