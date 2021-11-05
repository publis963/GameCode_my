using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 게임에 필요한 설정
public class GameSetUp : MonoBehaviour
{
    public GameObject m_setUpPoint;
    public GameObject m_setUpSliderObject;

    public Slider m_setUpslider;

    public Animation m_tReadyAni;
    public Animation m_fadeImageAni;
    public Animation m_tStartAni;
    public Animation m_topMenuAni;

    public Text m_coinText;
    public Text m_timeText;

    public ParticleSystem m_particle;
    public ParticleSystem.MainModule m_particleMain;

    private TileManager m_tileMng;
    private PlayerScript m_playerScript;
    private Camera m_camera;
    public List<Rigidbody> m_rgList;

    public float m_sliderSpeed;
    public float m_time;
    public float m_goodRange;
    public float m_goodSpeed;
    public float m_timeSpeed;
    public float m_originSpeed;
    public float m_feverTime;
    public float m_fovSpeed;
    private float m_value;

    // skill
    public Button m_feverButton;
    public Slider m_feverGauge;
    public float m_feverSV;
    private float m_timeInFever;

    public Button m_attackButton;
    public Slider m_attackGauge;
    public float m_speanSV;
    public bool m_attackTrigger;

    private Transform[] m_prevTransf;
    private Transform[] m_storeTransf;
    private Quaternion[] m_storeQua;
    public GameObject m_joint;
    private int m_count;
    public bool m_fever; // FeverTime 한번 호출
    private bool m_stopHandle;
    private bool m_flugFever;
    public GameObject m_feverTrail;
    public GameObject m_inGameTuto;
    public InGame m_inGameMng; // 2019.04.05

    // 코인 점수
    public int m_getCoin;
    public int m_totalGoldCoin;
    public int m_totalSilverCoin;
    public int m_totalBronzCoin;

    public int m_totalGoldScore;
    public int m_totalSilverScore;
    public int m_totalBronzScore;
    public int m_goldScore;
    public int m_silverScore;
    public int m_bronzScore;

    // UI 관련
    public Animation m_bossUIL;
    public Animation m_bossUIR;
    public Animation m_bossFace;
    public Text m_exposureText;
    public int m_exposureLevel;

    // ETC
    public bool m_pause;

    void Start()
    {
        m_time = 0;
        m_count = 0;
        m_getCoin = 0;

        m_stopHandle = true;
        m_fever = true;
        m_flugFever = false;
        m_pause = false;
        //m_timeInFever = 0;
        m_attackGauge.value = 0;
        m_feverGauge.value = 0;
        m_attackTrigger = false;

        m_tileMng = GameObject.Find("MapMaker").GetComponent<MakingMap>().m_makedMaps[0].GetComponent<TileManager>();
        m_playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();

        m_playerScript.m_life = false;
        m_originSpeed = m_playerScript.m_moveSpeed;
        m_value = m_setUpslider.maxValue;

        m_particleMain = m_particle.main;

        m_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        m_inGameMng = GameObject.Find("InGameCanvas").GetComponent<InGame>();

        m_prevTransf = m_joint.GetComponentsInChildren<Transform>();
        m_storeTransf = new Transform[m_joint.GetComponentsInChildren<Transform>().Length];
        m_storeQua = new Quaternion[m_prevTransf.Length];

        for (int i = 0; i < m_prevTransf.Length; i++)
        {
            m_storeQua[i] = m_prevTransf[i].localRotation;
            m_storeTransf[i] = m_prevTransf[i];
        }

        m_exposureLevel = 0;
        m_exposureText.text = "0%";
    }

    void Update()
    {
        if (m_inGameTuto.activeSelf == false)
        {
            switch (m_count)
            {
                case 0:
                    SetInitalizeRot();
                    break;

                case 1:
                    PlayTStartAni();
                    break;

                case 2:
                    PlayTopMenuAni();
                    break;

                default:
                    if (m_pause == false)
                    {
                        GameTime();

                        FeverSkillCharge();
                        SpinSkillCharge();
                        if (m_fever == false)
                        {
                            FeverGauge();
                            if (m_feverGauge.value == 1.0f)
                            {
                                m_feverButton.GetComponent<Button>().interactable = true;
                            }
                        }
                        else
                        {
                            m_feverButton.GetComponent<Button>().interactable = false;
                        }
                    }
                    break;
            }
        }
    }

