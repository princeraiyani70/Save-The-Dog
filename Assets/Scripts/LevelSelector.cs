using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public int currentPage;

    public int maxPage;

    public int maxLevel;

    public List<LevelItem> itemList;

    // Start is called before the first frame update
    void Start()
    {
        currentPage = 0;
        RefreshItem();
    }

    private void OnEnable()
    {
        currentPage = 0;
        RefreshItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RefreshItem()
    {
        
        int unlockLevel = GetUnlockLevelIndex();
        for (int i = 0; i < itemList.Count; i++)
        {
            if ((6 * currentPage + i) < maxLevel)
            {
                if ((6 * currentPage + i) < unlockLevel)
                    itemList[i].RefreshItem(currentPage, true);
                else
                    itemList[i].RefreshItem(currentPage, false);
            }
            else
            {
                itemList[i].gameObject.SetActive(false);
            }
        }
    }

    private int GetUnlockLevelIndex()
    {
        if (DataManager.Instance.LoveMode)
        {
            Debug.LogError("Love Moode Level UnlockIndex");
            return PlayerPrefs.GetInt("LoveUnlockLevel", 1);
        }
        else if(DataManager.Instance.MonsterMode)
        {
            Debug.LogError("monster Moode Level UnlockIndex");
            return PlayerPrefs.GetInt("MonsterUnlockLevel", 1);
        }
        else if (DataManager.Instance.LaserMode)
        {
            Debug.LogError("Laser Moode Level UnlockIndex");
            return PlayerPrefs.GetInt("LaserUnlockLevel", 1);
        }
        else if (DataManager.Instance.TeleportMode)
        {
            Debug.LogError("Teleport Moode Level UnlockIndex");
            return PlayerPrefs.GetInt("TeleUnlockLevel", 1);
        }
        else
        {
            Debug.LogError("Normal Moode Level UnlockIndex");
            return PlayerPrefs.GetInt("UnlockLevel", 1);
        }
    }

    public void NextPage()
    {
        AudioManager.instance.buttonAudio.Play();
        if (currentPage < maxPage - 1)
          currentPage++;
        RefreshItem();
    }

    public void PreviousPage()
    {
        AudioManager.instance.buttonAudio.Play();
        if (currentPage > 0)
            currentPage--;
        RefreshItem();
    }

    public void GoToLevel(int index)
    {
        int levelIndex = 6 * currentPage + index;
        AudioManager.instance.buttonAudio.Play();
        SetCurrentLevel(levelIndex + 1);
        SceneManager.LoadScene("Level");
    }

    private void SetCurrentLevel(int v)
    {
        if (DataManager.Instance.LoveMode)
        {
            DataManager.Instance.levelMode = "LoveMode";
             PlayerPrefs.SetInt("LoveCurrentLevel", v);
        }
        else if (DataManager.Instance.MonsterMode)
        {
            DataManager.Instance.levelMode = "MonsterMode";
            PlayerPrefs.SetInt("MonsterCurrentLevel", v);
        }
        else if (DataManager.Instance.LaserMode)
        {
            DataManager.Instance.levelMode = "LaserMode";
            PlayerPrefs.SetInt("LaserCurrentLevel", v);
        }
        else if (DataManager.Instance.TeleportMode)
        {
            DataManager.Instance.levelMode = "TeleportMode";
            PlayerPrefs.SetInt("TeleCurrentLevel", v);
        }
        else
        {
            DataManager.Instance.levelMode = null;
            PlayerPrefs.SetInt("CurrentLevel", v);
        }
    }
}
