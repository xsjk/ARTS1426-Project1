using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCountLogic : MonoBehaviour
{

    private int m_playerCount;
    public int playerCount {
        get { return m_playerCount; }
        set {
            if (m_playerCount != value) {
                m_playerCount = value;
                animator.SetBool("isPlaying", m_playerCount != 0);
            }
        }
    }

    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }


}
