using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Spine.Unity;
using Unity.Mathematics;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject clock, fail, complete, winPanel, failPanel, gamePlayScreen, Bg, ratingPopUp, guide, levelPanel, getStarPopup;
    public TextMeshProUGUI levelpanelText;
    public GameObject TeleportPanel;
    public TextMeshProUGUI TeleportText;
    private float timer;
    public GameObject gameWinShopBtn;
    public float timerMax;
    public Sprite nonFillStar;
    public Sprite fillStar;
    public List<GameObject> Stars = new List<GameObject>();
    public GameObject sliderObj;
    public TextMeshProUGUI gameWinTotalScore, gameWinGameScore, gameWinRewardButtonTxt;
    bool isRewardStart = false;
    public TextMeshProUGUI clockText, levelText;
    public Animator coinAnimation;
    public GameObject tapToContinue;
    public GameObject restartBtn;
    public Slider sliderImage;
    public float drawLimit;
    public GameObject starImage3, starImage2, starImage1;
    public bool isCollideWithGirl;
    public bool isCollideWithBee;
    public ParticleSystem gameWinPartical;
    public ParticleSystem winPanelParticle;

    public bool startClock;
    public bool isClick = false;
    public int rewardCoin;
    public GameObject monObj = null;

    public GameObject[] gameWinStars;
    public List<GameObject> bees = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        startClock = false;
        timer = timerMax;
    }

    // Start is called before the first frame update
    void Start()
    {
        bees.Clear();
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            AudioManager.instance.musicSource.volume = 1f;
        }
        else
        {
            AudioManager.instance.musicSource.volume = 0f;
        }
        levelPanel.SetActive(false);
        isRewardStart = false;
        if (GameController.instance.currentState == GameController.STATE.PLAYING)
        {
            gamePlayScreen.SetActive(true);
        }
        for (int i = 0; i < gameWinStars.Length; i++)
        {
            gameWinStars[i].SetActive(false);
        }
        sliderImage.value = 1;
        levelText.text = "LEVEL " + (GameController.instance.GetCurrentLevelIndex()).ToString();
        if (GameController.instance.GetCurrentLevelIndex() == 1)
        {
            if (DataManager.Instance.levelMode == null)
            {
                TeleportPanel.SetActive(false);
            }
            else
            {
                TeleportPanel.SetActive(true);
                if (DataManager.Instance.LoveMode)
                {
                    TeleportText.text = "Lets Meet The Dogs!";
                }
                else if (DataManager.Instance.MonsterMode)
                {
                    TeleportText.text = "Kill The Evil Dog!";
                }
                else if (DataManager.Instance.LaserMode)
                {
                    TeleportText.text = "Protect The Dog From Laser!";
                }
                else if (DataManager.Instance.TeleportMode)
                {
                    TeleportText.text = "Bees Will Teleport From Portal!";
                }
            }
        }
    }

    int lastTimer = 6;
    int currentTimer = 0;
    bool gameOverStart = false;

    // Update is called once per frame
    void Update()
    {
        if (isRewardStart)
        {
            //gameWinRewardButtonTxt.text = "X" + SliderScript.Instance.sliderInt;
        }
        if (startClock)
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                clockText.text = Mathf.CeilToInt(timer).ToString();
                currentTimer = Mathf.CeilToInt(timer);
            }
            else
            {
                startClock = false;
                clock.SetActive(false);
                ShowResult();
            }
            if (timer < 6)
            {
                if (currentTimer <= lastTimer && currentTimer != lastTimer)
                {
                    AudioManager.instance.clockAudio.Play();
                    lastTimer = currentTimer;
                }
                clock.GetComponent<Animator>().SetBool("Play", true);
            }

            if (GameController.instance.currentState == GameController.STATE.GAMEOVER && !gameOverStart)
            {
                gameOverStart = true;
                BeeSound();
                if (PlayerPrefs.GetInt("Music") == 0)
                {
                    AudioManager.instance.musicSource.volume = 0.2f;
                }
                else
                {
                    AudioManager.instance.musicSource.volume = 0f;
                }
                startClock = false;
                AudioManager.instance.failAudio.Play();
                clock.SetActive(false);
                StartCoroutine(ShowGameOverIE());
            }
        }
    }
    public Image ClockImg;
    public IEnumerator ClockTimeLimit()
    {

        float turntimer = 10;

        float fillamount = 1;

        while (turntimer > 0)
        {
            turntimer -= Time.deltaTime;

            fillamount = Mathf.InverseLerp(0, 10, turntimer);
            ClockImg.fillAmount = fillamount;
            yield return new WaitForEndOfFrame();
        }
    }
    public void ActiveClock()
    {
        clock.SetActive(true);
        startClock = true;
        StartCoroutine(ClockTimeLimit());
    }

    Coroutine gameWin;
    void ShowResult()
    {
        Debug.LogError("Level Mode : " + Level.Instance.levelMode);
        if (Level.Instance.levelMode == "LoveMode")
        {
            if (GameController.instance.currentState == GameController.STATE.GAMEOVER || !isCollideWithGirl)
            {
                StartGameOverCoroutine();
            }
            else
            {
                StartGameWinCoroutine();
            }
        }
        else if (Level.Instance.levelMode == "MonsterMode")
        {
            for (int i = 0; i < Level.Instance.monsters.Count; i++)
            {
                if (!Level.Instance.monsters[i].ishurt)
                {
                    monObj = Level.Instance.monsters[i].gameObject;
                }
            }
            if (monObj != null)
            {
                monObj.GetComponent<DogController>().mAnimator.AnimationName = "6-attack";
                monObj.GetComponent<CircleCollider2D>().isTrigger = true;
                monObj.transform.DOMove(Level.Instance.dogList[0].position, 3f).OnComplete(StartGameOverCoroutine);
            }
            else
            {
                StartGameWinCoroutine();
            }
        }
        else if (Level.Instance.levelMode == "LaserMode")
        {
            if (GameController.instance.currentState == GameController.STATE.GAMEOVER)
            {
                StartGameOverCoroutine();
            }
            else
            {
                StartGameWinCoroutine();
            }
        }
        else if (Level.Instance.levelMode == "TeleportMode")
        {
            if (GameController.instance.currentState == GameController.STATE.GAMEOVER)
            {
                StartGameOverCoroutine();
            }
            else
            {
                StartGameWinCoroutine();
            }
        }
        else if (Level.Instance.levelMode == "null")
        {
            Debug.LogError("This Is Calles");
            if (GameController.instance.currentState == GameController.STATE.GAMEOVER)
            {
                StartGameOverCoroutine();
            }
            else
            {
                StartGameWinCoroutine();
            }
        }

    }

    public void StartGameOverCoroutine()
    {
        BeeSound();
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            AudioManager.instance.musicSource.volume = 0.2f;
        }
        else
        {
            AudioManager.instance.musicSource.volume = 0f;
        }
        AudioManager.instance.failAudio.Play();

        StartCoroutine(ShowGameOverIE());
    }

    public void StartGameWinCoroutine()
    {
        BeeSound();
        gameWinPartical.Play();
        if (DataManager.Instance.levelMode == null)
        {
            for (int i = 0; i < Level.Instance.dogList.Count; i++)
            {
                if (Level.Instance.dogList[i].gameObject.tag == "Dog")
                {
                    Level.Instance.dogList[i].gameObject.GetComponent<SkeletonAnimation>().AnimationName = "5-happy";
                }
            }
        }
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            AudioManager.instance.musicSource.volume = 0.2f;
        }
        else
        {
            AudioManager.instance.musicSource.volume = 0f;
        }
        AudioManager.instance.winAudio.Play();
        GameController.instance.currentState = GameController.STATE.FINISH;
        gameWin = StartCoroutine(ShowGameWinIE());

        //Debug.Log("Rating :0 " + GameController.instance.Rating);
    }

    public void ShowRatingPopup()
    {
        //ratingPopUp.SetActive(true);
    }

    public void CloseRatingPopup()
    {
        //SuperStarSdkManager.Instance.Rate();
        //ratingPopUp.SetActive(false);
    }

    public void GoBackToHome()
    {
        SceneManager.LoadScene("Home");
    }

    public GameObject FailDog;
    public GameObject FailLoveDog;

    IEnumerator ShowGameOverIE()
    {
        fail.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        failPanel.SetActive(true);
        if (HomeManager.Instance.LoveMode == true)
        {
            FailDog.SetActive(false);
            FailLoveDog.SetActive(true);
        }
        if (HomeManager.Instance.LoveMode == false)
        {
            FailDog.SetActive(true);
            FailLoveDog.SetActive(false);
        }
        gamePlayScreen.SetActive(false);
        yield return new WaitForSeconds(1f);
        if (PlayerPrefs.GetInt("NoAds") == 0)
        {
            //if (GameController.instance.levelIndex > SuperStarSdkManager.Instance.crossPromoAssetsRoot.ad_Start_Level)
            //{
            //    SuperStarAd.Instance.ShowInterstitialTimer((o) => { });
            //    //SuperStarAd.Instance.ShowBannerAd();
            //}
        }
        //SSEventManager.Instance.SSGameOverEventCall(DataManager.Instance.levelMode + (PlayerPrefs.GetInt("UnlockLevel")));
    }
    public string levelMode;
    public GameObject windog;
    public GameObject winlovedog;
    public int score;
    IEnumerator ShowGameWinIE()
    {
        yield return new WaitForSeconds(1.0f);
        int level = GameController.instance.levelIndex;
        SaveLevelByMode();
        score = PlayerPrefs.GetInt("Coin", 0);
        gameWinTotalScore.text = score.ToString();
        PlayerPrefs.SetInt("Coin", score);
        tapToContinue.SetActive(false);
        restartBtn.SetActive(false);
        complete.SetActive(true);
        gamePlayScreen.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        winPanelParticle.Play();
        gameWinShopBtn.SetActive(true);
        winPanel.SetActive(true);

        if (GameController.instance.levelIndex == 3)
        {
            //SuperStarSdkManager.Instance.RatePopUpScreen.SetActive(true);
        }
        if (HomeManager.Instance.LoveMode == true)
        {
            windog.SetActive(false);
            winlovedog.SetActive(true);
        }
        if (HomeManager.Instance.LoveMode == false)
        {
            windog.SetActive(true);
            winlovedog.SetActive(false);
        }
        gamePlayScreen.SetActive(false);
        AudioManager.instance.peopleAudio.Play();
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(StarCollect());
        yield return new WaitForSeconds(0.5f);
        isRewardStart = true;
        yield return new WaitForSeconds(1.5f);
        tapToContinue.SetActive(true);
        restartBtn.SetActive(true);
        if (DataManager.Instance.levelMode == null)
        {
            Debug.LogError("Rate Popup Open");
            if (GameController.instance.levelIndex == 3)
            {
                //SuperStarSdkManager.Instance.Rate();
            }
        }

        //SSEventManager.Instance.SSGameWinEventCall(DataManager.Instance.levelMode + (level - 1));
    }

    IEnumerator StarCollect()
    {
        for (int i = 0; i < starCount; i++)
        {
            AudioManager.instance.starAudio.Play();
            gameWinStars[i].SetActive(true);
            HapticManager.Instance.SoftHapticCalled();
            yield return new WaitForSeconds(0.9f);
            gameWinStars[i].GetComponent<Animator>().SetBool("Idle", true);
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void BeeSound()
    {
        for (int i = 0; i < Level.Instance.bees.Count; i++)
        {
            Level.Instance.bees[i].GetComponent<BeeController>().beeSound.volume = 0f;
        }
    }
    public int starCount = 0;
    private void SaveLevelByMode()
    {
        if (DataManager.Instance.LoveMode)
        {
            int level = GameController.instance.levelIndex;
            int maxLevel = PlayerPrefs.GetInt("LoveUnlockLevel", 1);
            if (level >= maxLevel)
            {
                PlayerPrefs.SetInt("LoveUnlockLevel", level + 1);
            }
            if (drawLimit <= 0.33f)
            {
                starCount = 1;
                rewardCoin = 10;
                gameWinGameScore.text = "x10";
                PlayerPrefs.SetInt((level) + "LoveStars", 1);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i==0)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
            else if (drawLimit <= 0.66f)
            {
                starCount = 2;
                rewardCoin = 20;
                gameWinGameScore.text = "x20";
                PlayerPrefs.SetInt((level) + "LoveStars", 2);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i <= 1)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
            else if (drawLimit >= 0.66f)
            {
                starCount = 3;
                rewardCoin = 30;
                gameWinGameScore.text = "x30";
                PlayerPrefs.SetInt((level) + "LoveStars", 3);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i <= 2)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
        }
        else if (DataManager.Instance.MonsterMode)
        {
            int level = GameController.instance.levelIndex;
            int maxLevel = PlayerPrefs.GetInt("MonsterUnlockLevel", 1);
            if (level >= maxLevel)
            {
                PlayerPrefs.SetInt("MonsterUnlockLevel", level + 1);
            }
            if (drawLimit <= 0.33f)
            {

                starCount = 1;
                rewardCoin = 10;
                gameWinGameScore.text = "x10";
                PlayerPrefs.SetInt((level) + "MonsterStars", 1);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i <= 0)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
            else if (drawLimit <= 0.66f)
            {
                starCount = 2;
                rewardCoin = 20;
                gameWinGameScore.text = "x20";
                PlayerPrefs.SetInt((level) + "MonsterStars", 2);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i <= 1)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
            else if (drawLimit >= 0.66f)
            {
                starCount = 3;
                rewardCoin = 30;
                gameWinGameScore.text = "x30";
                PlayerPrefs.SetInt((level) + "MonsterStars", 3);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i <= 2)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
        }
        else if (DataManager.Instance.LaserMode)
        {
            int level = GameController.instance.levelIndex;
            int maxLevel = PlayerPrefs.GetInt("LaserUnlockLevel", 1);
            if (level >= maxLevel)
            {
                PlayerPrefs.SetInt("LaserUnlockLevel", level + 1);
            }
            if (drawLimit <= 0.33f)
            {
                starCount = 1;
                rewardCoin = 10;
                gameWinGameScore.text = "x10";
                PlayerPrefs.SetInt((level) + "LaserStars", 1);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i <= 0)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
            else if (drawLimit <= 0.66f)
            {
                starCount = 2;
                rewardCoin = 20;
                gameWinGameScore.text = "x20";
                PlayerPrefs.SetInt((level) + "LaserStars", 2);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i <= 1)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
            else if (drawLimit >= 0.66f)
            {
                starCount = 3;
                rewardCoin = 30;
                gameWinGameScore.text = "x30";
                PlayerPrefs.SetInt((level) + "LaserStars", 3);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i <= 2)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
        }
        else if (DataManager.Instance.TeleportMode)
        {
            int level = GameController.instance.levelIndex;
            int maxLevel = PlayerPrefs.GetInt("TeleUnlockLevel", 1);
            if (level >= maxLevel)
            {
                PlayerPrefs.SetInt("TeleUnlockLevel", level + 1);
            }
            if (drawLimit <= 0.33f)
            {
                starCount = 1;
                rewardCoin = 10;
                gameWinGameScore.text = "x10";
                PlayerPrefs.SetInt((level) + "TeleStars", 1);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i <= 0)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
            else if (drawLimit <= 0.66f)
            {
                starCount = 2;
                rewardCoin = 20;
                gameWinGameScore.text = "x20";
                PlayerPrefs.SetInt((level) + "TeleStars", 2);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i <= 1)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
            else if (drawLimit >= 0.66f)
            {
                starCount = 3;
                rewardCoin = 30;
                gameWinGameScore.text = "x30";
                PlayerPrefs.SetInt((level) + "TeleStars", 3);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i <= 2)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
        }
        else
        {
            int level = GameController.instance.levelIndex;
            int maxLevel = PlayerPrefs.GetInt("UnlockLevel", 1);
            if (level >= maxLevel)
            {
                PlayerPrefs.SetInt("UnlockLevel", level + 1);
            }
            if (drawLimit <= 0.33f)
            {
                starCount = 1;
                rewardCoin = 10;
                gameWinGameScore.text = "x10";
                PlayerPrefs.SetInt((level) + "Stars", 1);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i <= 0)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
            else if (drawLimit <= 0.66f)
            {
                starCount = 2;
                rewardCoin = 20;
                gameWinGameScore.text = "x20";
                PlayerPrefs.SetInt((level) + "Stars", 2);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i <= 1)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
            else if (drawLimit >= 0.66f)
            {
                starCount = 3;
                rewardCoin = 30;
                gameWinGameScore.text = "x30";
                PlayerPrefs.SetInt((level) + "Stars", 3);
                //for (int i = 0; i < gameWinStars.Length; i++)
                //{
                //    if (i <= 2)
                //    {
                //        gameWinStars[i].SetActive(true);
                //    }
                //    else
                //    {
                //        gameWinStars[i].SetActive(false);
                //    }
                //}
            }
            if (maxLevel >= 20)
            {
                PlayerPrefs.SetInt("LoveMode", 1);
            }
            if (maxLevel >= 40)
            {
                PlayerPrefs.SetInt("MonsterMode", 1);
            }
            if (maxLevel >= 60)
            {
                PlayerPrefs.SetInt("LaserMode", 1);
            }
            if (maxLevel >= 80)
            {
                PlayerPrefs.SetInt("TeleportMode", 1);
            }
        }
    }

    int starIndx;
    public IEnumerator OnClickRating()
    {
        yield return new WaitForSeconds(0.8f);
        if (starIndx > 2)
        {
            //SuperStarSdkManager.Instance.Rate();
            //ratingPopUp.SetActive(false);
        }
        else
        {
            //NextLevel();
            //ratingPopUp.SetActive(false);
        }
    }

    public void StarButton(int indx)
    {
        starIndx = indx;
        for (int i = 0; i < Stars.Count; i++)
        {
            if (i <= indx)
            {
                Stars[i].GetComponent<Image>().sprite = fillStar;
            }
            else
            {
                Stars[i].GetComponent<Image>().sprite = nonFillStar;
            }
        }
        StartCoroutine(OnClickRating());
    }

    public void Replay()
    {
        AudioManager.instance.buttonAudio.Play();
        PlayerPrefs.SetInt("CurrentLevel", GameController.instance.levelIndex);
        SceneManager.LoadScene("Level");
    }

    public void NextLevel()
    {
        gameWinShopBtn.SetActive(false);
        AudioManager.instance.buttonAudio.Play();
        StartNextCoroutine(true);
    }

    public void StartNextCoroutine(bool isRewarded)
    {
        if (isRewarded)
        {
            StartCoroutine(StopSlider());
        }
        else
        {
            StartCoroutine(ContinueLoadLevel());
        }
    }
    public void TapToContinue()
    {
        gameWinShopBtn.SetActive(false);
        AudioManager.instance.buttonAudio.Play();
        if (PlayerPrefs.GetInt("NoAds") == 0)
        {
            Debug.LogError("Ad Shown");
            //if (GameController.instance.levelIndex > SuperStarSdkManager.Instance.crossPromoAssetsRoot.ad_Start_Level)
            //{
            //    SuperStarAd.Instance.ShowInterstitialTimer((o) => { });
            //    // SuperStarAd.Instance.ShowBannerAd();
            //}
        }
        StartCoroutine(ContinueLoadLevel());
    }
    IEnumerator StopSlider()
    {
        StopCoroutine(gameWin);
        //sliderObj.GetComponent<Animator>().enabled = false;
        int gameScore;
        if (drawLimit <= 0.25f)
        {
            gameScore = 10 * 2;
        }
        else if (drawLimit <= 0.5f)
        {
            gameScore = 20 * 2;
        }
        else
        {
            gameScore = 30 * 2;
        }

        gameWinGameScore.text = gameScore.ToString();
        yield return new WaitForSeconds(1f);
        AudioManager.instance.coinCollectAudio.Play();
        coinAnimation.SetBool("Play", true);
        HapticManager.Instance.HeavyHapticCalled();
        yield return new WaitForSeconds(1.5f);
        int totalScore = gameScore + PlayerPrefs.GetInt("Coin", 0);
        gameWinTotalScore.text = totalScore.ToString();
        PlayerPrefs.SetInt("Coin", totalScore);
        yield return new WaitForSeconds(1f);
        LoadNewLevel();
    }

    IEnumerator ContinueLoadLevel()
    {
        int gameScore = rewardCoin;
        gameWinGameScore.text = gameScore.ToString();
        yield return new WaitForSeconds(1f);
        AudioManager.instance.coinCollectAudio.Play();
        coinAnimation.SetBool("Play", true);
        yield return new WaitForSeconds(1.5f);
        int totalScore = gameScore + PlayerPrefs.GetInt("Coin", 0);
        gameWinTotalScore.text = totalScore.ToString();
        PlayerPrefs.SetInt("Coin", totalScore);
        yield return new WaitForSeconds(1f);
        LoadNewLevel();
    }

    public void WinRestarnBtn()
    {
        //SuperStarAd.Instance.ShowInterstitialTimer((o) =>
        //{

            gameWinShopBtn.SetActive(false);
            StartCoroutine(RestartLevel());
        //});

    }

    IEnumerator RestartLevel()
    {
        int gameScore = rewardCoin;
        gameWinGameScore.text = gameScore.ToString();
        yield return new WaitForSeconds(1f);
        AudioManager.instance.coinCollectAudio.Play();
        coinAnimation.SetBool("Play", true);
        yield return new WaitForSeconds(1.5f);
        int totalScore = gameScore + PlayerPrefs.GetInt("Coin", 0);
        gameWinTotalScore.text = totalScore.ToString();
        PlayerPrefs.SetInt("Coin", totalScore);
        yield return new WaitForSeconds(1f);
        PlayerPrefs.SetInt("CurrentLevel", GameController.instance.levelIndex);
        SceneManager.LoadScene("Level");
    }

    public void LoadNewLevel()
    {
        int num = GameController.instance.levelIndex;
        SetCurrentLevelIndexByMode(num + 1);
        SceneManager.LoadScene("Level");
    }
    private void SetCurrentLevelIndexByMode(int num)
    {
        if (DataManager.Instance.LoveMode)
        {
            PlayerPrefs.SetInt("LoveCurrentLevel", num);
        }
        else if (DataManager.Instance.MonsterMode)
        {
            PlayerPrefs.SetInt("MonsterCurrentLevel", num);
        }
        else if (DataManager.Instance.LaserMode)
        {
            PlayerPrefs.SetInt("LaserCurrentLevel", num);
        }
        else if (DataManager.Instance.TeleportMode)
        {
            PlayerPrefs.SetInt("TeleCurrentLevel", num);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", num);
        }
    }

    public void Home()
    {
        AudioManager.instance.buttonAudio.Play();
        if (PlayerPrefs.GetInt("NoAds") == 0)
        {
            Debug.LogError("Is Called on Home Button Ads");
            //SuperStarAd.Instance.ShowForceInterstitialWithLoader(null, 3);
            LoadHomePanel();
        }
        else
        {
            LoadHomePanel();
        }
    }

    public void LoadHome()
    {
        SceneManager.LoadScene("Home");
    }
    private void LoadHomePanel()
    {
        if (DataManager.Instance.LoveMode)
        {
            levelPanel.SetActive(true);
            levelpanelText.text = "LOVE MODE";
        }
        else if (DataManager.Instance.MonsterMode)
        {
            levelPanel.SetActive(true);
            levelpanelText.text = "MONSTER MODE";
        }
        else if (DataManager.Instance.LaserMode)
        {
            levelPanel.SetActive(true);
            levelpanelText.text = "LASER MODE";
        }
        else if (DataManager.Instance.TeleportMode)
        {
            levelPanel.SetActive(true);
            levelpanelText.text = "TELEPORT MODE";
        }
        else
        {
            SceneManager.LoadScene("Home");
        }
    }

    public void Hint()
    {
        AudioManager.instance.buttonAudio.Play();
        isAdplaying = true;
        ExampleShowRewardAssign(true);
        //  guide.SetActive(true);
    }
    public GameObject shopPopup;
    public void OnClickWinShop()
    {
        AudioManager.instance.buttonAudio.Play();
        shopPopup.SetActive(true);
    }

    public Material Linemat;
    public Gradient Linecolor;
    public bool isAdplaying = false;
    public void ExampleShowRewardAssign(bool isrewarded)
    {
        if (isrewarded)
        {
            //Give reward here
            Debug.Log("Reward Given");
            guide.GetComponent<LineRenderer>().material = Linemat;
            guide.GetComponent<LineRenderer>().colorGradient = Linecolor;
            guide.SetActive(true);
            Invoke("InvokeIsPlaying", 1f);
        }
        else
        {
            Debug.Log("Reward Eroor Given");
            Invoke("InvokeIsPlaying", 0f);
        }

    }


    public void InvokeIsPlaying()
    {

        isAdplaying = false;
    }

    public void TelePortPanelCloseBtn()
    {
        TeleportPanel.SetActive(false);
    }
}
