using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;


[System.Serializable]
public class MainMenuOption : MonoBehaviour
{
    public Slider m_bgmSlider;
    public Slider m_effectSlider;
    public Animation m_canAni;
    public Animation m_okButtonAni;
    public Animation m_delButtonAni;

    private AudioSource m_click;
    private AudioSource m_bgmVolumeAudio;
    private AudioSource m_effectVolumeAudio;
    private FileMng m_fileMng;

    private int m_previousRes;

    private int m_choose;
    private bool m_once;

    void OnEnable()
    {
        m_canAni.Play("Ani_popUp"); 
        m_choose = 0;
        m_once = true;
    }

    void Start()
    {
        m_fileMng = GameObject.Find("FileMng").GetComponent<FileMng>();
        m_click = GameObject.Find("Click").GetComponent<AudioSource>();
    }

    void Update()
    {
        ButtonAni(); 
    }

    public void Ok_Save()
    {
        Static.m_gameData.m_bgmcheck = m_bgmVolumeAudio.volume;
        m_bgmVolumeAudio.volume = m_bgmSlider.value;

        Static.m_gameData.m_Resolution = m_previousRes;
        Static.Save(Static.m_gameData.m_tutorialCheck, Static.m_gameData.m_inGameTutoCheck);//2019.02.15 

        m_okButtonAni.Play("Ani_button");
        m_click.Play();

        m_previousRes = 0;
        m_choose = 1;
    }

    public void Delete()
    {
        m_delButtonAni.Play("Ani_button");
        m_click.Play();

        ResSetting();

        m_choose = 2;
        m_previousRes = 0;
    }

    public void ResSetting()
    {
        switch (Static.m_gameData.m_Resolution)
        {
            case 1:
                LowResol();
                break;
            case 2:
                MidResol();
                break;
            case 3:
                HighResol();
                break; 
        }
    }
    public void SetBGMVolume()
    {
        if (SceneManager.GetActiveScene().name == "Main02")
        {
            m_bgmVolumeAudio = GameObject.Find("BGM").GetComponent<AudioSource>();
        }
        else if (SceneManager.GetActiveScene().name == "TestInGame")
        {
            if (GameObject.Find("BGM_InGame") != null)
            {
                m_bgmVolumeAudio = GameObject.Find("BGM_InGame").GetComponent<AudioSource>();
            }
            else
            {
                m_bgmVolumeAudio = GameObject.Find("BGM_RedZone").GetComponent<AudioSource>();
            }
        }

        m_bgmVolumeAudio.volume = m_bgmSlider.value; 
    } 
    void ButtonAni()
    {
        if (m_choose == 1)
        {
            if (m_okButtonAni.isPlaying == false)
            {
                PopDown();
            }
        }
        else if (m_choose == 2)
        {
            if (m_delButtonAni.isPlaying == false)
            {
                PopDown();
            }
        }
    }

    void PopDown()
    {
        if (m_once)
        {
            m_canAni.Play("Ani_popDown");
            m_once = false;
        }

        if (m_canAni.isPlaying == false)
        {
            this.gameObject.SetActive(false);

            m_bgmSlider.value = Static.m_gameData.m_bgmcheck;//2019.03.15
            m_effectSlider.value = Static.m_gameData.m_effectcheck;//3019.03.15
        }
    }

    public void LowResol()
    {
        Screen.SetResolution(Static.m_playerWindowWidth / 2, Static.m_playerWindowHeight / 2, true);
        m_previousRes = 1; 
        Debug.Log("Low");
    }

    public void MidResol()
    {
        Screen.SetResolution(Static.m_playerWindowWidth / 4 * 3, Static.m_playerWindowHeight / 4  * 3, true);
        m_previousRes = 2; 
        Debug.Log("Middle");
    }

    public void HighResol()
    {
        Screen.SetResolution(Static.m_playerWindowWidth, Static.m_playerWindowHeight, true);
        m_previousRes = 3; 
        Debug.Log("High");
    }

    public void RePlayTuto()//2019.03.18
    {
        gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Main02")
        {
            GameObject obj2 = GameObject.Find("MainUI").transform.Find("TutorialWindow").gameObject;
            obj2.SetActive(true);
        }
        else if (SceneManager.GetActiveScene().name == "TestInGame")
        {
            GameObject obj1 = GameObject.Find("InGameCanvas").transform.Find("InGameTutorial").gameObject;
            obj1.SetActive(true);
        }
    }

    //public void Rateit()
    //{
    //    Application.OpenURL("market://details?id=com.ksy.ex1");
    //}

}

