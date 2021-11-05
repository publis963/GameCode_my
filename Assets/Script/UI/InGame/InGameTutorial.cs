using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameTutorial : MonoBehaviour
{
    public List<string> m_textList;
    public List<Sprite> m_spriteList;

    public Text m_text;
    public Image m_image;

    public Button m_rightButton;
    public Button m_leftButton;
    public FileMng m_fileMng;
    public Toggle m_check;

    private int m_clickNum;
    private bool m_once;


    void Start()
    {
        m_fileMng = GameObject.Find("FileMng").GetComponent<FileMng>();
        m_clickNum = 0;
        m_once = true;

        m_text.text = m_textList[m_clickNum].ToString();
        m_image.sprite = m_spriteList[m_clickNum];
    }

    void Update()
    {
        if (Static.m_gameData.m_inGameTutoCheck == true && m_once)
        {
            this.gameObject.SetActive(false);
            m_once = false;
        }

        CheckList();
    }

    void CheckList()
    {
        if (m_clickNum == m_textList.Count - 1)
        {
            m_rightButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            m_rightButton.GetComponent<Button>().interactable = true;
        }

        if (m_clickNum == 0)
        {
            m_leftButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            m_leftButton.GetComponent<Button>().interactable = true;
        }
    }

    public void Right()
    {
        ImagenTextList(1);
    }

    public void Left()
    {
        ImagenTextList(-1);
    }

    void ImagenTextList(int num)
    {
        m_clickNum += num;
        m_text.text = m_textList[m_clickNum].ToString();
        m_image.sprite = m_spriteList[m_clickNum];
    }

    public void Back()
    {
        if (m_check.isOn == true)
        {
            Static.Save(Static.m_gameData.m_tutorialCheck, m_check.isOn);//2019.02.15
        }
        this.gameObject.SetActive(false);
    }
}
