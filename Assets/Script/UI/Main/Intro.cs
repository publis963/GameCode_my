using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

[System.Serializable]
public class Intro : MonoBehaviour
{
    public Animation m_teamLogoAni;
    public Animation m_titleLogoAni;
    public Animation m_fadeAni;
    public Animation m_touchScreenAni;
    public Animation m_loadingAni;

    public GameObject m_topMenu;
    public GameObject m_sideMenu;
    public GameObject m_fadeImage;

    public GameObject m_introObject;
    FileMng m_filemng;

    public float m_changeTime = 1.5f;
    private float m_time;
    private int m_count;
    private int m_countCount;
    private bool m_click;
    private bool m_once;
    public bool m_checkLogin;

    void Start()
    {
        m_filemng = GameObject.Find("FileMng").GetComponent<FileMng>();

        m_count = 0;
        m_countCount=0;
        m_time = 0;

        m_click = true;
        m_once = true;
        m_checkLogin = false;
    }

    void Update()
    {
        if (Static.m_isDone == 0)
        {
            switch (m_count)
            {
                case 0: // team
                    PlayTeamLogoUI();
                    break;

                case 1: // Title
                    PlayTitleLogoUI();
                    break;

                case 2: // Main Fade Image
                    MainFadeOutUI();
                    break;

                case 3: // Loading
                    PlayLoadingUI();
                    break;

                case 4:
                    PlayTouchScreenUI(); // TouchScrean
                    break;

                case 5:
                    AllFadeOut();
                    break;

                default:
                    Tutorial();
                    break;
            }
        }
        else if (Static.m_isDone == 1)
        {
            switch (m_countCount)
            {
                case 0:
                    m_fadeAni.Play("Ani_fadeOut");
                    m_countCount += 1;
                    break;

                default:
                    if (m_fadeAni.isPlaying == false)
                    {
                        m_fadeImage.SetActive(false);

                        m_topMenu.SetActive(true);
                        m_sideMenu.SetActive(true);

                        m_introObject.SetActive(false);

                        Static.m_isDone = 2;
                    }
                    break;
            }
        }
    }

    void PlayTeamLogoUI()
    {
        m_teamLogoAni.Play("Ani_teamLogo");

        m_count += 1;
    }

    void PlayTitleLogoUI()
    {
        if (m_teamLogoAni.isPlaying == false)
        {
            m_titleLogoAni.Play("Ani_gameLogoFadeIn");

            m_count += 1;
        }
    }

    void MainFadeOutUI()
    {
        m_time += Time.deltaTime;

        if (m_time >= m_changeTime)
        {
            if (m_titleLogoAni.isPlaying == true)
            {
                m_fadeAni.Play("Ani_introFadeOut");
            }
            m_count += 1;
        }
    }

    void PlayLoadingUI() // loading
    {
        if (m_fadeAni.GetComponent<Image>().color.a <= 0)
        {
            m_fadeImage.SetActive(false);
            m_loadingAni.Play();
            m_count += 1;
        }
    }

    void PlayTouchScreenUI() // Touch
    {
        if (m_checkLogin)
        {
            m_loadingAni.wrapMode = WrapMode.Default;
            if (!m_loadingAni.isPlaying)
            {
                m_touchScreenAni.Play();
                m_count += 1;
            }
        }
    }

    void AllFadeOut() 
    {
        if (Input.GetMouseButton(0) && m_click)
        {
            m_titleLogoAni.Play("Ani_gameLogoFadeOut"); // title Out
            m_touchScreenAni.Play("Ani_fadeOut"); // touch Out

            m_click = false;
            
            m_count += 1;
        }  
    }

    void Tutorial()
    {
        if (!m_titleLogoAni.isPlaying && m_once && !m_touchScreenAni.isPlaying)
        {
            m_fadeImage.SetActive(false);
            m_topMenu.SetActive(true);
            m_sideMenu.SetActive(true);

            if (!Static.m_gameData.m_tutorialCheck)
            {
                GameObject obj = GameObject.Find("MainUI").transform.Find("TutorialWindow").gameObject;
                obj.SetActive(true);
            }
            m_introObject.SetActive(false);

            m_once = false;
        }
    }
}