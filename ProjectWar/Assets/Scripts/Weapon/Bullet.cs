using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision objectWeHit)
    {
        if (objectWeHit.gameObject.CompareTag("Target"))
        {
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }
        
        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }
    }

    void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject hole = Instantiate(References.Instance.bulletImpactEffectPrefab, 
            contact.point,
            Quaternion.LookRotation(contact.normal));
        
        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }
}
