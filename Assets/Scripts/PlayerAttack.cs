using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int atk;
    public int acn;
    public int ap;
    private float time;
    public Animator animator;
    public AnimatorStateInfo stateInfo;
    private bool isAnimEnd;
    private Rigidbody2D myRigidbody2D;
    
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {

        IsAniEnd();
        Animt();
        Attack();
        DoNotMove();
    }

    void DoNotMove()
    {
        
    }

    void IsAniEnd()
    {

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        if (stateInfo.IsName("ATK1")||stateInfo.IsName("ATK2")||stateInfo.IsName("ATK3"))
        {
            Debug.Log(stateInfo.normalizedTime);
            if (stateInfo.normalizedTime <= 0.95f)
            {
                isAnimEnd = false;
                return;
            }
            
        }

        isAnimEnd = true;

        


    }

    void Attack()
    {
        AttackSpeed();
        if (ap==1)
        {
            if (Input.GetButtonDown("Fire1")&& isAnimEnd)
            {
                acn++;
                atk = acn;
                ap = 0;

            }
            else
            {
                atk = 0;
            }

            if (acn>=3)
            {
                acn = 0;
            }
        }


        
        
        
        
    }

    void AttackSpeed()
    {
        if (ap==0)
        {
            time += Time.deltaTime;
            if (time>=0.1f)
            {
                ap = 1;
                time = 0;
            }
            
        }
    }

    void Animt()
    {
        animator.SetInteger("atk",atk);
        
    }
}
