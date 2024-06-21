using System.Collections.Generic;
using UnityEngine;


public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }
    public List<GameObject> weaponSlots;
    public GameObject weaponActiveSlot;

    [Header("Ammo")]
    public int totalGreenAmmo = 0;
    public int totalRedAmmo = 0;

    [Header("Throwable")]

    public float throwForce = 40f;

    public GameObject throwableSpawn;
    public float forceMutiplier = 0f;
    public float forceMutiplierLimit = 2f;

    [Header("Lethals")]
    public int maxLethal = 2;
    public int lethalCount = 0;
    public Throwable.ThrowableType equippedLethalType;
    public GameObject grenadePrefab;

    [Header("Tactical")]
    public int maxTactical = 2;
    public int tacticalCount = 0;
    public Throwable.ThrowableType equippedTacticalType;
    public GameObject smokeGrenadePrefab;

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

    private void Start()
    {
        weaponActiveSlot = weaponSlots[0];

        equippedLethalType = Throwable.ThrowableType.None;
        equippedTacticalType = Throwable.ThrowableType.None;
    }


    private void Update()
    {
        //Check ActiveSlot
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == weaponActiveSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        //Change Gun
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

        //Throwable
        if (Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.T))
        {
            forceMutiplier += Time.deltaTime;
            if (forceMutiplier > forceMutiplierLimit)
            {
                forceMutiplier = forceMutiplierLimit;
            }

        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            if (lethalCount > 0)
            {
                ThrowLethal();
            }
            forceMutiplier = 0;
        }

        else if (Input.GetKeyUp(KeyCode.T))
        {
            if (tacticalCount > 0)
            {
                ThrowTactical();
            }
            forceMutiplier = 0;
        }
    }


    public void PickupWeapon(GameObject pickedupWeapon)
    {
        DropCurrentWeapon(pickedupWeapon);
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }



    #region || ---- Throwables ---- ||
    public void PickupThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.ThrowableType.Grenade:

                PickUpThrowableAsLethal(Throwable.ThrowableType.Grenade);
                break;

            case Throwable.ThrowableType.Smoke_Grenade:

                PickUpThrowableAsTactical(Throwable.ThrowableType.Smoke_Grenade);
                break;
        }
    }

    private void PickUpThrowableAsTactical(Throwable.ThrowableType tactical)
    {
        if (equippedTacticalType == tactical || equippedTacticalType == Throwable.ThrowableType.None)
        {
            equippedTacticalType = tactical;

            if (tacticalCount < maxTactical)
            {
                tacticalCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HudManager.Instance.UpdateThrowablesUI();
            }
            else
            {
                print("Tactical limit reached");
            }
        }
    }

    private void PickUpThrowableAsLethal(Throwable.ThrowableType lethal)
    {
        if (equippedLethalType == lethal || equippedLethalType == Throwable.ThrowableType.None)
        {
            equippedLethalType = lethal;

            if (lethalCount < maxLethal)
            {
                lethalCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HudManager.Instance.UpdateThrowablesUI();
            }
            else
            {
                print("Lethals limit reached");
            }
        }
    }


    private void ThrowLethal()
    {
        GameObject lethalPrefab = GetThrowablePrefab(equippedLethalType);

        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMutiplier), ForceMode.Impulse);
        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        lethalCount -= 1;

        if (lethalCount <= 0)
        {
            equippedLethalType = Throwable.ThrowableType.None;
        }

        HudManager.Instance.UpdateThrowablesUI();

    }

    private void ThrowTactical()
    {
        GameObject taticalPrefab = GetThrowablePrefab(equippedTacticalType);

        GameObject throwable = Instantiate(taticalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMutiplier), ForceMode.Impulse);
        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        tacticalCount -= 1;

        if (tacticalCount <= 0)
        {
            equippedTacticalType = Throwable.ThrowableType.None;
        }

        HudManager.Instance.UpdateThrowablesUI();

    }


    private GameObject GetThrowablePrefab(Throwable.ThrowableType throwableType)
    {
        switch (throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                return grenadePrefab;

            case Throwable.ThrowableType.Smoke_Grenade:
                return smokeGrenadePrefab;

        }
        return new();
    }
    #endregion

    // Weapon
    private void AddWeaponIntoActiveSlot(GameObject pickedupWeapon)
    {
        pickedupWeapon.transform.SetParent(weaponActiveSlot.transform, false);

        Weapon weapon = pickedupWeapon.GetComponent<Weapon>();

        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if (weaponActiveSlot.transform.childCount > 0)
        {
            Weapon weaponToDrop = weaponActiveSlot.transform.GetChild(0).GetComponent<Weapon>();
            weaponToDrop.isActiveWeapon = false;
            weaponToDrop.animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedupWeapon.transform.parent);

            weaponToDrop.transform.localPosition = pickedupWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedupWeapon.transform.localRotation;
        }
    }

    private void SwitchActiveSlot(int number)
    {
        if (weaponActiveSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = weaponActiveSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }
        weaponActiveSlot = weaponSlots[number];

        if (weaponActiveSlot.transform.childCount > 0)
        {
            Weapon newWeapon = weaponActiveSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }

    }

    //Ammobox
    public void PickupAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.GreenLazerGun:
                totalGreenAmmo += ammo.ammoAmount;
                break;

            case AmmoBox.AmmoType.RedLazerGun:
                totalRedAmmo += ammo.ammoAmount;
                break;
        }
    }

    public void DecreaseTotalAmmo(int bulletToDecrease, Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.GreenLazerGun:
                totalGreenAmmo -= bulletToDecrease;
                break;


            case Weapon.WeaponModel.RedLazerGun:
                totalRedAmmo -= bulletToDecrease;
                break;
        }
    }

    public int CheckAmmoLeftFor(Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.GreenLazerGun:
                return totalGreenAmmo;

            case Weapon.WeaponModel.RedLazerGun:
                return totalRedAmmo;

            default:
                return 0;
        }
    }


}

