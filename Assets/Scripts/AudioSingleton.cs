using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSingleton : MonoBehaviour
{
    public Sound[] sounds;
    public Image fadeScreen;
    string activeSound;
    public static bool finalCutscene = true;

    private static AudioSingleton _instance;
    public static AudioSingleton instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioSingleton>();
                if (_instance == null)
                {
                    throw new UnityException("Instance of AudioSingleton not found in scene");
                }
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
        }
        _instance = this;
        DontDestroyOnLoad(_instance);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        PlayMusic("MenuMusic");
    }

    private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Stop(_instance.activeSound);
            SceneManager.LoadScene(1);
            PlayMusic("IntroStagesMusic");
        }
        if (Input.GetKeyDown("2"))
        {
            Stop(_instance.activeSound);
            SceneManager.LoadScene(3);
            PlayMusic("CyanStagesMusic");
        }
        if (Input.GetKeyDown("3"))
        {
            Stop(_instance.activeSound);
            SceneManager.LoadScene(6);
            PlayMusic("YellowStagesMusic");
        }
        if (Input.GetKeyDown("4"))
        {
            Stop(_instance.activeSound);
            SceneManager.LoadScene(10);
            PlayMusic("MagentaStagesMusic");
        }
        if (Input.GetKeyDown("5"))
        {
            Stop(_instance.activeSound);
            SceneManager.LoadScene(15);
            PlayMusic("BossMusic");
        }
    }

    public static void Play(string name)
    {
        Sound s = Array.Find(_instance.sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public static void PlayMusic(string name)
    {
        Sound s = Array.Find(_instance.sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
        _instance.activeSound = name;
    }


    // Stop a Sound by name, ex. Stop("Song1")
    public static void Stop(string name)
    {
        Sound s = Array.Find(_instance.sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    public static void NextScene()
    {
        int scene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(scene);
        if (scene == 1)
        {
            Stop(_instance.activeSound);
            PlayMusic("IntroStagesMusic");
        }
        else if (scene == 3)
        {
            Stop(_instance.activeSound);
            PlayMusic("CyanStagesMusic");
        }
        else if (scene == 6)
        {
            Stop(_instance.activeSound);
            PlayMusic("YellowStagesMusic");
        }
        else if (scene == 10)
        {
            Stop(_instance.activeSound);
            PlayMusic("MagentaStagesMusic");
        }
        else if (scene == 15)
        {
            Stop(_instance.activeSound);
            PlayMusic("BossMusic");
        }
        else if (scene == 16)
        {
            Stop(_instance.activeSound);
            PlayMusic("MenuMusic");
        }
    }
}

