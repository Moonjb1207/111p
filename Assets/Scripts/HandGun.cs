using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : CharacterProperty
{
    public static HandGun Inst = null;
    public GameObject startPos;

    private void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack()
    {
        if (!myAnim.GetBool("IsAttacking"))
        {
            myAnim.SetTrigger("Attacking");
            MuzzleFire.Inst.myAnim.SetTrigger("Attacking");
            Instantiate(Resources.Load("Prefabs/bullet") as GameObject, startPos.transform.position, Quaternion.identity);
        }
    }
}
