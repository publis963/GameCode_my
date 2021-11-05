using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Mail : MonoBehaviour
{
    public Button m_levelButton;

    public MailData m_md;

    void Start()
    {
         m_md = new MailData();

        for (int i = 0; i <m_md.list.Count;i++)
        {
            Button obj = Instantiate(m_levelButton, transform.position, Quaternion.identity, GameObject.Find("MailButtonList").transform);
            Text mailText = obj.transform.Find("LevelText").GetComponent<Text>();
            mailText.text = m_md.list[i];
        }
    }

    public void Xbutton()
    {
        Destroy(gameObject);
    }
}

