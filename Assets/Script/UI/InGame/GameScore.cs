using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{
    //게임 스코어
    public AudioSource m_audio;

    public Text m_timeBonusTxt;
    public Text m_totalTimeTxt;
    public Text m_timeTotalScoreTxt;
    public Text m_goldBonusTxt;
    public Text m_goldNumTxt;
    public Text m_goldScoreTxt;
    public Text m_silverBonusTxt;
    public Text m_silverNumTxt;
    public Text m_silverScoreTxt;
    public Text m_bronzBonusTxt;
    public Text m_bronzNumTxt;
    public Text m_bronzScoreTxt;
    public Text m_totalScoreTxt;
    public Text m_coinNumTxt;

    public InGame m_inGame;
    public List<Button> m_buttonList;
    public List<Animation> m_aniList;

    private GameSetUp m_gameSetUp;
    private TileManager m_tileMng;

    public string m_restart;
    public string m_main;

    private float m_totalTime;
    private float m_timeScore;
    private float m_totalScore;
    public int m_timeBonus;
    private int m_count;

    private bool m_once;

    private GameObject m_finishAD;

    //2019.05.10
    public GameObject m_alertIMG;


    void Start()
    {
        m_gameSetUp = GameObject.Find("GameSetUp").GetComponent<GameSetUp>();
        m_tileMng = GameObject.Find("MapMaker").GetComponent<MakingMap>().m_makedMaps[0].GetComponent<TileManager>();
        m_finishAD = GameObject.Find("GameManager");

        m_count = 0;

        m_once = true;
        ButtonList(false);

        Test_Score();
    }

    void Update()
    {
        switch (m_count)
        {
            case 0:
                PlayCompoAni(0);
                break;
            case 1:
                PlayCompoAni(1);
                break;
            case 2:
                PlayCompoAni(2);
                break;
            case 3:
                PlayCompoAni(3);
                break;
            case 4:
                PlayCompoAni(4);
                break;
            case 5:
                PlayCompoAni(5);
                break;
            case 6:
                PlayCompoAni(6);
                break;
            case 7:
                PlayCompoAni(7);
                break;
            case 8:
                PlayCompoAni(8);
                break;
            case 9:
                PlayCompoAni(9);
                break;

            case 10:
                m_once = false;
                ButtonList(true);

                m_inGame.LoadScene();
                break;
        }
    }

    void PlayCompoAni(int num)
    {
        if (m_aniList[num].isPlaying == false)
        {
            m_aniList[num + 1].Play();
            m_count += 1;
        }
    }

    public void Test_Score()
    {
        m_totalTime = m_tileMng.m_time;
        m_timeScore = m_totalTime * m_timeBonus;
        m_timeBonusTxt.text = m_timeBonus.ToString();
        m_totalTimeTxt.text = m_tileMng.m_time.ToString("N2");
        m_timeTotalScoreTxt.text = m_timeScore.ToString("N0");

        //금 동전
        m_goldBonusTxt.text = m_gameSetUp.m_goldScore.ToString();
        m_goldNumTxt.text = m_gameSetUp.m_totalGoldCoin.ToString();
        m_goldScoreTxt.text = m_gameSetUp.m_totalGoldScore.ToString();

        //은 동전
        m_silverBonusTxt.text = m_gameSetUp.m_silverScore.ToString();
        m_silverNumTxt.text = m_gameSetUp.m_totalSilverCoin.ToString();
        m_silverScoreTxt.text = m_gameSetUp.m_totalSilverScore.ToString();

        //동 동전
        m_bronzBonusTxt.text = m_gameSetUp.m_bronzScore.ToString();
        m_bronzNumTxt.text = m_gameSetUp.m_totalBronzCoin.ToString();
        m_bronzScoreTxt.text = m_gameSetUp.m_totalBronzScore.ToString();

        m_coinNumTxt.text = m_gameSetUp.TotalCoinScore().ToString();

        m_totalScore = m_timeScore + m_gameSetUp.TotalCoinScore();
        m_totalScoreTxt.text = m_totalScore.ToString("N0");
        //Debug.Log(m_gameSetUp.TotalCoinScore());

        ////2019.05.10 + 2019.05.13
        //Static.m_currentUser.Money += m_gameSetUp.TotalCoinScore();
        //if ((int)m_totalScore > Static.m_currentUser.Score)
        //{
        //    Static.m_currentUser.Score = (int)m_totalScore;
        //}
        //string json;
        //json = JsonUtility.ToJson(Static.m_currentUser);
        //Static.SaveDB(json);
    }

    //public void RestartScene() //2019.05.10
    //{
    //    if (Static.m_currentUser.VanCha > 0)
    //    {
    //        Static.m_currentUser.VanCha -= 1;
    //        string json;
    //        json = JsonUtility.ToJson(Static.m_currentUser);
    //        Static.SaveDB(json);
    //        m_audio.Play();
    //        m_inGame.FadeIn();
    //        m_inGame.m_load = 0;
    //        ShowAD();
    //    }
    //    else
    //    {
    //        m_alertIMG.SetActive(true);
    //    }
    //}

    public void BackMain()
    {
        m_audio.Play();
        m_inGame.FadeIn();
        m_inGame.m_load = 1;

        //ShowAD();
    }

    void ButtonList(bool isEnabled)
    {
        for (int i = 0; i < m_buttonList.Count; i++)
        {
            m_buttonList[i].enabled = isEnabled;
        }
    }

    public void SkipAni() //2019.03.22
    {
        for (int i = 0; i < m_aniList.Count; i++)
        {
            m_aniList[i][m_aniList[i].clip.name].speed = 4.0f;
        }
    }

    //private void ShowAD()
    //{
    //    if (Static.m_currentUser.isAdPaid == "NPAID") //2019.05.28
    //    {
    //        int maxP = 3;
    //        int percentage = 1;

    //        if (percentage == Random.Range(0, maxP))
    //        {
    //            m_finishAD.GetComponent<UnityAdScript>().ShowAd();
    //        }
    //    }
    //}

    //2019.05.10
    public void CancelButton()
    {
        m_alertIMG.SetActive(false);
    }
}