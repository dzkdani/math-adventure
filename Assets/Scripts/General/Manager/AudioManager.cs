using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public List<SFXData> SFXDataList;
    public bool IsMute;

    [SerializeField]
    private List<AudioSource> sfxSourceList = new List<AudioSource>();

    private GameObject sfxContainer;
    private Coroutine _coroutine;
    string currentScene = null;

    public static AudioManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        { 
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        Init();
        LoadData();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != currentScene)
        {
            CheckScene();
        }
    }

    void CheckScene()
    {
        currentScene = SceneManager.GetActiveScene().name;
        bool sudah = false;
        for (int i = 0; i < sfxSourceList.Count; i++)
        {
            if (sfxContainer != null)
            {
                if (SFXDataList[i].SFXType.Equals(SFXType.BGM))
                {
                    foreach (string a in SFXDataList[i].scenes)
                    {
                        if (a.Equals(currentScene))
                        {
                            if(currentScene != "Intro")
                            {
                                if (!sfxSourceList[i].isPlaying) sfxSourceList[i].Play();
                                sudah = true;
                            }
                            break;
                        }
                    }

                    if (sudah.Equals(false)) sfxSourceList[i].Stop();
                    else sudah = false;
                }
            }
        }
    }

    private void Init()
    {
        for (int i = 0; i < SFXDataList.Count; i++)
        {
            if (sfxContainer == null)
            {
                sfxContainer = new GameObject();
                sfxContainer.name = "SFX";
                sfxContainer.transform.SetParent(transform);
            }
            GameObject audioSource = new GameObject();
            audioSource.transform.SetParent(sfxContainer.transform);
            audioSource.AddComponent<AudioSource>();

            if(SFXDataList[i].audioMixerGroup != null)
            {
                audioSource.GetComponent<AudioSource>().outputAudioMixerGroup = SFXDataList[i].audioMixerGroup;
            }

            audioSource.name = SFXDataList[i].SFXType.ToString();
            audioSource.GetComponent<AudioSource>().clip = SFXDataList[i].AudioClip;
            audioSource.GetComponent<AudioSource>().volume = SFXDataList[i].Volume;
            audioSource.GetComponent<AudioSource>().playOnAwake = SFXDataList[i].playOnAwake;
            audioSource.GetComponent<AudioSource>().loop = SFXDataList[i].loop;
            audioSource.GetComponent<AudioSource>().ignoreListenerPause = true;
            sfxSourceList.Add(audioSource.GetComponent<AudioSource>());
        }
    }

    public void PlaySFX(SFXType sfxType)
    {
        for (int i = 0; i < SFXDataList.Count; i++)
        {
            if (SFXDataList[i].SFXType == sfxType)
            {
                sfxSourceList[i].Play();
                break;
            }
        }
    }

    public void PlaySFX(int index)
    {
        sfxSourceList[index].Play();
    }

    public void PlaySFX(int index, Button.ButtonClickedEvent e)
    {
        sfxSourceList[index].Play();
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(InvokeAfterSfxHasDonePlaying(e, sfxSourceList[index].clip.length));
    }

    public void PlaySFX(int index, Action e)
    {
        sfxSourceList[index].Play();
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(InvokeAfterSfxHasDonePlaying(sfxSourceList[index].clip.length, e));
    }

    IEnumerator InvokeAfterSfxHasDonePlaying(Button.ButtonClickedEvent e, float time)
    {
        yield return new WaitForSeconds(time+0.1f);
        e.Invoke();
        yield return new WaitForEndOfFrame();
    }
    IEnumerator InvokeAfterSfxHasDonePlaying(float time, Action e)
    {
        yield return new WaitForSeconds(time+ 0.1f);
        e.Invoke();
        yield return new WaitForEndOfFrame();
    }

    public void MuteAudio()
    {
        IsMute = true;
        for (int i = 0; i < SFXDataList.Count; i++)
        {
            sfxSourceList[i].mute = true;
        }
        SaveData();
    }
    public void UnmuteAudios()
    {
        IsMute = false;
        for (int i = 0; i < SFXDataList.Count; i++)
        {
            sfxSourceList[i].mute = false;
        }
        SaveData();
    }
    public void SaveData()
    {
        if (IsMute)
        {
            PlayerPrefs.SetInt("MuteAudio", 1);
        }
        else
        {
            PlayerPrefs.SetInt("MuteAudio", 0);
        }
    }
    public void LoadData()
    {
        if (PlayerPrefs.GetInt("MuteAudio", 0) == 1)
        {
            IsMute = true;
        }
        else
        {
            IsMute = false;
        }
    }

    [System.Serializable]
    public struct SFXData
    {
        public SFXType SFXType;
        public AudioClip AudioClip;
        public AudioMixerGroup audioMixerGroup;
        public float Volume;
        public bool playOnAwake;
        public bool loop;
        public string[] scenes;
    }

    [System.Serializable]
    public enum SFXType
    {
        ButtonClick,
        BGM
    }
}
