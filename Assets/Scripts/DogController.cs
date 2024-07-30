using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    public Rigidbody2D rb;
    public SkeletonAnimation mAnimator;
    public GameObject deathVfx;
    public GameObject fireVfx;
    public GameObject waterVfx;
    public static DogController Instance;
    public bool ishurt;
    public bool isMonster;
    public Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
       
    }

    public void Hurt()
    {
        if (GameController.instance.currentState != GameController.STATE.GAMEOVER)
        {
            HapticManager.Instance.MediumHapticCalled();
            AudioManager.instance.dogAudio.Play();
        }
        ishurt = true;
        mAnimator.AnimationName = "4-sting";
        GameController.instance.currentState = GameController.STATE.GAMEOVER;
    }
    public void MonsterHurt()
    {
        if (mAnimator.AnimationName != "4-sting")
        {
            HapticManager.Instance.MediumHapticCalled();
            AudioManager.instance.dogAudio.Play();
        }
        ishurt = true;
        //animator.SetBool("Hurt", true);
        mAnimator.AnimationName = "4-sting";
    }

    public void Hurtdestroy()
    {
        GameController.instance.currentState = GameController.STATE.GAMEOVER;
        Instantiate(deathVfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Lava" || collision.gameObject.tag == "Water" || collision.gameObject.tag == "Spike")
        {
            GameController.instance.currentState = GameController.STATE.GAMEOVER;
            if (collision.gameObject.tag == "Lava")
            {
                HapticManager.Instance.MediumHapticCalled();
                AudioManager.instance.burnAudio.Play();
                Instantiate(deathVfx, transform.position, Quaternion.identity);
            }
            else if (collision.gameObject.tag == "Spike")
            {
                HapticManager.Instance.MediumHapticCalled();
                AudioManager.instance.dogAudio.Play();
                Instantiate(deathVfx, transform.position, Quaternion.identity);
            }
            else if(collision.gameObject.tag == "Water")
            {
                HapticManager.Instance.MediumHapticCalled();
                AudioManager.instance.bigWaterAudio.Play();
                Instantiate(deathVfx, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }

        if (Level.Instance.LoveMode)
        {
            if (collision.gameObject.tag == "LDog")
            {
                UIManager.Instance.isCollideWithGirl = true;
                Level.Instance.StartLoveAnim();
            }
        }
        

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (isMonster)
        {
            if (col.gameObject.tag == "Dog")
            {
                Debug.LogError("Is Collide with Monster");
                HapticManager.Instance.HeavyHapticCalled();
                col.gameObject.GetComponent<DogController>().Hurt();
                UIManager.Instance.StartGameOverCoroutine();
                DOTween.KillAll();
            }
        }
        if (Level.Instance.laserMode)
        {
            if (col.gameObject.tag == "Laser")
            {
                HapticManager.Instance.MediumHapticCalled();
                GameController.instance.currentState = GameController.STATE.GAMEOVER;
                Instantiate(deathVfx, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }


    }

}
