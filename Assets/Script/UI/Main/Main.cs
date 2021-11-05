using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Animation m_fadeImageAni;
    public GameObject m_storeWindow;
    public string m_loadingNxtScene;//loadingscene다음에 나올 씬
    private bool m_isPlaying;
     
    void Start()
    {
        m_isPlaying = false;
    }

    void Update()
    {
        LoadScene();
    }

    void LoadScene()
    {
        if (m_fadeImageAni.isPlaying == false && m_isPlaying)
        {
            Debug.Log("이전: " + m_loadingNxtScene);
            Loading.LoadScene(m_loadingNxtScene);

            m_isPlaying = false;
        }
    }

    public void ButtonClick()
    {
        if (Static.m_currentUser.VanCha > 0) // 0419
        {
            Static.m_currentUser.VanCha -= 1;

            string json;
            json = JsonUtility.ToJson(Static.m_currentUser);
            Static.SaveDB(json);
        }
        else
        {
            m_storeWindow.SetActive(true);
            return;
        }

        GameObject go = GameObject.Find("MainUI").transform.Find("FadeImage").gameObject;
        go.SetActive(true);

        Animation ani = GameObject.Find("FadeImage").GetComponent<Animation>();
        ani.Play("Ani_fadeIn");

        m_isPlaying = true;
    }
}