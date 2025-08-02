// ReSharper disable InconsistentNaming
using UnityEngine.Events;

namespace CustomSaber;

public class AccuracyReachedEvent : EventFilterBehaviour
{
    public float Target = 1f;
    public UnityEvent OnAccuracyReachTarget;
    public UnityEvent OnAccuracyHigherThanTarget;
    public UnityEvent OnAccuracyLowerThanTarget;
    
    private float prevAccuracy;

    private void OnEnable()
    {
        EventManager.OnAccuracyChanged.AddListener(OnAccuracyReached);
        prevAccuracy = 1f;
    }

    private void OnDisable()
    {
        EventManager.OnAccuracyChanged.RemoveListener(OnAccuracyReached);
    }

    private void OnAccuracyReached(float accuracy)
    {
        if ((prevAccuracy > Target && accuracy < Target) || (prevAccuracy < Target && accuracy > Target))
        {
            OnAccuracyReachTarget.Invoke();
        }
        if (prevAccuracy < Target && accuracy > Target)
        {
            OnAccuracyHigherThanTarget.Invoke();
        }
        if (prevAccuracy > Target && accuracy < Target)
        {
            OnAccuracyLowerThanTarget.Invoke();
        }
        prevAccuracy = accuracy;
    }
}