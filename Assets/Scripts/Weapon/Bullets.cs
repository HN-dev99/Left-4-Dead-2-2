using System;
using UnityEngine;

public class Bullets : MonoBehaviour
{

    public int bulletDamage;
    private void OnCollisionEnter(Collision objectWeHit)
    {
        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit Wall");

            CreateBulletEffect(objectWeHit);
            Destroy(gameObject);
        }

        if (objectWeHit.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Hit Ground");
            CreateBulletEffect(objectWeHit);
            Destroy(gameObject);
        }

        if (objectWeHit.gameObject.CompareTag("Enemy") && objectWeHit.gameObject.GetComponent<Enemy>().isDead == false)
        {
            objectWeHit.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            CreateBloodEffect(objectWeHit);
            Destroy(gameObject);
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
