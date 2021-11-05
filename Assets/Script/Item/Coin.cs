using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 코인 자체 관련
public class Coin : MonoBehaviour
{
    private AudioSource m_coinAudio;
    public ParticleSystem m_particle;
    private GameSetUp m_gameSet;

    void Start ()
    {
        m_coinAudio = GameObject.Find("Coin").GetComponent<AudioSource>();
        m_gameSet = GameObject.Find("GameSetUp").GetComponent<GameSetUp>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player") // 플레이어와 부딪혔을 때
        {
            Instantiate(m_particle, transform.position + new Vector3(0, 1, 0), Quaternion.identity, transform.parent);

            m_particle.Play();
            m_coinAudio.Play();
            m_gameSet.GetCoin();

            // 코인 스코어
            if (this.tag == "G_SliverCoin")
            {
                m_gameSet.GetS_Coin();
            }
            else if (this.tag == "G_BronzeCoin")
            {
                m_gameSet.GetB_Coin();
            }
            else if (this.tag == "G_GoldCoin")
            {
                m_gameSet.GetG_Coin();
            }

            Destroy(gameObject);
        }
    }
}
