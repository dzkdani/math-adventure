using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioSource SFX, Mouth, Environment, Run;

    public List<AudioListener> audioListener;

    //0. run //1. dash //2. boy hit //3. girl hit //4. hop boy //5. hop girl //6. arrive in land
    //7. terkena gada //8. terkena missle
    public AudioClip[] audioClips;

    public bool SFX_IsPlaying = false;

    private void Start()
    {
        init_PlayerRun();
    }

    public void PlayerLanding()
    {
        Environment.PlayOneShot(audioClips[6]);
    }

    public void PlayerGettingHitByRocket()
    {
        Environment.PlayOneShot(audioClips[8]);
    }

    public void PlayerGettingHitByGada()
    {
        Environment.PlayOneShot(audioClips[7]);
    }

    public void PlayerSlide()
    {
        Run.Pause();
        SFX.pitch = 1;
        if (!SFX_IsPlaying)
        {
            SFX_IsPlaying = true;
            SFX.PlayOneShot(audioClips[1]);
        }
    }

    public void init_PlayerRun()
    {
        Run.clip = audioClips[0];
        Run.pitch = 3.5f;
        Run.loop = true;
        Run.Play();
        Run.Pause();
    }

    public void PlayerRun()
    {
        Run.UnPause();
    }

    public void PlayAudioButton(ExtendedButtonOnClickPassingVariable e)
    {
        AudioManager.Instance.PlaySFX(e._integer[0], e._onClick);
    }
    public void PlayAudioButton(int index)
    {
        AudioManager.Instance.PlaySFX(index);
    }

    public void PlayerStop()
    {
        SFX.Pause();
        SFX_IsPlaying = false;

        Run.Pause();
    }

    public void Mulut_Cowok_GetDamage()
    {
        if (!Mouth.isPlaying)
        {
            Mouth.PlayOneShot(audioClips[2]);
        }
    }

     public void Mulut_Cowok()
    {
        if (!Mouth.isPlaying)
        {
            Mouth.PlayOneShot(audioClips[4]);
        }
    }

    public void Mulut_Cewek()
    {
        if (!Mouth.isPlaying)
        {
            Mouth.PlayOneShot(audioClips[5]);
        }
    }

    public void Mulut_Cewek_GetDamage()
    {
        if (!Mouth.isPlaying)
        {
            Mouth.PlayOneShot(audioClips[3]);
        }
    }


    public void AudioListenerON() { audioListener.ForEach(x=> x.enabled = true); }
}