    void SetInitalizeRot()
    {
        if (m_stopHandle)
        {
            m_setUpPoint.transform.rotation = Quaternion.Euler(0, 0, m_setUpslider.value);

            SetKinematic(true, false);

            BackandForth();

            PlayTReadyAni();
        }
    }

    public void SetKinematic(bool kinematic, bool skill)
    {
        for (int i = 0; i < m_rgList.Count; i++)
        {
            m_rgList[i].velocity = new Vector3(); // 물리 초기화

            if (!kinematic && skill)
            {
                m_playerScript.ResetTransform(); // TR 초기화
            }

            if (i == 9)
            {
                m_rgList[i].isKinematic = true;
            }
            else
            {
                m_rgList[i].isKinematic = kinematic;
            }
        }
    }

    void BackandForth()
    {
        m_setUpslider.value = Mathf.MoveTowards(m_setUpslider.value, m_value, m_sliderSpeed * Time.deltaTime);

        if (m_setUpslider.value == m_value)
        {
            if (m_setUpslider.value > 0)
            {
                m_value = m_setUpslider.minValue;
            }
            else
            {
                m_value = m_setUpslider.maxValue;
            }
        }
    }

    void PlayTReadyAni()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_stopHandle = false;

            m_tReadyAni.Play("Ani_tReady");

            SliderRange();

