using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum tileType { Front, Left, Right };

public class TileScript : MonoBehaviour
{
    public Transform m_spawnPoint; // Enemy 생성 기준 좌표
    private bool m_firstTile;

    public TileManager m_tManager;
    public GameObject[] m_enemyBossArr; 
    public GameObject[] m_enemyCoffeArr;
    public GameObject[] m_enemyBoxArr;
    public GameObject[] m_enemyDeskArr;
    public int m_maxPosX; // x축 추가 좌표 
    public int m_spawnPercentage;// 100 퍼 기준 생성

    // 기둥 셋필드
    public List<Material> m_mtList;
    public Renderer m_LPillar;
    public Renderer m_RPillar;

    // 코인
    public int m_totalCoins;
    public int m_minCoins;
    public int m_maxCoins;
    private int m_choosePos; // *********
    private int m_cutCoinCount; // *******
    public float m_coinx; // *****
    public GameObject m_goldCoin; // *****
    public GameObject m_sliverCoin; // *****
    public GameObject m_bronzeCoin; // *****
    public float m_initgold; // ******
    public float m_initsliv; // ******
    private float m_percent; // ******

    // Level OB
    public GameObject m_levelBoard;
    public Sprite[] m_spriteArr;
    public Material[] m_materiaArr;
    public SpriteRenderer m_levelSprite;
    public MeshRenderer m_levelMaterial;

    //void Start ()
    //{
    //    m_tManager = GetComponentInParent<TileManager>();
    //    m_spawnPercentage = m_tManager.GetSpawnPercentage();

    //    if (m_tManager.GetCountSt() < m_tManager.GetDefaultSt())
    //    {
    //        m_spawnPercentage = 0;
    //    }
    //    if (transform.parent.GetComponent<TileManager>().m_tileList[0] == this.gameObject)
    //    {
    //        m_firstTile = true;
    //    }
    //    else
    //    {
    //        m_firstTile = false;
    //    }

    //    SetPillar(); // Pillar 설정
    //    SetStraightCoin(); // 코인 생성
    //}

    //public void CreateEnemy()
    //{
    //    int num = Random.Range(0, 100);
    //    int selectEnemy = Random.Range(0, m_enemyBossArr.Length);
    //    int xPos = Random.Range(-m_maxPosX, m_maxPosX);

    //    Vector3 newPos = new Vector3(xPos, 0, 0);

    //    if (m_tManager.GetCountSt() > m_tManager.GetDefaultSt())
    //    {
    //        // 0 Level 설정
    //        if (num <= m_tManager.m_gameLeverList[0].enemyBossPercentage) // Boss 
    //        {
    //            xPos = Random.Range(-m_maxPosX, m_maxPosX);
    //            newPos = new Vector3(xPos, 0, 0);
    //            Instantiate(m_enemyBossArr[selectEnemy], m_spawnPoint.position + newPos, Quaternion.Euler(0, 180f, 0), this.transform);
    //        }

    //        if (num <= m_tManager.m_gameLeverList[0].enemyCoffeePercentage) // Coffe
    //        {
    //            xPos = Random.Range(-m_maxPosX, m_maxPosX);
    //            newPos = new Vector3(xPos, 0, 0);
    //            Instantiate(m_enemyCoffeArr[selectEnemy], m_spawnPoint.position + newPos, Quaternion.Euler(0, 180f, 0), this.transform);
    //        }

    //        if (num <= m_tManager.m_gameLeverList[0].enemyBoxPercentage) // Box
    //        {
    //            xPos = Random.Range(-m_maxPosX, m_maxPosX);
    //            newPos = new Vector3(xPos, 0, 0);
    //            Instantiate(m_enemyBoxArr[selectEnemy], m_spawnPoint.position + newPos, Quaternion.Euler(0, 180f, 0), this.transform);
    //        }
    //    }
    //}

    //private void SetEnemy()
    //{
    //    int percentage;

    //    for (int i = 0; i < m_enemyDeskArr.Length; ++i)
    //    {
    //        percentage = Random.Range(1, 100);

    //        if (percentage <= m_tManager.m_gameLeverList[0].enemyLoopChairPercentage) // 0 ~ 10
    //        {
    //            if (i < 3)
    //            {
    //                m_enemyDeskArr[i].transform.Find("EnemyLoopMen").gameObject.SetActive(true);

    //                m_enemyDeskArr[i + 3].transform.Find("EnemyChiar").gameObject.SetActive(false);
    //                m_enemyDeskArr[i + 3].transform.Find("EnemyChiarMen").gameObject.SetActive(false);
    //                m_enemyDeskArr[i + 3].transform.Find("EnemyLoopMen").gameObject.SetActive(false);
    //            }
    //            else
    //            {
    //                m_enemyDeskArr[i].transform.Find("EnemyLoopMen").gameObject.SetActive(true);

