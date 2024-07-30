using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Spine.Unity;
using System.Security.Cryptography;
using UnityEngine.UIElements.Experimental;
using JetBrains.Annotations;

public class GameController : MonoBehaviour
{
    [HideInInspector]
    public Level currentLevel;

    public int levelIndex;

    public int maxLevel;
    public int loveMax;
    public int monsterMax;
    public int laserMax;
    public int teleportMax;

    public static GameController instance;

    public GameObject drawManager;

    public UIManager uiManager;

    public GameObject testLevel;

    public List<GameObject> playerSkin = new List<GameObject>();

    public GameObject dog, lDog, monster;
    public enum STATE
    {
        DRAWING,
        PLAYING,
        FINISH,
        GAMEOVER
    }

    public STATE currentState;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        currentState = STATE.DRAWING;
        levelIndex = GetCurrentLevelIndex();
        CreateLevel();
        Application.targetFrameRate = 60;
    }

    public int GetCurrentLevelIndex()
    {
        if (DataManager.Instance.LoveMode)
        {
            return PlayerPrefs.GetInt("LoveCurrentLevel", 1);
        }
        else if (DataManager.Instance.MonsterMode)
        {
            return PlayerPrefs.GetInt("MonsterCurrentLevel", 1);
        }
        else if (DataManager.Instance.LaserMode)
        {
            return PlayerPrefs.GetInt("LaserCurrentLevel", 1);
        }
        else if (DataManager.Instance.TeleportMode)
        {
            return PlayerPrefs.GetInt("TeleCurrentLevel", 1);
        }
        else
        {
            return PlayerPrefs.GetInt("CurrentLevel", 1);
        }


    }

    public int GetUnlockLevelIndex()
    {
        if (DataManager.Instance.LoveMode)
        {
            return PlayerPrefs.GetInt("LoveUnlockLevel", 1);
        }
        else if (DataManager.Instance.MonsterMode)
        {
            return PlayerPrefs.GetInt("MonsterUnlockLevel", 1);
        }
        else if (DataManager.Instance.LaserMode)
        {
            return PlayerPrefs.GetInt("LaserUnlockLevel", 1);
        }
        else if (DataManager.Instance.TeleportMode)
        {
            return PlayerPrefs.GetInt("TeleUnlockLevel", 1);
        }
        else
        {
            return PlayerPrefs.GetInt("UnlockLevel", 1);
        }
    }
   // public GameObject RatingObj;
  // public bool Rating = true;
   
    private void Update()
    {
        //Debug.Log("Rating :0 " + Rating);
        //if (levelIndex == RetingOnLeval)
        //{
        //    if (Rating == true)
        //    {
        //        RatingObj.SetActive(true);
        //        Debug.Log("Rating :1" + Rating);


        //        RetingOnLeval += 3;
        //    }
        //}
    }


    public void ActivateGame()
    {
        currentState = STATE.PLAYING;
        ActiveDog();
        uiManager.ActiveClock();
    }

    void ActiveDog()
    {
        for (int i = 0; i < currentLevel.dogList.Count; i++)
        {
            if (!currentLevel.dogList[i].CompareTag("Monster"))
            {
                currentLevel.dogList[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
    public List<Transform> doglist = new List<Transform>();
    void CreateLevel()
    {
        GetMaxLevel();
        GameObject levelObj = GetLevelObj();
        //GameObject levelObj = Instantiate(testLevel);

        currentLevel = levelObj.GetComponent<Level>();

        GameObject getSkin = playerSkin[PlayerPrefs.GetInt("PlayerSkin", 0)];
        Level.Instance.monsters.Clear();
        if (!DataManager.Instance.LoveMode)
        {
            for (int i = 0; i < currentLevel.dogList.Count; i++)
            {
                if (currentLevel.dogList[i].gameObject.tag == "Dog")
                {
                    GameObject obj = Instantiate(getSkin, currentLevel.dogList[i].position + new Vector3(0, -0.75f, 0), Quaternion.identity);
                    Destroy(currentLevel.dogList[i].gameObject);
                    currentLevel.dogList[i] = obj.transform;
                }
                else if (currentLevel.dogList[i].gameObject.tag == "Monster")
                {
                    GameObject obj = Instantiate(monster, currentLevel.dogList[i].position + new Vector3(0, -0.75f, 0), Quaternion.identity);
                    Destroy(currentLevel.dogList[i].gameObject);
                    currentLevel.dogList[i] = obj.transform;
                    Level.Instance.monsters.Add(obj.GetComponent<DogController>());
                }
            }
        }
        else
        {
            for (int i = 0; i < currentLevel.dogList.Count; i++)
            {
                if (currentLevel.dogList[i].gameObject.tag == "Dog")
                {
                    GameObject obj = Instantiate(dog, currentLevel.dogList[i].position + new Vector3(0, -0.75f, 0), Quaternion.identity);
                    Destroy(currentLevel.dogList[i].gameObject);
                    currentLevel.dogList[i] = obj.transform;
                }
                else if (currentLevel.dogList[i].gameObject.tag == "LDog")
                {
                    GameObject obj = Instantiate(lDog, currentLevel.dogList[i].position + new Vector3(0, -0.75f, 0), Quaternion.identity);
                    Destroy(currentLevel.dogList[i].gameObject);
                    currentLevel.dogList[i] = obj.transform;
                }
            }
        }
    }

    private void GetMaxLevel()
    {
        if (DataManager.Instance.LoveMode)
        {
            if (levelIndex > loveMax)
            {
                levelIndex = UnityEngine.Random.Range(1, loveMax);
            }
        }
        else if (DataManager.Instance.MonsterMode)
        {
            if (levelIndex > monsterMax)
            {
                levelIndex = UnityEngine.Random.Range(1, monsterMax);
            }
        }
        else if (DataManager.Instance.LaserMode)
        {
            if (levelIndex > laserMax)
            {
                levelIndex = UnityEngine.Random.Range(1, laserMax);
            }
        }
        else if (DataManager.Instance.TeleportMode)
        {
            if (levelIndex > teleportMax)
            {
                levelIndex = UnityEngine.Random.Range(1, teleportMax);
            }
        }
        else
        {
            if (levelIndex > maxLevel)
            {
                levelIndex = UnityEngine.Random.Range(1, maxLevel);
            }
        }
    }


    private GameObject GetLevelObj()
    {
        if (DataManager.Instance.LoveMode)
        {
            return Instantiate(Resources.Load("LoveMode/Level" + (levelIndex).ToString())) as GameObject;
        }
        else if (DataManager.Instance.MonsterMode)
        {
            return Instantiate(Resources.Load("MonsterMode/Level" + (levelIndex).ToString())) as GameObject;
        }
        else if (DataManager.Instance.LaserMode)
        {
            return Instantiate(Resources.Load("LaserMode/Level" + (levelIndex).ToString())) as GameObject;
        }
        else if (DataManager.Instance.TeleportMode)
        {
            return Instantiate(Resources.Load("TeleportMode/Level" + (levelIndex).ToString())) as GameObject;
        }
        else
        {
            return Instantiate(Resources.Load("Levels/Level" + (levelIndex).ToString())) as GameObject;
        }
    }
}
