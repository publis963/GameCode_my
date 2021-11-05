using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject m_optionCanvas;
    public InGame m_inGame;

    void Update()
    {
        m_inGame.LoadScene();
    }

    public void ReplayNBack()
    {
        this.gameObject.SetActive(false);
        m_inGame.InitPlayer(false, true);
    }

    public void Option()
    {
        m_optionCanvas.SetActive(true);
    }

    public void Mainmenu()
    {
        m_inGame.FadeIn();
        m_inGame.m_load = 1;
    }
}

