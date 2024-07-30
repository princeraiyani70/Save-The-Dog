using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;
//using Obvious.Soap;

public class HapticManager : MonoBehaviour
{

    public static HapticManager Instance;

    //[SerializeField] private BoolVariable SHaptic;

    private void Awake()
    {
        if (FindObjectsOfType(typeof(HapticManager)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("HapticManager Call");
    }

    public void SoftHapticCalled()
    {
        if (PlayerPrefs.GetInt("Vibrate") == 0)
        {
            //MMVibrationManager.Haptic(HapticTypes.SoftImpact, false, true, this);
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

        }

    }

    public void MediumHapticCalled()
    {
        if (PlayerPrefs.GetInt("Vibrate") == 0)
        {
            // MMVibrationManager.Haptic(HapticTypes.MediumImpact, false, true, this);
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }
    }
    public void HeavyHapticCalled()
    {
        if (PlayerPrefs.GetInt("Vibrate") == 0)
        {
            // MMVibrationManager.Haptic(HapticTypes.HeavyImpact, false, true, this);
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
        }
    }


    public void NotificationSucessHaptic()
    {
        if (PlayerPrefs.GetInt("Vibrate") == 0)
        {
            // MMVibrationManager.Haptic(HapticTypes.Success, false, true, this);
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
        }

    }
}
