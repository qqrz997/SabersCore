using UnityEngine;

namespace CustomSaber;

public class EventFilterBehaviour : MonoBehaviour
{
    private EventManager eventManager;

    protected EventManager EventManager
    {
        get
        {
            if (eventManager == null)
            {
                eventManager = GetComponent<EventManager>();
            }
            return eventManager;
        }
    }
}