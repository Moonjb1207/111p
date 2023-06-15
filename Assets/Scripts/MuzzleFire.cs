using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFire : CharacterProperty
{
    public static MuzzleFire Inst = null;

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
        }
    }
}
