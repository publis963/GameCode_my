using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FollowType2 { Once, Loop, PingPong, }
public enum FollowDirection2 { Forward, Backward, }

public class PlayerScript : MonoBehaviour
{
    public float m_speedValue;
    public float m_topplePower;

    public GameObject m_originCharater;
    private Transform[] transformArr;
    private List<Vector3> StPosList;
    private List<Quaternion> StRosList;
    public GameObject[] m_aniCharacter;
    public GameObject m_dummyCharacter;
    public Rigidbody m_DummyRig;
    public CharacterJoint m_joint;
    public Animator m_ani;
    public int m_attackTrigger;

    private TileManager m_tileManager;
    private MakingMap m_mapMaker;
    public int m_cutMapCount;

    private TileScript m_underTile;
    private Ray ray;
    public RaycastHit hit;
    public bool m_life;
    public bool m_stayAction;

    // 이동간 좌표, 상수값
    public float m_moveSpeed;
    public float m_sideSpeed;
    public float m_sideWidth;
    public FollowType2 m_followType;
    public FollowDirection2 m_followDirection;
    public Transform m_jointAnchor;

    private GameSetUp m_gameSetUp;
    public Animation m_dumChar2Ani;
    public Animation[] m_playerAniArr;
    public List<GameObject> m_balloonList;
    private GameObject m_target;

    public ParticleSystem m_playerEffect;

    private bool buttonTrigger = true;
        
    void Start()
    {
        m_life = false;
        m_stayAction = false;
        m_speedValue = 1;
        m_attackTrigger = 0;
        m_mapMaker = GameObject.Find("MapMaker").GetComponent<MakingMap>();
        m_gameSetUp = GameObject.Find("GameSetUp").GetComponent<GameSetUp>();

        StPosList = new List<Vector3>();
        StRosList = new List<Quaternion>();

        transformArr = m_originCharater.GetComponentsInChildren<Transform>();

        for (int i = 0; i< transformArr.Length; ++i)
        {
            Vector3 pos = new Vector3();

            pos.x = transformArr[i].localPosition.x;
            pos.y = transformArr[i].localPosition.y;
            pos.z = transformArr[i].localPosition.z;

            Quaternion Ros = new Quaternion();
            Ros.eulerAngles = transformArr[i].localEulerAngles;

            StPosList.Add(pos);
            StRosList.Add(Ros);
        }
    }

    void Update () // 인게임 시작전 딜레이필요
    {
        if (m_life == true)
        {
            CheckTile();
            SpinAttack();
            if (m_gameSetUp.m_fever == false && m_stayAction == false)
            {
                SetSpeed(GetAngle(), m_joint); 
            }
        }

       // SecondCtl();
    }

    void FixedUpdate()
    {
        if (m_life == true)
        {
            if(!m_stayAction)
            {
                SideMove();
            }
            ForwardMove();
        }
    }

    void SpinAttack()
    {
        switch (m_attackTrigger)
        {
            case 0:
                break;

            case 1:
                m_stayAction = true; // 스킬 동작 유무
                m_gameSetUp.SetKinematic(true, true);
                transform.Find("JointAnchor").transform.Find("OriginCharacter").transform.Find("CH_BusinessMan").gameObject.SetActive(false);
                m_aniCharacter[3].SetActive(true);
                m_attackTrigger = 2;
                break;

            case 2:
                if (m_aniCharacter[3].GetComponent<Animation>().isPlaying == false)
                {
                    m_attackTrigger = 3;
                }
                break;

            case 3:
                m_aniCharacter[3].SetActive(false);
                transform.Find("JointAnchor").transform.Find("OriginCharacter").transform.Find("CH_BusinessMan").gameObject.SetActive(true);
                m_gameSetUp.SetKinematic(false, true);
                m_stayAction = false;
                m_gameSetUp.m_attackTrigger = false;
                m_attackTrigger = 0; 
                break;

        }
    }

    public void Jump()
    {
        if (m_gameSetUp.m_rgList[0].useGravity == true)
        {
            m_dumChar2Ani.Play("Ani_dumChar2Jump");
        }
    }

    void ForwardMove() // 직진 이동
    {
        float moveValue;

        if (m_attackTrigger == 2)
        {
            moveValue = 15;

        }
        else
        {
            moveValue = m_moveSpeed / 2 + m_moveSpeed * m_speedValue;
        }
        transform.Translate(transform.forward * moveValue * Time.deltaTime);
    }

