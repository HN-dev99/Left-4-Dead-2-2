using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullets : MonoBehaviour
{

    public int bulletDamage;
    private ObjectPool bulletPool;

    private void Start()
    {
        bulletPool = FindObjectOfType<ObjectPool>();
    }
    private void OnCollisionEnter(Collision objectWeHit)
    {

        if (objectWeHit.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Hit Ground");
            CreateBulletEffect(objectWeHit);
            bulletPool.ReturnBulletToPool(gameObject);
        }

        if (objectWeHit.gameObject.CompareTag("Enemy"))
        {
            CreateBloodEffect(objectWeHit);
            bulletPool.ReturnBulletToPool(gameObject);
            if (objectWeHit.gameObject.GetComponent<Enemy>().isDead == false)
            {
                objectWeHit.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            }

        }
    }

    private void CreateBloodEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject bloodSprayPrefab = Instantiate(
            GlobalReference.Instance.bloodSprayEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform);
    }

    private void CreateBulletEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject hole = Instantiate(
            GlobalReference.Instance.bulletEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }

}
