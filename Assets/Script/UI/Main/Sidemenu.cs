using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sidemenu : MonoBehaviour
{
    public GameObject m_OptionCanvas;
    public GameObject m_MailCanvas;
    public GameObject m_AwardCanvas;
    public GameObject m_RankingCanvas;
    public GameObject m_StoreCanvas;
    public GameObject m_PurchaseCanvas;

    public void Option()
    {
        m_OptionCanvas.SetActive(true);
    }

    public void Mail()
    {
        m_MailCanvas.SetActive(true);
    }

    public void Award()
    {
        m_AwardCanvas.SetActive(true);
    }

    public void Ranking() //0408
    {
        m_RankingCanvas.SetActive(true);
    }

    public void Store() // 0408
    {
        m_StoreCanvas.SetActive(true);
    }

    public void Purchase()//0412
    {
        m_PurchaseCanvas.SetActive(true);
    }
}
