using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattle
{
    Transform transform
    {
        get;
    }

    void OnDamage(int n);

    void KnockBack(float shieldP);
}