    //                m_enemyDeskArr[i - 3].transform.Find("EnemyChiar").gameObject.SetActive(false);
    //                m_enemyDeskArr[i - 3].transform.Find("EnemyChiarMen").gameObject.SetActive(false);
    //                m_enemyDeskArr[i - 3].transform.Find("EnemyLoopMen").gameObject.SetActive(false);
    //            }
    //        }
    //        else if (percentage > m_tManager.m_gameLeverList[0].enemyLoopChairPercentage && percentage <= m_tManager.m_gameLeverList[0].enemyChairPercentage)// 10 ~ 30
    //        {
    //            if (i < 3)
    //            {
    //                if (!m_enemyDeskArr[i].transform.Find("EnemyLoopMen").gameObject.activeSelf &&
    //                    !m_enemyDeskArr[i + 3].transform.Find("EnemyLoopMen").gameObject.activeSelf)
    //                {
    //                    m_enemyDeskArr[i].transform.Find("EnemyChiarMen").gameObject.SetActive(true);
    //                }
    //            }
    //            else
    //            {
    //                if (!m_enemyDeskArr[i].transform.Find("EnemyLoopMen").gameObject.activeSelf &&
    //                    !m_enemyDeskArr[i - 3].transform.Find("EnemyLoopMen").gameObject.activeSelf)
    //                {
    //                    m_enemyDeskArr[i].transform.Find("EnemyChiarMen").gameObject.SetActive(true);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (i < 3)
    //            {
    //                if (!m_enemyDeskArr[i].transform.Find("EnemyLoopMen").gameObject.activeSelf &&
    //                    !m_enemyDeskArr[i + 3].transform.Find("EnemyLoopMen").gameObject.activeSelf)
    //                {
    //                    m_enemyDeskArr[i].transform.Find("EnemyChiar").gameObject.SetActive(true);
    //                }
    //            }
    //            else
    //            {
    //                if (!m_enemyDeskArr[i].transform.Find("EnemyLoopMen").gameObject.activeSelf &&
    //                    !m_enemyDeskArr[i - 3].transform.Find("EnemyLoopMen").gameObject.activeSelf)
    //                {
    //                    m_enemyDeskArr[i].transform.Find("EnemyChiar").gameObject.SetActive(true);
    //                }
    //            }
    //        }
    //    }
    //}

    //private void SetPillar()
    //{
    //    int setNum = Random.Range(0, m_mtList.Count);
    //    m_LPillar.material = m_mtList[setNum];
    //    setNum = Random.Range(0, m_mtList.Count);
    //    m_RPillar.material = m_mtList[setNum];
    //}

    public void SetStraightCoin()
    {
        m_cutCoinCount = Random.Range(m_minCoins, m_maxCoins);

        float totalLength = 30.0f;
        float gap = totalLength / m_totalCoins;
        float posz = transform.localPosition.z - 15;

        float xrand = 0;

        if (m_tManager.GetCountSt() > m_tManager.GetDefaultSt())//2019.03.22
        {
            for (int a = 1; a <= m_totalCoins; a++)
            {
                if (a != 0)
                {
                    Vector3 pos;

                    if ((a - 3) % m_cutCoinCount == 0)
                    {
                        if (m_choosePos == 0)
                        {
                            xrand = Random.Range(0, 5);
                            m_choosePos = Random.Range(0, 2);
                        }
                        else
                        {
                            xrand = Random.Range(-5, 0);
                            m_choosePos = Random.Range(0, 2);
                        }
                    }

                    float posx = xrand;

                    if (m_choosePos == 0)
                    {
                        pos = new Vector3(posx, 1f, posz);
                        posz += gap;
                    }
                    else
                    {
                        pos = new Vector3(posx, 1f, posz);
                        posz += gap;
                    }

                    int rand = Random.Range(0, 100);

                    float gold = m_initgold + m_percent;
                    float sliver = m_initsliv + m_percent;

                    if (rand < gold)
                    {
                        Instantiate(m_goldCoin, pos, Quaternion.identity, gameObject.transform.Find("MoneyObject"));
                    }
                    else if (rand >= gold && rand < sliver)
                    {
                        Instantiate(m_sliverCoin, pos, Quaternion.identity, gameObject.transform.Find("MoneyObject"));
                    }
                    else if (rand >= sliver)
                    {
                        Instantiate(m_bronzeCoin, pos, Quaternion.identity, gameObject.transform.Find("MoneyObject"));
                    }
                }

            }
        }
    }

    //public void SetBoard(int level)
    //{
    //    m_levelSprite.sprite = m_spriteArr[level - 1];
    //    m_levelMaterial.material = m_materiaArr[level - 1];
    //    m_levelBoard.SetActive(true);
    //}
}
