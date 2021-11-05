using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

[System.Serializable]

public class Tutorial : MonoBehaviour
{
    public Toggle m_checkMark;
    FileMng m_fileMng;

    void Start()
    {
        m_fileMng = GameObject.Find("FileMng").GetComponent<FileMng>();
    }

    public void Back()
    {
        Static.Save(m_checkMark.isOn, Static.m_gameData.m_inGameTutoCheck);//2019.02.15
        gameObject.SetActive(false);//2019.03.18
    }
}