    void SideMove() // 좌우 이동
    {
        float playerAngle = GetAngle();
        Vector3 sidePos = new Vector3();

        sidePos = new Vector3(m_sideSpeed, 0, 0) * Time.deltaTime * -playerAngle / 10;

        sidePos = m_jointAnchor.localPosition + sidePos;

        if (sidePos.x <= m_sideWidth && sidePos.x >= -m_sideWidth)
        {
            m_jointAnchor.localPosition = sidePos;
        }
    }

    void CheckAngle(CharacterJoint joint)
    {
        float playerAngle = GetAngle();

        if (playerAngle <= joint.lowTwistLimit.limit || playerAngle >= joint.highTwistLimit.limit)
        {
            //PlayerDead();
        }
    }

    void CheckTile()
    {
        ray = new Ray(this.transform.position + new Vector3(0, 1, 0) + (this.transform.forward * 0.1f), new Vector3(0, -1, 0));
        m_tileManager = m_mapMaker.m_makedMaps[0].GetComponent<TileManager>();

        if (Physics.Raycast(ray, out hit))
        {
            if (m_underTile != hit.transform.GetComponent<TileScript>() || m_underTile == null)
            {
                if (m_tileManager.m_tileList.Count > m_tileManager.m_tileList.Count - 1)
                {
                    if (m_tileManager.m_tileList.IndexOf(hit.transform.gameObject) == m_cutMapCount)
                    {
                        m_tileManager.DeletePreviousTile();
                    }
                }
            }
        }
    }

    public void PlayerDead()
    {
        Transform[] dummyTransformArr;
        m_life = false;
        Debug.Log("Player Dead");

        transformArr = m_originCharater.GetComponentsInChildren<Transform>();
        dummyTransformArr = m_aniCharacter[1].GetComponentsInChildren<Transform>();

        for (int i = 0; i < dummyTransformArr.Length; ++i)
        {
            dummyTransformArr[i].transform.SetPositionAndRotation(transformArr[i].transform.position, transformArr[i].transform.rotation);
        }

        m_originCharater.gameObject.SetActive(false); // origin 비활성화
        m_aniCharacter[1].SetActive(true); // dummy 활성화
    }

    public void SetSpeed(float angleZ, CharacterJoint joint)
    {
        angleZ = Mathf.Abs(angleZ);
        m_speedValue = 1 - angleZ / joint.highTwistLimit.limit;

        m_ani.speed = 1.0f + (0.5f * m_speedValue); // Animation 재생속도
    }

    public float GetAngle()
    {
        float angle = 0;

        if (m_originCharater.transform.eulerAngles.z > 180)
        {
            angle = m_originCharater.transform.eulerAngles.z - 360;
        }
        else
        {
            angle = m_originCharater.transform.eulerAngles.z;
        }

        return angle;
    }

    //public void SetTarget(GameObject _target)
    //{
    //    m_target = _target;
    //}

    public void ResetTransform()
    {
        for (int i = 0; i< transformArr.Length; ++i)
        {
            transformArr[i].localPosition = StPosList[i];
            transformArr[i].localEulerAngles = StRosList[i].eulerAngles;
        }
    }

    public void PlayEffect()
    {
        m_playerEffect.Stop();
        m_playerEffect.Play();

        m_playerAniArr[0].Stop();
        m_playerAniArr[0].Play();

        m_playerAniArr[1].Stop();
        m_playerAniArr[1].Play();
    }

    //public void SecondCtl()
    //{
    //    float LR = Input.GetAxisRaw("Horizontal");
    //    float UD = Input.GetAxisRaw("Vertical");

    //    if (LR != 0)
    //    {
    //        m_originCharater.transform.localEulerAngles = new Vector3(0, 0, 30 * LR * -1);

    //        if (LR == 1)
    //        {
    //            m_originCharater.GetComponent<CharacterControl>().m_LeftRig.velocity += new Vector3(0, 3, 0);
    //        }
    //        else
    //        {
    //            m_originCharater.GetComponent<CharacterControl>().m_RightRig.velocity += new Vector3(0, 3, 0);
    //        }
    //        Debug.Log("좌우 처리");
    //    }
        
    //    if(UD != 0)
    //    {
    //        m_originCharater.transform.localEulerAngles = new Vector3(0, 0, 0);
    //        Debug.Log("업 처리");
    //    }

    //}
}
