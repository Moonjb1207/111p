using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterProperty, IBattle
{
    public float speed = 0.5f;
    public bool IsStop = false;
    public bool shield = false;
    
    public void OnDamage(int n)
    {
        if (n == 0)
            Player.Inst.plusGage(1);

        InGameManager.Inst.changeScore(5);
        InGameManager.Inst.curGen.RemoveAt(0);
        if (InGameManager.Inst.curGen.Count != 0)
            InGameManager.Inst.curGen[0].GetComponentInChildren<Enemy>().gameObject.layer = 10;
        Destroy(gameObject);

        int rnd = Random.Range(0, 2);

        GameObject obj = Instantiate(Resources.Load("Prefabs/Item") as GameObject);
        obj.GetComponent<Item>().createItem((Item.itemtype)rnd);

        obj.GetComponent<Item>().giveItem();
    }

    public void KnockBack(float shieldP)
    {
        Player.Inst.plusGage(2);

        for (int i = 0; i < InGameManager.Inst.curGen.Count; i++)
        {
            InGameManager.Inst.curGen[i].GetComponentInChildren<Enemy>().shield = true;
            InGameManager.Inst.curGen[i].GetComponentInChildren<Enemy>().speed -= 1.0f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Falling());
    }

    // Update is called once per frame
    void Update()
    {
        speed += Time.deltaTime;

        if (speed >= 0)
        {
            shield = false;
        }
    }

    IEnumerator Falling()
    {
        Vector3 dir = -transform.up;

        while (true)
        {
            float delta = Time.deltaTime * speed;

            transform.Translate(dir * delta, Space.World);

            yield return new WaitWhile(isStop);
        }
    }

    bool isStop()
    {
        return IsStop;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !shield)
        {
            for (int i = 0; i < InGameManager.Inst.curGen.Count; i++)
            {
                InGameManager.Inst.curGen[i].GetComponentInChildren<Enemy>().IsStop = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !shield)
        {
            for (int i = 0; i < InGameManager.Inst.curGen.Count; i++)
            {
                InGameManager.Inst.curGen[i].GetComponentInChildren<Enemy>().IsStop = false;
                InGameManager.Inst.curGen[i].GetComponentInChildren<Enemy>().speed = 0.5f;
            }
        }
    }
}
