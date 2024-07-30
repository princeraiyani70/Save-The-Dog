using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;

    public AudioSource failAudio;

    public AudioSource winAudio;

    public AudioSource dogAudio;

    public AudioSource buttonAudio;

    public AudioSource breakAudio;

    public AudioSource clockAudio;

    public AudioSource starAudio;

    public AudioSource pencilAudio;

    public AudioSource peopleAudio;

    public AudioSource burnAudio;

    public AudioSource bigWaterAudio;
    
    public AudioSource smallWaterAudio;

    public AudioSource coinCollectAudio;
    //[HideInInspector]
    public int soundState=1,musicState=1;

    private void Awake()
    {

        if (FindObjectsOfType(typeof(AudioManager)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }


        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnSoundOff()
    {
        failAudio.volume = 0.0f;
        dogAudio.volume = 0.0f;
        winAudio.volume = 0.0f;
        buttonAudio.volume = 0.0f;
        breakAudio.volume = 0.0f;
        clockAudio.volume = 0.0f;
        starAudio.volume = 0.0f;
        pencilAudio.volume = 0.0f;
        peopleAudio.volume = 0.0f;
        burnAudio.volume = 0.0f;
        bigWaterAudio.volume = 0.0f;
        smallWaterAudio.volume = 0.0f;  
        coinCollectAudio.volume = 0.0f;
        soundState = 0;
    }

    public void TurnSoundOn()
    {
        failAudio.volume = 1.0f;
        winAudio.volume = 1.0f;
        dogAudio.volume = 1.0f;
        buttonAudio.volume = 1.0f;
        breakAudio.volume = 1.0f;
        clockAudio.volume=1.0f;
        starAudio.volume= 1.0f;
        pencilAudio.volume = 1.0f;
        peopleAudio.volume= 1.0f;
        burnAudio.volume= 1.0f;
        bigWaterAudio.volume= 1.0f;
        smallWaterAudio.volume= 1.0f;
        coinCollectAudio.volume= 1.0f;
        soundState = 1;
    }

    public void TurnMusicOff()
    {
        musicSource.volume = 0.0f;
        musicState = 0;
    }

    public void TurnMusicOn()
    {
        musicSource.volume = 0.8f;
        musicState = 1;
    }
}
