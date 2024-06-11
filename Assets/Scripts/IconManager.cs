using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : MonoBehaviour
{
    public static Sprite Green_Weapon;
    public static Sprite Red_Weapon;
    public static Sprite Green_Ammo;
    public static Sprite Red_Ammo;
    public static IconManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        Green_Weapon = Resources.Load<GameObject>("Green_Weapon").GetComponent<SpriteRenderer>().sprite;
        Red_Weapon = Resources.Load<GameObject>("Red_Weapon").GetComponent<SpriteRenderer>().sprite;
        Green_Ammo = Resources.Load<GameObject>("Green_Ammo").GetComponent<SpriteRenderer>().sprite;
        Red_Ammo = Resources.Load<GameObject>("Red_Ammo").GetComponent<SpriteRenderer>().sprite;
    }
}
