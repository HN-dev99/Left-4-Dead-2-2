
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;

    public static HudManager Instance { get; private set; }
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
    }

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.weaponActiveSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletLeft / activeWeapon.bulletPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);
            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }

        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;
            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;

            if (WeaponManager.Instance.lethalCount <= 0)
            {
                lethalUI.sprite = emptySlot;
            }

            if (WeaponManager.Instance.tacticalCount <= 0)
            {
                tacticalUI.sprite = emptySlot;
            }
        }
    }


    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.weaponActiveSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.GreenLazerGun:
                return IconManager.Green_Ammo;

            case Weapon.WeaponModel.RedLazerGun:
                return IconManager.Red_Ammo;

            default:
                return null;
        }
    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.GreenLazerGun:
                return IconManager.Green_Weapon;

            case Weapon.WeaponModel.RedLazerGun:
                return IconManager.Red_Weapon;

            default:
                return null;
        }
    }

    public void UpdateThrowablesUI()
    {
        lethalAmountUI.text = $"{WeaponManager.Instance.lethalCount}";
        tacticalAmountUI.text = $"{WeaponManager.Instance.tacticalCount}";

        switch (WeaponManager.Instance.equippedLethalType)
        {
            case Throwable.ThrowableType.Grenade:
                lethalUI.sprite = IconManager.grenade;
                break;
        }

        switch (WeaponManager.Instance.equippedTacticalType)
        {
            case Throwable.ThrowableType.Smoke_Grenade:
                tacticalUI.sprite = IconManager.smokeGrenade;
                break;
        }
    }
}


