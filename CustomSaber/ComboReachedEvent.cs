// ReSharper disable InconsistentNaming
using UnityEngine.Events;

namespace CustomSaber;

public class ComboReachedEvent : EventFilterBehaviour
{
    public int ComboTarget = 50;
    public UnityEvent NthComboReached;

    private void OnEnable()
    {
        EventManager.OnComboChanged.AddListener(OnComboReached);
    }

    private void OnDisable()
    {
        EventManager.OnComboChanged.RemoveListener(OnComboReached);
    }

    private void OnComboReached(int combo)
    {
        if (combo == ComboTarget)
        {
            NthComboReached.Invoke();
        }
    }
}