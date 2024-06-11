using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    public static InteractionManager Instance { get; private set; }

    public Weapon hoveredWeapon;

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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectWehit = hit.transform.gameObject;

            if (objectWehit.GetComponent<Weapon>() && objectWehit.GetComponent<Weapon>().isActiveWeapon == false)
            {
                hoveredWeapon = objectWehit.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupWeapon(objectWehit);
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            //AmmoBox

            //Throwable

        }
    }
}
