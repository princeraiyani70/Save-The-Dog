using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public string levelMode;

    public bool LoveMode, MonsterMode, LaserMode, TeleportMode;

    private void Awake()
    {
        if (FindObjectsOfType(typeof(DataManager)).Length > 1)
        {
            Debug.LogError("Destroyed");
            Destroy(gameObject);
            return;
        }


        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
