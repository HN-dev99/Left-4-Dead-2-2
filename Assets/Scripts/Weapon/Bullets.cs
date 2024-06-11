using System;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    private void OnCollisionEnter(Collision objectWeHit)
    {
        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit Wall");

            CreateBulletEffect(objectWeHit);
            Destroy(gameObject);
        }

        else if (objectWeHit.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Hit Ground");
            CreateBulletEffect(objectWeHit);
            Destroy(gameObject);
        }
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
