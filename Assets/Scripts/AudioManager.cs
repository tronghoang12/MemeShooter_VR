using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource effectsAudioSource;

    [SerializeField] AudioSource gameBGM;

    [SerializeField] AudioSource victoryMusic;
    [SerializeField][Range(0, 1)] float victoryMusicVol = 0.8f;

    [SerializeField] List<AudioClip> memeKilledSFX;
    [SerializeField][Range(0, 1)] float memeKilledVolume = 0.5f;

    [SerializeField] AudioClip btnPressedSFX;
    [SerializeField] [Range(0,1)] float btnPressedVol = 0.5f;

    bool audioIsPlaying = false;


    private void Awake()
    {
        effectsAudioSource = GetComponent<AudioSource>();
        //ManageSingleton();

        victoryMusic.enabled = false;
    }

    //void ManageSingleton()
    //{
    //    int instanceNum = FindObjectsOfType(GetType()).Length;

    //    if (instanceNum > 1)
    //    {
    //        {
    //            gameObject.SetActive(false);
    //            Destroy(gameObject);
    //        }
    //    }
    //    else
    //    {
    //        DontDestroyOnLoad(gameObject);
    //    }

    //}

    void PlayAudioClip(AudioClip audioToPlay, float volume)
    {
        if (effectsAudioSource != null)
        {
            effectsAudioSource.PlayOneShot(audioToPlay, volume);
        }
    }

    public void PlayMemeKilledSFX()
    {
        if (!audioIsPlaying)
        {
            int randomIndex = Random.Range(0, memeKilledSFX.Count - 1);
            PlayAudioClip(memeKilledSFX[randomIndex], memeKilledVolume);

            audioIsPlaying = true;
            StartCoroutine(ResetAudioIsPlaying(memeKilledSFX[randomIndex].length));
        }


    }

    public void PlayButtonPressedSFX()
    {
        PlayAudioClip(btnPressedSFX, btnPressedVol);
    }

    IEnumerator ResetAudioIsPlaying(float delay)
    {
        yield return new WaitForSeconds(delay);

        audioIsPlaying = false;
    }

    public void PlayVictorySFX()
    {
        victoryMusic.enabled = true;
        victoryMusic.volume = victoryMusicVol;
    }

    public void DisableBGM()
    {
        gameBGM.Stop();
        effectsAudioSource.Stop();
    }
}
