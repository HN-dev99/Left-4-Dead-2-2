using System;
using System.Collections;

using UnityEngine;

public class Weapon : MonoBehaviour
{

    public bool isActiveWeapon;

    public Animator animator;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 500f;
    [SerializeField] private float bulletLifeTime = 3f;
    [SerializeField] private float shootingDelay = 2f;
    [SerializeField] private Transform bulletSpawn;

    [SerializeField] private float spreadIntensity = 0.2f;


    [Header("Shooting")]
    bool isShooting;
    bool readyToShoot;

    [Header("Reload")]
    bool isReloading;
    [SerializeField] private float reloadTime = 2f;

    [Header("Burst")]
    public int bulletPerBurst;
    public int burstBulletLeft;
    public int magazineSize;
    public int bulletLeft;

    public GameObject muzzleEffect;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;


    public enum ShootingMode
    {
        Auto,
        Single,
        Burst,
    }
    public ShootingMode currentShootingMode;

    public enum WeaponModel
    {
        RedLazerGun,
        GreenLazerGun,
    }
    public WeaponModel thisWeaponModel;


    private void Start()
    {
        animator = GetComponent<Animator>();
        readyToShoot = true;
        burstBulletLeft = bulletPerBurst;
        bulletLeft = magazineSize;
    }

    void Update()
    {
        HandleInputShootingMode();

        if (isActiveWeapon)
        {
            if (isShooting && readyToShoot && bulletLeft > 0)
            {
                Fire();
                isShooting = false;
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletLeft < magazineSize && !isReloading && !isShooting)
            {
                Reload();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {

            }

        }

    }

    private void Reload()
    {
        isReloading = true;
        SoundManager.Instance.reloadChanel.PlayOneShot(SoundManager.Instance.reloadSound);
        Invoke("ReloadCompleted", reloadTime);
        animator.SetTrigger("RELOAD");
    }

    private void ReloadCompleted()
    {
        bulletLeft = magazineSize;
        isReloading = false;
    }

    private void HandleInputShootingMode()
    {
        if (currentShootingMode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);

        }
    }

    private void Fire()
    {
        SoundManager.Instance.shootingChanel.PlayOneShot(SoundManager.Instance.shootingSound);
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");

        bulletLeft--;

        readyToShoot = false;
        Vector3 shootingDirection = CaculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterLifeTime(bullet, bulletLifeTime));

        if (currentShootingMode == ShootingMode.Burst)
        {
            burstBulletLeft--;
            if (burstBulletLeft > 0)
            {
                Fire();
            }
            else
            {
                burstBulletLeft = bulletPerBurst;
                Invoke("ReadyToShoot", shootingDelay);
            }
        }
        else
        {
            Invoke("ReadyToShoot", shootingDelay);
        }
    }

    private void ReadyToShoot()
    {
        readyToShoot = true;
    }

    private IEnumerator DestroyBulletAfterLifeTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    private Vector3 CaculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        return direction + new Vector3(x, y, 0);
    }

}
