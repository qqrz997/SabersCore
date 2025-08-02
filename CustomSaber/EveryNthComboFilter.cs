// ReSharper disable InconsistentNaming
using UnityEngine.Events;
 
namespace CustomSaber;

public class EveryNthComboFilter : EventFilterBehaviour
{
    public int ComboStep = 50;
    public UnityEvent NthComboReached;

    private void OnEnable()
    {
        EventManager.OnComboChanged.AddListener(OnComboStep);
    }

    private void OnDisable()
    {
        EventManager.OnComboChanged.RemoveListener(OnComboStep);
    }

    private void OnComboStep(int combo)
    {
        if (combo % ComboStep == 0 && combo != 0)
        {
            NthComboReached.Invoke();
        }
    }
}