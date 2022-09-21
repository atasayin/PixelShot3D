using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : AbstarctMonoSingleton<VibrationManager>
{
    private bool _isVibEnable = true;


    public void EnableVibration()
    {
        _isVibEnable = true;
    }

    public void DisableVibration()
    {
        _isVibEnable = false;
    }

    public void SwitchVibration()
    {
        _isVibEnable = !_isVibEnable;
    }


    public bool IsVibEnable
    {
        get { return _isVibEnable; }
    }

}
