using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class MusicScript : MonoBehaviour
{

    public AudioClip[] MusicClips;

    public AudioSource Audio;
    
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameMusic");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        AudioSource source = new AudioSource();

        switch (scene.name)
        {
            case "Startscherm":
                checkPrevAudio(MusicClips[0]);
                break;
            case "GameOver":
                checkPrevAudio(MusicClips[1]);
                break;
            case "BoatGame2.0":
                checkPrevAudio(MusicClips[2]);
                break;
            default:
                checkPrevAudio(MusicClips[0]);
                break;
        }
    }

    private void checkPrevAudio(AudioClip clip)
    {
        if (clip != Audio.clip)
        {
            Audio.enabled = false;
            Audio.clip = clip;
            Audio.enabled = true;
        }
    }

}
