using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum itemtype
    {
        Shield, Potion, none
    }

    public itemtype myType = itemtype.none;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createItem(itemtype n)
    {
        myType = n;
    }

    public void giveItem()
    {
        if (!Player.Inst.ItemShield)
        {
            switch (myType)
            {
                case itemtype.Shield:
                    Player.Inst.ItemShield = true;
                    Player.Inst.haveItemShield();
                    break;
                case itemtype.Potion:
                    InGameManager.Inst.changeLife(1);
                    break;
            }
        }
        Destroy(gameObject);
    }
}
