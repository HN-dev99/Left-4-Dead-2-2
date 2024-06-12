using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 40;

    public enum AmmoType
    {
        GreenLazerGun,
        RedLazerGun,
    }
    public AmmoType ammoType;

}
