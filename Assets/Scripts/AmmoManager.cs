using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    // Current Ammo for weapon
    public static int currentMagazine;
    public static int currentReserve;
    public static int currentCapacity;


    // Starting Ammo for each weapon
    public static int pistolMagazine = 8;
    public static int pistolReserve = 280;
    public static int pistolAmmoLeft = pistolMagazine;

    public static int deagleMagazine = 6;
    public static int deagleReserve = 210;
    public static int deagleAmmoLeft = deagleMagazine;

    public static int smgMagazine = 40;
    public static int smgReserve = 360;
    public static int smgAmmoLeft = smgMagazine;

    public static int shotgunMagazine = 12;
    public static int shotgunReserve = 108;
    public static int shotgunAmmoLeft = shotgunMagazine;

    public static int arMagazine = 30;
    public static int arReserve = 270;
    public static int arAmmoLeft = arMagazine;

    public static int lmgMagazine = 80;
    public static int lmgReserve = 400;
    public static int lmgAmmoLeft = lmgMagazine;

    public static int rayMagazine = 21;
    public static int rayReserve = 189;
    public static int rayAmmoLeft = rayMagazine;

    private void Start()
    {
        resetAmmo(false);
    }

    public static void resetAmmo(bool resetCurrent)
    {
        // Reset the current ammo of player if set to true
        if (resetCurrent)
        {
            currentMagazine = 0;
            currentReserve = 0;
            currentCapacity = 0;
        }
        

        pistolMagazine = 8;
        pistolReserve = 280;
        pistolAmmoLeft = pistolMagazine;

        deagleMagazine = 6;
        deagleReserve = 210;
        deagleAmmoLeft = deagleMagazine;

        smgMagazine = 40;
        smgReserve = 360;
        smgAmmoLeft = smgMagazine;

        shotgunMagazine = 12;
        shotgunReserve = 108;
        shotgunAmmoLeft = shotgunMagazine;

        arMagazine = 30;
        arReserve = 270;
        arAmmoLeft = arMagazine;

        lmgMagazine = 80;
        lmgReserve = 400;
        lmgAmmoLeft = lmgMagazine;

        rayMagazine = 21;
        rayReserve = 189;
        rayAmmoLeft = rayMagazine;
    }
    public static void storeAmmo(string weapon)
    {
        // Store Ammo for each weapon to be picked up later
        switch (weapon)
        {
            case "Pistol":
                pistolAmmoLeft = Weapon.ammoLeft;
                pistolReserve = Weapon.reserveAmmo;
                break;
            case "Deagle":
                deagleAmmoLeft = Weapon.ammoLeft;
                deagleReserve = Weapon.reserveAmmo;
                break;
            case "SMG":
                smgAmmoLeft = Weapon.ammoLeft;
                smgReserve = Weapon.reserveAmmo;
                break;
            case "Shotgun":
                shotgunAmmoLeft = Weapon.ammoLeft;
                shotgunReserve = Weapon.reserveAmmo;
                break;
            case "AR":
                arAmmoLeft = Weapon.ammoLeft;
                arReserve = Weapon.reserveAmmo;
                break;
            case "LMG":
                lmgAmmoLeft = Weapon.ammoLeft;
                lmgReserve = Weapon.reserveAmmo;
                break;
            case "Ray":
                rayAmmoLeft = Weapon.ammoLeft;
                rayReserve = Weapon.reserveAmmo;
                break;
        }
    }

    public static void setAmmo(string weapon)
    {
        // Set the stored ammo to the Players current ammo
        switch (weapon)
        {
            case "Pistol":
                currentMagazine = pistolMagazine;
                currentReserve = pistolReserve;
                currentCapacity = pistolAmmoLeft;
                break;
            case "Deagle":
                currentMagazine = deagleMagazine;
                currentReserve = deagleReserve;
                currentCapacity = deagleAmmoLeft;
                break;
            case "SMG":
                currentMagazine = smgMagazine;
                currentReserve = smgReserve;
                currentCapacity = smgAmmoLeft;
                break;
            case "Shotgun":
                currentMagazine = shotgunMagazine;
                currentReserve = shotgunReserve;
                currentCapacity = shotgunAmmoLeft;
                break;
            case "AR":
                currentMagazine = arMagazine;
                currentReserve = arReserve;
                currentCapacity = arAmmoLeft;
                break;
            case "LMG":
                currentMagazine = lmgMagazine;
                currentReserve = lmgReserve;
                currentCapacity = lmgAmmoLeft;
                break;
            case "Ray":
                currentMagazine = rayMagazine;
                currentReserve = rayReserve;
                currentCapacity = rayAmmoLeft;
                break;
        }
    }

    public static bool addAmmo(string weapon, int numClip)
    {
        // Add ammo to player with given number of clip
        bool canAdd = true;

        switch (weapon)
        {
            case "Pistol":
                if ((Weapon.reserveAmmo + (pistolMagazine * numClip)) <= 999)
                {
                    Weapon.reserveAmmo += pistolMagazine * numClip;
                }
                else
                {
                    canAdd = false;
                }
                break;
            case "Deagle":
                if ((Weapon.reserveAmmo + (deagleMagazine * numClip)) <= 999)
                {
                    Weapon.reserveAmmo += deagleMagazine * numClip;
                }
                else
                {
                    canAdd = false;
                }
                break;
            case "SMG":
                if ((Weapon.reserveAmmo + (smgMagazine * numClip)) <= 999)
                {
                    Weapon.reserveAmmo += smgMagazine * numClip;
                }
                else
                {
                    canAdd = false;
                }
                break;
            case "Shotgun":
                if ((Weapon.reserveAmmo + (shotgunMagazine * numClip)) <= 999)
                {
                    Weapon.reserveAmmo += shotgunMagazine * numClip;
                }
                else
                {
                    canAdd = false;
                }
                break;
            case "AR":
                if ((Weapon.reserveAmmo + (arMagazine * numClip)) <= 999)
                {
                    Weapon.reserveAmmo += arMagazine * numClip;
                }
                else
                {
                    canAdd = false;
                }
                break;
            case "LMG":
                if ((Weapon.reserveAmmo + (lmgMagazine * numClip)) <= 999)
                {
                    Weapon.reserveAmmo += lmgMagazine * numClip;
                }
                else
                {
                    canAdd = false;
                }
                break;
            case "Ray":
                if ((Weapon.reserveAmmo + (rayMagazine * numClip)) <= 999)
                {
                    Weapon.reserveAmmo += rayMagazine * numClip;
                }
                else
                {
                    canAdd = false;
                }
                break;
        }
        return canAdd;
    }

}
