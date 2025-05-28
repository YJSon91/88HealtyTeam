using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource footstepSource;

    [Header("Audio Clips")]
    public AudioClip lobbyBGM;
    public AudioClip stageBGM_Normal;
    public AudioClip stageBGM_Emergency;
    public AudioClip damageSound;
    public AudioClip buttonPressSound;
    public AudioClip doorOpenSound;
    public AudioClip footstepWalk;
    public AudioClip footstepRun;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayStageBGM(string stageName, bool isEmergency)
    {
        bgmSource.clip = isEmergency ? stageBGM_Emergency : stageBGM_Normal;
        bgmSource.Play();
    }

    public void PlayLobbyBGM()
    {
        bgmSource.clip = lobbyBGM;
        bgmSource.loop = true;
        bgmSource.volume = 1f;
        bgmSource.Play();
    }

    public void PlaySFX(string sfxName)
    {
        AudioClip clip = Resources.Load<AudioClip>("SFX/" + sfxName);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SFX not found: " + sfxName);
        }
    }

    public void PlayPlayerFootstep(bool isRunning)
    {
        AudioClip clip = isRunning ? footstepRun : footstepWalk;
        footstepSource.clip = clip;
        if (!footstepSource.isPlaying)
        {
            footstepSource.Play();
        }
    }

    public void PlayDamageSound()
    {
        sfxSource.PlayOneShot(damageSound);
    }

    public void PlayButtonPressSound()
    {
        sfxSource.PlayOneShot(buttonPressSound);
    }

    public void PlayDoorOpenSound()
    {
        sfxSource.PlayOneShot(doorOpenSound);
    }
}
