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
    public AudioClip doorCloseSound;
    public AudioClip footstepWalk;
    public AudioClip footstepRun;
    public AudioClip jumpSound;
    public AudioClip stageSuccessBGM;

    private string currentBGM = "";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 다른 씬으로 넘어가도 이 오브젝트는 파괴되지 않고 유지됨. 
        }
        else
        {
            Destroy(gameObject);  // 만약 다른 씬에서 이미 존재한다면 중복 방지를 위해 자기 자신을 파괴함.
        }
    }

    public void PlayStageBGM(string stageName, bool isEmergency)
    {
        bgmSource.clip = isEmergency ? stageBGM_Emergency : stageBGM_Normal;
        bgmSource.volume = 0.05f;
        bgmSource.Play();
        currentBGM = "Stage";
    }

    public void PlayLobbyBGM()
    {
        if (currentBGM == "Lobby") return;

        bgmSource.clip = lobbyBGM;
        bgmSource.loop = true;
        bgmSource.volume = 1f;
        bgmSource.Play();

        currentBGM = "Lobby";
    }

    public void PlayStageSuccessBGM()
    {
        bgmSource.clip = stageSuccessBGM;
        bgmSource.Play();

        currentBGM = "StageSuccess";
    }

    public void PlaySFX(string sfxName)  
    {
        AudioClip clip = Resources.Load<AudioClip>("SFX/" + sfxName); // Resources.Load로 효과음들 로드
        if (clip != null)                                             // Resources폴더안에 SFX 폴더안에 오디오네임
        {                                                            // 점프에 효과음을 넣고싶으면 점프 코드안에 밑에 코드를 넣어주면 효과음이 나옴
            sfxSource.PlayOneShot(clip);                             // SoundManager.Instance.PlaySFX("jumpSound");
        }
        else
        {
            Debug.LogWarning("SFX not found: " + sfxName);           // 파일이 없으면 콘솔에 경고 출력
        }
    }

    public void PlayPlayerFootstep(bool isRunning)
    {
        AudioClip clip = isRunning ? footstepRun : footstepWalk;
        sfxSource.PlayOneShot(clip);
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
    public void PlayDoorCloseSound()
    {
        sfxSource.PlayOneShot(doorCloseSound);
    }

    public void PlayJumpSound()
    {
        sfxSource.PlayOneShot(jumpSound);
    }

    public void StopFootstep()
    {
        if (footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume); // 0.0 ~ 1.0 사이로 제한
    }
}
