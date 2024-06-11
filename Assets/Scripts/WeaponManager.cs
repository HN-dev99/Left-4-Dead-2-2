using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;
    public List<GameObject> weaponSlots;
    public GameObject weaponSlotActive;



    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        weaponSlotActive = weaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            weaponSlot.SetActive(weaponSlot == weaponSlotActive ? true : false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }

    }

    internal void PickupWeapon(GameObject pickedupWeapon)
    {
        DropCurrentWeapon(pickedupWeapon);
        AddWeaponPickedup(pickedupWeapon);
    }



    private void AddWeaponPickedup(GameObject pickedupWeapon)
    {
        pickedupWeapon.transform.SetParent(weaponSlotActive.transform, false);

        Weapon weapon = pickedupWeapon.GetComponent<Weapon>();

        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnPosition.y, weapon.spawnPosition.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if (weaponSlotActive.transform.childCount > 0)
        {
            Weapon currentWeapon = weaponSlotActive.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
            currentWeapon.animator.enabled = false;

            currentWeapon.transform.SetParent(pickedupWeapon.transform.parent);

            currentWeapon.transform.localPosition = pickedupWeapon.transform.localPosition;
            currentWeapon.transform.localRotation = pickedupWeapon.transform.localRotation;
        }
    }

    private void SwitchWeapon(int number)
    {
        if (weaponSlotActive.transform.childCount > 0)
        {
            Weapon currentWeapon = weaponSlotActive.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
            currentWeapon.animator.enabled = false;
        }
        weaponSlotActive = weaponSlots[number];
        if (weaponSlotActive.transform.childCount > 0)
        {
            Weapon newWeapon = weaponSlotActive.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
            newWeapon.animator.enabled = true;
        }


    }
}
