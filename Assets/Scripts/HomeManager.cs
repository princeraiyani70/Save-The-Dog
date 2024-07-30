using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HomeManager : MonoBehaviour
{
    public GameObject levelPanel, settingPanel, noAds;
    public static HomeManager Instance;
    public TextMeshProUGUI scoreTxt;
    public string levelMode;


    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        
        if (PlayerPrefs.GetInt("NoAds") == 1)
        {
            noAds.SetActive(false);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            AudioManager.instance.musicSource.volume = 1f;
        }
        else
        {
            AudioManager.instance.musicSource.volume = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        DataManager.Instance.levelMode = null;
        DataManager.Instance.LoveMode = false;
        DataManager.Instance.MonsterMode = false;
        DataManager.Instance.LaserMode = false;
        DataManager.Instance.TeleportMode = false;
        AudioManager.instance.buttonAudio.Play();
        int unlockLevel = PlayerPrefs.GetInt("UnlockLevel", 1);
        PlayerPrefs.SetInt("CurrentLevel", unlockLevel);
        //SuperStarAd.Instance.ShowRewardVideo();
        SceneManager.LoadScene("Level");
        Debug.Log("ADD Added !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //SSEventManager.Instance.SSGameStarEventCall(unlockLevel);
    }

    public bool LoveMode = false;
    public void LoveModePlay()
    {
        DataManager.Instance.levelMode = "LoveMode";
        DataManager.Instance.LoveMode = true;
        DataManager.Instance.MonsterMode = false;
        DataManager.Instance.LaserMode = false;
        DataManager.Instance.TeleportMode = false;
        AudioManager.instance.buttonAudio.Play();
        if (PlayerPrefs.GetInt("LoveMode", 0) == 0)
        {
            ModsPanel.Instance.popupPanel.SetActive(true);
            ModsPanel.Instance.LogoTxt.text = "LOVE MODE";
            ModsPanel.Instance.SetLogo(1);
            ModsPanel.Instance.LevelText.text = "Automatically Unlock After Complete\n20 Level in main mode, or unlock\nimmediately after watching ad!";
        }
        else
        {
            int unlockLevel = PlayerPrefs.GetInt("LoveUnlockLevel", 1);
            PlayerPrefs.SetInt("LoveCurrentLevel", unlockLevel);
            SceneManager.LoadScene("Level");
            //SSEventManager.Instance.SSGameStarEventCall("LoveMode"+unlockLevel);
        }
    }
    public void LoveModeIsEnable(bool isRewarded)
    {
        if (isRewarded)
        {
            PlayerPrefs.SetInt("LoveMode", 1);
            ModsPanel.Instance.popupPanel.SetActive(false);
            LoveModePlay();
            LoveMode = true;
        }
        else
        {
            PlayerPrefs.SetInt("LoveMode", 0);
            ModsPanel.Instance.popupPanel.SetActive(false);
            SceneManager.LoadScene("Home");
        }
    }
    public void MonsterModePlay()
    {
        DataManager.Instance.levelMode = "MonsterMode";
        DataManager.Instance.LoveMode = false;
        DataManager.Instance.MonsterMode = true;
        DataManager.Instance.LaserMode = false;
        DataManager.Instance.TeleportMode = false;
        AudioManager.instance.buttonAudio.Play();
        if (PlayerPrefs.GetInt("MonsterMode", 0) == 0)
        {
            ModsPanel.Instance.popupPanel.SetActive(true);
            ModsPanel.Instance.LogoTxt.text = "MONSTER MODE";
            ModsPanel.Instance.SetLogo(2);
            ModsPanel.Instance.LevelText.text = "Automatically Unlock After Complete\n40 Level in main mode, or unlock\nimmediately after watching ad!";

        }
        else
        {
            int unlockLevel = PlayerPrefs.GetInt("MonsterUnlockLevel", 1);
            PlayerPrefs.SetInt("MonsterCurrentLevel", unlockLevel);
            SceneManager.LoadScene("Level");
            //SSEventManager.Instance.SSGameStarEventCall("MonsterMode" + unlockLevel);
        }
    }
    public void MonsterModeIsEnable(bool isRewarded)
    {
        if (isRewarded)
        {
            PlayerPrefs.SetInt("MonsterMode", 1);
            ModsPanel.Instance.popupPanel.SetActive(false);
            MonsterModePlay();
        }
        else
        {
            PlayerPrefs.SetInt("MonsterMode", 0);
            ModsPanel.Instance.popupPanel.SetActive(false);
            SceneManager.LoadScene("Home");
        }
    }
    public void LaserModePlay()
    {
        DataManager.Instance.levelMode = "LaserMode";
        DataManager.Instance.LoveMode = false;
        DataManager.Instance.MonsterMode = false;
        DataManager.Instance.LaserMode = true;
        DataManager.Instance.TeleportMode = false;
        AudioManager.instance.buttonAudio.Play();
        if (PlayerPrefs.GetInt("LaserMode", 0) == 0)
        {
            ModsPanel.Instance.popupPanel.SetActive(true);
            ModsPanel.Instance.LogoTxt.text = "LASER MODE";
            ModsPanel.Instance.SetLogo(3);
            ModsPanel.Instance.LevelText.text = "Automatically Unlock After Complete\n60 Level in main mode, or unlock\nimmediately after watching ad!";

        }
        else
        {
            int unlockLevel = PlayerPrefs.GetInt("LaserUnlockLevel", 1);
            PlayerPrefs.SetInt("LaserCurrentLevel", unlockLevel);
            SceneManager.LoadScene("Level");
            //SSEventManager.Instance.SSGameStarEventCall("LaserMode" + unlockLevel);
        }
    }
    public void LaserModeIsEnable(bool isRewarded)
    {
        if (isRewarded)
        {
            PlayerPrefs.SetInt("LaserMode", 1);
            ModsPanel.Instance.popupPanel.SetActive(false);
            LaserModePlay();
        }
        else
        {
            PlayerPrefs.SetInt("LaserMode", 0);
            ModsPanel.Instance.popupPanel.SetActive(false);
            SceneManager.LoadScene("Home");
        }
    }
    public void TeleportModePlay()
    {
        DataManager.Instance.levelMode = "TeleportMode";
        DataManager.Instance.LoveMode = false;
        DataManager.Instance.MonsterMode = false;
        DataManager.Instance.LaserMode = false;
        DataManager.Instance.TeleportMode = true;
        AudioManager.instance.buttonAudio.Play();
        if (PlayerPrefs.GetInt("TeleportMode", 0) == 0)
        {
            ModsPanel.Instance.popupPanel.SetActive(true);
            ModsPanel.Instance.LogoTxt.text = "TELEPORT MODE";
            ModsPanel.Instance.SetLogo(4);
            ModsPanel.Instance.LevelText.text = "Automatically Unlock After Complete\n80 Level in main mode, or unlock\nimmediately after watching ad!";

        }
        else
        {
            int unlockLevel = PlayerPrefs.GetInt("TeleUnlockLevel", 1);
            PlayerPrefs.GetInt("TeleCurrentLevel", unlockLevel);
            SceneManager.LoadScene("Level");
            //SSEventManager.Instance.SSGameStarEventCall("TeleportMode" + unlockLevel);
        }
    }
    public void TeleModeIsEnable(bool isRewarded)
    {
        if (isRewarded)
        {
            PlayerPrefs.SetInt("TeleportMode", 1);
            ModsPanel.Instance.popupPanel.SetActive(false);
            TeleportModePlay();
        }
        else
        {
            PlayerPrefs.SetInt("TeleportMode", 0);
            ModsPanel.Instance.popupPanel.SetActive(false);
            SceneManager.LoadScene("Home");
        }
    }

    public void OnClickShop()
    {
        AudioManager.instance.buttonAudio.Play();
        ShopPanel.Instance.popupPanel.SetActive(true);
        ShopPanel.Instance.SetPanel();
    }
    public void OnClickNoAds()
    {
        AudioManager.instance.buttonAudio.Play();
        //InaapManager.Instance.PurchaseNoAdsProuduct();
    }
    public void ShowMoreApps() 
    {

#if UNITY_ANDROID
        //Application.OpenURL(SuperStarSdkManager.Instance.crossPromoAssetsRoot.moreappurl);
#elif UNITY_IOS
        Application.OpenURL(SuperStarSdkManager.Instance.crossPromoAssetsRoot.moreappurl);

#endif
    }

    public void ShowLevelSelector()
    {
        AudioManager.instance.buttonAudio.Play();
        levelPanel.SetActive(true);
    }

    public void ShowSetting()
    {
        AudioManager.instance.buttonAudio.Play();
        settingPanel.SetActive(true);
    }

    public void CloseSetting()
    {
        AudioManager.instance.buttonAudio.Play();
        settingPanel.SetActive(false);
    }

    public void CloseLevelSelector()
    {
        AudioManager.instance.buttonAudio.Play();
        levelPanel.SetActive(false);
    }
}
