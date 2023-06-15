using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float bulletSpeed = 5.0f;

    float lifeTime = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fire());
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator fire()
    {
        Vector3 dir = transform.up;

        while (true)
        {
            float delta = Time.deltaTime * bulletSpeed;

            transform.Translate(dir * delta, Space.World);

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IBattle ib = collision.GetComponent<IBattle>();
            ib?.OnDamage(1);

            Destroy(gameObject);
        }
    }
}
