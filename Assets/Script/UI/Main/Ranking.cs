using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Firebase.Unity.Editor;
using Firebase.Database;
using Firebase;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;

public class Ranking : MonoBehaviour
{
    public Text m_results;
    public GameObject m_ContentsParent;
    public Text m_rankResult; 

    private List<User> m_LeaderUsers;
    private List<Text> m_LeaderText;

    private User m_showCurrntRanking;
    private bool m_checkOnce = true;

    void Start()
    {
        ShowUserRank();
        DoRefresh(1);
    }

    public void RefeshButton()
    {
        if (m_checkOnce == true)
        {
            m_checkOnce = false;
            ShowUserRank();
            DoRefresh(0);
        }
    }
    private void DoRefresh(int first)
    { 
        FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith(task =>
        {
            if (first == 0)
            { 
                for (int i = 0; i < m_LeaderText.Count; i++)
                {
                    Destroy(m_LeaderText[i].gameObject);
                }
            }
            
            
            m_LeaderUsers = new List<User>();
            m_LeaderText = new List<Text>();

            DataSnapshot snapshot = task.Result;


            foreach (DataSnapshot user in snapshot.Children)
            {
                IDictionary dictUser = (IDictionary)user.Value;

                User temp = new User();

                temp.Score = Convert.ToInt32(dictUser["Score"]);
                temp.UserType = dictUser["UserType"].ToString();
                temp.NickName = dictUser["NickName"].ToString();
                temp.UID = dictUser["UID"].ToString();
                m_LeaderUsers.Add(temp);
            }

            m_LeaderUsers.Sort(delegate (User A, User B)
            {
                if (A.Score < B.Score) return 1;
                else if (A.Score > B.Score) return -1;
                return 0;
            });
            
            for (int i = 0; i < 10; i++)
            {
                Text tempText = Instantiate(m_results, m_ContentsParent.transform, false);
                tempText.text = string.Format("  NO.{0}\r\n  NickName:{1}\r\n  Score:{2}", i+1, m_LeaderUsers[i].NickName, m_LeaderUsers[i].Score);
                m_LeaderText.Add(tempText);
            }

            if (m_showCurrntRanking != null)
            {
                int index = m_LeaderUsers.FindIndex(r => r.UID == m_showCurrntRanking.UID);

                m_rankResult.text = string.Format("Your Rank: {0}\r\nScore: {1}", index + 1, m_showCurrntRanking.Score);
            }
            m_checkOnce = true;
        });
    }
    public void ShowUserRank()
    {
        m_showCurrntRanking = Static.m_currentUser;
    }
    public void Delete()
    {
        this.gameObject.SetActive(false);
    }
}