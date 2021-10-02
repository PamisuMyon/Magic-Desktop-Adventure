using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set;}

    public AudioMixer mixer;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            float volume;
            mixer.GetFloat("MusicVolume", out volume);
            if (volume == -80f)
                volume = 0;
            else 
                volume = -80f;
            mixer.SetFloat("MusicVolume", volume);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            float volume;
            mixer.GetFloat("SfxVolume", out volume);
            if (volume == -80f)
                volume = 0;
            else 
                volume = -80f;
            mixer.SetFloat("SfxVolume", volume);
        }
    }

}