            m_count += 1;
        }
    }

    void SliderRange()
    {
        if (m_setUpslider.value >= -m_goodRange && m_setUpslider.value <= m_goodRange)
        {
            m_playerScript.m_moveSpeed = m_goodSpeed;
        }
    }

    void PlayTStartAni()
    {
        if (m_tReadyAni.isPlaying == false)
        {
            m_setUpSliderObject.SetActive(false);

            m_tStartAni.Play("Ani_tStart");

            ResetRotation(m_setUpslider.value);

            m_count += 1;
        }
    }

    void ResetRotation(float sliderValue)
    {
        if (sliderValue >= -m_goodRange && sliderValue <= m_goodRange)
        {
            m_playerScript.m_originCharater.transform.localRotation = Quaternion.identity;
            m_flugFever = true;
        }
        else
        {
            m_flugFever = false;
        }
    }

    void PlayTopMenuAni()
    {
        if (m_tStartAni.isPlaying == false)
        {
            m_topMenuAni.Play("Ani_inGameTopMenu");

            m_playerScript.m_life = true;
            m_tileMng.m_timeStop = true;
            m_tileMng.gameObject.GetComponent<EnemyManager>().m_startBRun = true; // 0327
            m_count += 1;
        }
    }

    public void SetExposure(int value)
    {
        if (m_exposureLevel < 100)
        {
            if (m_exposureLevel + value < 100)
            {
                m_exposureLevel += value;
                m_exposureText.text = m_exposureLevel + "%";

                m_playerScript.PlayEffect();
            }
            else // GameOver
            {
                m_playerScript.PlayEffect();

                m_exposureLevel = 100;
                m_exposureText.text = m_exposureLevel + "%";

                Debug.Log("GameOver");
                m_playerScript.PlayerDead();
                m_inGameMng.GameOver();
            }
            // 2019.02.20  Ani수정
            m_bossUIL.Stop();
            m_bossUIL.Play();
            m_bossUIR.Stop();
            m_bossUIR.Play();
            m_bossFace.Stop();
            m_bossFace.Play();
        }
    }

    //IEnumerator EndExposure()
    //{
    //    yield return new WaitForSeconds(0.3f); // 피격 애니 종료시간
    //    SetKinematic(false, false);
    //}

    //피버 스킬
    void FeverSkillCharge()
    {
        if (m_fever)
        {
            if (m_flugFever) // 피버시 처리
            {
                if (m_timeInFever >= m_feverTime) // 피버종료후 처리(연속), m_feverTime 피버시간
                {
                    if (m_particleMain.loop == true)
                    {
                        SetKinematic(false, true);

                        m_particleMain.loop = false;
                        m_particle.Stop();
                        m_particle.Clear();
                        m_feverTrail.SetActive(false);
                    }

                    m_playerScript.m_moveSpeed = Mathf.MoveTowards(m_playerScript.m_moveSpeed, m_originSpeed, Time.deltaTime * m_timeSpeed); //감속 연속 처리
                }
                else if (m_timeInFever == 0) // 피버 시작시 처리
                {
                    Transform[] currentTrans = m_joint.GetComponentsInChildren<Transform>();

                    for (int i = 0; i < m_prevTransf.Length; i++)
                    {
                        currentTrans[i].localRotation = m_storeQua[i];
                    }

                    SetKinematic(true, true);
                    m_playerScript.m_originCharater.transform.localRotation = Quaternion.identity;
                    m_playerScript.m_ani.speed = 3; // 2019.02.20 이동 수정

                    m_particleMain.loop = true;
                    m_particle.Play();
                    m_feverTrail.SetActive(true);
                    m_timeInFever += Time.deltaTime;
                }
                else
                {
                    m_timeInFever += Time.deltaTime;
                }

                if (m_playerScript.m_moveSpeed == m_originSpeed) // 피버기능 종료
                {
                    m_feverGauge.value = 0.0f;
                    m_fever = false;
                    m_timeInFever = 0.0f;
                }
            }
            else // start 피버 실패시
            {
                SetKinematic(false, false);
                m_fever = false;
            }
        }
    }

    // 피버 게이지
    void FeverGauge()
    {
        float angle;
        angle = m_playerScript.GetAngle();

        if (Mathf.Abs(angle) <= 25)
        {
            m_feverGauge.value += m_feverSV * Time.deltaTime;
        }
    }

    // 피버 버튼
    public void FeverButton()
    {
        if (m_feverGauge.value == 1.0f && m_playerScript.m_attackTrigger == 0 && m_playerScript.m_playerAniArr[0].isPlaying == false)
        {
            m_fever = true;
            m_flugFever = true;

            m_playerScript.m_moveSpeed = 20.0f;
            m_playerScript.m_speedValue = 1.0f;
        }
    }

    void SpinSkillCharge() // SpinAttack
    {
        if (!m_attackTrigger)
        {
            if (m_attackGauge.value == 1.0f) // 스킬 사용준비 완료
            {
                if (m_fever == false)
                {
                    m_attackButton.interactable = true;
                    m_attackTrigger = true;
                }
            }
            else // 스킬 쿨타임 회복
            {
                m_attackGauge.value += m_speanSV * Time.deltaTime;
            }
        }
    }

    public void SpinButton()
    {
        if (!m_fever && m_attackTrigger && m_rgList[0].isKinematic == false)
        {
            m_attackButton.interactable = false;
            m_attackGauge.value = 0;
            m_playerScript.m_attackTrigger = 1;
        }
    }

    // 시간 표시
    public void GameTime()
    {
        m_tileMng.PlayTime();

        if (m_tileMng.m_timeStop == true && m_playerScript.m_life)
        {
            m_timeText.text = "Time: " + string.Format("{00:N2}", m_tileMng.m_time);
        }
    }

    //코인 점수
    public void GetCoin()
    {
        m_coinText.text = TotalCoinScore().ToString();
    }

    public void GetG_Coin()
    {
        m_totalGoldCoin += 1;
        m_totalGoldScore += m_goldScore;
    }

    public void GetS_Coin()
    {
        m_totalSilverCoin += 1;
        m_totalSilverScore += m_silverScore;
    }

    public void GetB_Coin()
    {
        m_totalBronzCoin += 1;
        m_totalBronzScore += m_bronzScore;
    }

    public int TotalCoin()
    {
        return m_totalGoldCoin + m_totalSilverCoin + m_totalBronzCoin;
    }

    public int TotalCoinScore()
    {
        return m_totalGoldScore + m_totalSilverScore + m_totalBronzScore;
    }

}