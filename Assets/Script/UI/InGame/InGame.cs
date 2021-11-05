using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.U2D;

public class InGame : MonoBehaviour
{
    public GameObject m_InGameImage;
    public GameObject m_menuObject;
    public GameObject m_skillButton;
    public Animation m_InGameImageFadeOutAni;
    public Animator m_animator;
    private TileManager m_tileMng;
    private PlayerScript m_player;
    private GameSetUp m_gameSetUp;
    public GameObject m_scoreBoard;

    public float m_gameOverTime;//게임오버 조건:시간
    public int m_countTile;//게임오버 조건: 타일
    public string m_loadingNxtScene;

    private bool m_isPlaying;
    public int m_load;
    public SpriteAtlas m_atlas;
    public Image[] m_imageArr;

    void Start()
    {
        m_gameSetUp = GameObject.Find("GameSetUp").GetComponent<GameSetUp>();
        m_player = GameObject.Find("Player").GetComponent<PlayerScript>();
        m_tileMng = GameObject.Find("MapMaker").GetComponent<MakingMap>().m_makedMaps[0].GetComponent<TileManager>();

        m_isPlaying = true;
        m_load = 0;
        m_InGameImageFadeOutAni.Play();

        for (int i = 0; i < m_imageArr.Length; ++i)
        {
            m_imageArr[i].sprite = m_atlas.GetSprite(m_imageArr[i].sprite.name);
        }
    }

    void Update()
    {
        DestroyInGameImage();
    }

    public void GameOver()
    {
        m_scoreBoard.SetActive(true);
        m_skillButton.SetActive(false);
        //버튼 비활성화
    }

    void DestroyInGameImage()
    {
        if (m_InGameImageFadeOutAni.isPlaying == false && m_isPlaying)
        {
            m_InGameImage.SetActive(false);

            m_isPlaying = false;
        }
    }

    public void ClickMenu()
    {
        m_menuObject.SetActive(true);

        if (m_menuObject.activeSelf == true)
        {
            InitPlayer(true, false);
        }
    }

    public void InitPlayer(bool init, bool _init)
    {
        m_gameSetUp.SetKinematic(init, false);//true
        m_tileMng.m_timeStop = _init;//false
        m_player.m_life = _init;//false
    }

    // loading  //
    public void LoadScene()
    {
        if (m_InGameImage.GetComponent<Image>().color.a == 1.0f)
        {
            if (m_load == 0)
            {
                SceneManager.LoadScene("TestInGame");
                Debug.Log("Load: TestInGame");
            }
            else
            {
                Loading.LoadScene("Main02");
                Debug.Log("Load: Main02");
            }
        }

        Static.m_isDone = 1;
    }

    public void FadeIn()
    {
        GameObject go = GameObject.Find("InGameCanvas").transform.Find("FadeImage").gameObject;
        go.SetActive(true);

        Animation ani = GameObject.Find("FadeImage").GetComponent<Animation>();
        ani.Play("Ani_fadeIn");
    }
}