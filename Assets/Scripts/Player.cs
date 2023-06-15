using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterProperty
{
    public static Player Inst = null;

    float jumpPower = 10.0f;
    public bool IsJumping;
    bool IsCol = false;
    public LayerMask myGround;
    public LayerMask myEnemy;
    curWeap myWeap = curWeap.Hammer;
    curWeap myorgWeap = curWeap.Hammer;
    float shieldPower = 10.0f;
    public GameObject myRange;
    public GameObject[] Jumping = new GameObject[2];
    float range = 0.5f;

    float shieldTime = 0.0f;
    bool IsShield = false;
    float LifeTime = 1.0f;
    float curlifeTime = 0.0f;
    bool IsSuccess = false;

    float skillTime = 10.0f;
    float curskillTime = 0.0f;
    float rushSpeed = 20.0f;
    float rushDist = 20.0f;
    int curskill = 0;
    bool IsOnSkill = false;
    bool IsSecond = false;

    public bool ItemShield = false;

    int skillGage = 0;
    int fullGage = 5;

    public GameObject[] myWeaps = new GameObject[4];
    public GameObject myBubble;
    public GameObject myRush;

    public enum curWeap
    {
        Hammer, Sycthe, Gun, Bow, none
    }

    private void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InGameManager.Inst.changeSkillfill(skillGage, fullGage);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsShield)
        {
            shieldTime += Time.deltaTime;
        }

        if (curlifeTime > LifeTime && myRigid.velocity.y == 0.0f && !IsJumping && IsCol)
        {
            if (ItemShield)
            {
                ItemShield = false;
                myBubble.SetActive(false);
                curlifeTime = 0.0f;
            }
            else
            {
                InGameManager.Inst.changeLife(-1);
                curlifeTime = 0.0f;
            }
        }
        else if (curlifeTime < LifeTime && myRigid.velocity.y == 0.0f && !IsJumping && IsCol)
        {
            curlifeTime += Time.deltaTime;
        }

        if (shieldTime > 3.0f)
        {
            ShieldEnd();
        }

        if (myWeap == curWeap.Gun || myWeap == curWeap.Bow)
        {
            curskillTime += Time.deltaTime;
        }

        if (curskillTime > skillTime)
        {
            curskillTime = 0.0f;
            WeaponChange(myorgWeap);
            IsOnSkill = false;
        }

        if (InGameManager.Inst.curWeapon > InGameManager.Inst.secondWeapon && !IsOnSkill && !IsSecond)
        {
            WeaponChange(curWeap.Sycthe);
            myorgWeap = myWeap;
            IsSecond = true;
        }
    }

    public void haveItemShield()
    {
        if (ItemShield)
        {
            myBubble.SetActive(true);
        }
    }

    public void Jump()
    {
        if (!IsJumping && !IsShield)
        {
            for (int i = 0; i < 2; i++)
            {
                Jumping[i].SetActive(true);
            }
            myAnim.SetTrigger("Jumping");
            IsJumping = true;
            myRigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    public void WeaponChange(curWeap next)
    {
        if (myWeap == next)
            return;

        myWeap = next;
        
        for (int i = 0; i < (int)curWeap.none; i++)
        {
            myWeaps[i].SetActive(false);
        }

        myWeaps[(int)myWeap].SetActive(true);
    }

    public void ShieldStart()
    {
        myAnim.SetTrigger("ShieldStart");
        IsShield = true;

        if (myRigid.velocity.y > -0.1f && !IsSuccess)
        {
            if (shieldTime < 1.0f)
            {
                Collider2D[] list = Physics2D.OverlapCircleAll(myRange.transform.position, range, myEnemy);
                foreach (Collider2D col in list)
                {
                    IBattle ib = col.GetComponent<IBattle>();
                    ib?.KnockBack(shieldPower);
                    Debug.Log("success");
                    IsSuccess = true;
                }
            }
        }
        else
        {
            Collider2D[] list = Physics2D.OverlapCircleAll(myRange.transform.position, range, myEnemy);

            if (list != null)
            {
                IsCol = false;
            }
        }
    }
    
    public void ShieldEnd()
    {
        myAnim.SetTrigger("ShieldEnd");
        IsShield = false;
        shieldTime = 0.0f;
        IsSuccess = false;
    }

    public void Attack()
    {
        if (!myAnim.GetBool("IsAttacking") && !IsShield)
        {
            switch (myWeap)
            {
                case curWeap.Hammer:
                    myAnim.SetTrigger("HammerAttack");
                    Hammer.Inst.Attack();
                    break;

                case curWeap.Sycthe:
                    myAnim.SetTrigger("SyctheAttack");
                    Sycthe.Inst.Attack();
                    break;

                case curWeap.Gun:
                    myAnim.SetTrigger("GunAttack");
                    HandGun.Inst.Attack();
                    IsOnSkill = true;
                    break;

                case curWeap.Bow:
                    myAnim.SetTrigger("BowAttack");
                    Bow.Inst.Attack();
                    IsOnSkill = true;
                    break;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            IsJumping = false;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IsCol = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            IsJumping = true;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IsCol = false;
        }
    }

    public void Damaging(int n)
    {
        Collider2D[] list = Physics2D.OverlapCircleAll(myRange.transform.position, range, myEnemy);

        foreach (Collider2D col in list)
        {
            IBattle ib = col.GetComponent<IBattle>();
            ib?.OnDamage(n);
        }
    }

    public void plusGage(int n)
    {
        skillGage += n;
        if (skillGage > fullGage)
        {
            skillGage = fullGage;
        }

        InGameManager.Inst.changeSkillfill(skillGage, fullGage);
    }

    public void Rush()
    {
        StartCoroutine(Rushing());
    }

    IEnumerator Rushing()
    {
        Vector3 dir = transform.up;

        float dist = rushDist;

        myRush.SetActive(true);

        while (dist > 0.0f)
        {
            Damaging(1);

            float delta = Time.deltaTime * rushSpeed;

            if (delta > dist)
            {
                delta = dist;
            }
            dist -= delta;

            transform.Translate(dir * delta, Space.World);

            yield return null;
        }

        myRush.SetActive(false);
    }

    public void UseHandGun()
    {
        WeaponChange(curWeap.Gun);
    }

    public void UseBow()
    {
        WeaponChange(curWeap.Bow);
    }

    public void useSkill()
    {
        if (skillGage >= fullGage)
        {
            switch (curskill)
            {
                case 0:
                    Rush();
                    break;
                case 1:
                    UseBow();
                    break;
                case 2:
                    UseHandGun();
                    break;
            }
            plusGage(-10);
            curskill++;
            if (curskill > 2)
                curskill = 0;
        }
    }
}
