using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public static Level Instance;
    public int levelIndex;
    public bool isDarkBg = false;
    public GameObject guide;
    public List<Transform> dogList;
    public SpiderControl[] spider;
    public float maxDrawLimit;
    public bool LoveMode;
    public bool monsterMode;
    public bool spiderMode;
    public bool laserMode;
    public bool BombMode;
    public bool TeleportMode;
    public bool QueenMode;
    public bool PathFindMode;
    public string levelMode;

    public Transform teleTargetObj;
    public Transform teleOutObj;
    public List<DogController> monsters = new List<DogController>();
    public GameObject[] laserLineObj;
    public Bomb bombObj;
    public List<GameObject> bees = new List<GameObject>();
    public SpriteRenderer laserSwitch;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        if (spider != null)
        {
            for (int i = 0; i < spider.Length; i++)
            {
                spider[i].enabled = false;
            }
        }
        UIManager.Instance.guide = this.guide;
        if (levelIndex != 0)
        {
            guide.SetActive(false);
        }
        else
        {
            guide.SetActive(true);
        }
        if (isDarkBg)
        {
            UIManager.Instance.Bg.GetComponent<SpriteRenderer>().color = new Color32(85, 85, 85, 255);
        }
        else
        {
            UIManager.Instance.Bg.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(GameController.instance.levelIndex == 0)
            {
                guide.SetActive(false);
            }
        }
        
    }

    public void StartLoveAnim()
    {
        for (int i = 0; i < dogList.Count; i++)
        {
            if (dogList[i].gameObject.tag == "Dog")
            {
                dogList[i].gameObject.GetComponent<DogController>().mAnimator.AnimationName = "6-meet";
            }
            else if (dogList[i].gameObject.tag == "LDog")
            {
                Debug.LogError("Is hear in ldog");
                dogList[i].gameObject.GetComponent<DogController>().mAnimator.AnimationName = "5-happy & meet";
            }
        }
    }


}
