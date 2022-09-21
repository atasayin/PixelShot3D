using UnityEngine;
using MoreMountains.NiceVibrations;

public class DeathVibration : IDeathEffect
{
    private MonoBehaviour _monoBehaviour;

    public DeathVibration(MonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
    }

    public void InvokeDeathEffect()
    {
        if (VibrationManager.Instance.IsVibEnable)
        {
            MMVibrationManager.Haptic(HapticTypes.Success, false, true, _monoBehaviour);

        }


    }
}
