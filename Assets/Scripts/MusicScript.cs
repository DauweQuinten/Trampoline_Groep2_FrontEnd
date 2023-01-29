using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                source.clip = MusicClips[0];
                break;
            case "GameOver":
                source.clip = MusicClips[1];
                break;
            case "BoatGame2.0":
                source.clip = MusicClips[2];
                break;
            default:
                source.clip = MusicClips[0];
                break;
        }

        if (source.clip != Audio.clip)
        {
            Audio.enabled = false;
            Audio.clip = source.clip;
            Audio.enabled = true;
        }
    }

}
