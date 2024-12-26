using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class StoreManager : MonoBehaviour
{
    public RawImage currentWeapon;

    // Raw Images for each weapon
    public Texture pistolImg;
    public Texture deagleImg;
    public Texture smgImg;
    public Texture shotgunImg;
    public Texture arImg;
    public Texture lmgImg;
    public Texture rayImg;

    // Transform for each weapon button
    public Transform arButton;
    public Transform pistolButton;
    public Transform deagleButton;
    public Transform smgButton;
    public Transform shotgunButton;
    public Transform lmgButton;
    public Transform rayButton;

    public Transform healthButton;
    public Transform ammoButton;

    // Player prefab for each weapon
    public GameObject AR;
    public GameObject Pistol;
    public GameObject Deagle;
    public GameObject SMG;
    public GameObject Shotgun;
    public GameObject LMG;
    public GameObject Ray;

    // Weapons prices
    public static int arPrice = 25000;
    public static int pistolPrice = 0;
    public static int deaglePrice = 2000;
    public static int smgPrice = 5000;
    public static int shotgunPrice = 10000;
    public static int lmgPrice = 50000;
    public static int rayPrice = 125000;

    public static int healthPrice = 10000;
    public static int ammoPrice = 100;

    // Reference to the AudioSource component
    private AudioSource audio;

    // Sound clip for cashing out
    public AudioClip cashout;

    // UI element for displaying points
    public TextMeshProUGUI pointsDisplay;

    // UI element for displaying ammunition
    public TextMeshProUGUI ammoDisplay;

    // Reference to the crosshair RawImage
    public RawImage crosshair;

    // Reference to the player GameObject for New Player model
    private GameObject player;

    // Collider to get position and rotation of player
    public static Collider person;

    // Flag indicating if text is being updated
    public static bool updatingText = false;

    // Number of clips
    private int numClip = 3;


    private void Awake()
    {
        // Add Listeners to all Buttons
        audio = GetComponent<AudioSource>();
        pistolButton.GetComponent<Button>().onClick.AddListener(() => equip(Pistol, "Pistol", pistolButton));
        arButton.GetComponent<Button>().onClick.AddListener(buyAR);
        deagleButton.GetComponent<Button>().onClick.AddListener(buyDeagle);
        smgButton.GetComponent<Button>().onClick.AddListener(buySMG);
        shotgunButton.GetComponent<Button>().onClick.AddListener(buyShotgun);
        lmgButton.GetComponent<Button>().onClick.AddListener(buyLMG);
        rayButton.GetComponent<Button>().onClick.AddListener(buyRay);

        healthButton.GetComponent<Button>().onClick.AddListener(buyHealth);
        ammoButton.GetComponent<Button>().onClick.AddListener(buyAmmo);
    }

    // -------------------- Functions for buying different items ------------------------------------------------------------
    public void buyAmmo()
    {
        if (Player.points >= ammoPrice && AmmoManager.addAmmo(Player.currentWeapon, numClip))
        {
            Player.points -= ammoPrice;
            audio.clip = cashout;
            audio.Play();
            StartCoroutine(purchased(0.1f, ammoButton));
        }
        else
        {
            StartCoroutine(notEnough(0.1f, ammoButton));
        }
    }

    public void buyHealth()
    {
        if (Player.points >= healthPrice && (Player.health < 100))
        {
            if ((100 - Player.health) >= 25)
            {
                Player.points -= healthPrice;
                Player.health += 25;

            }
            else
            {
                Player.points -= (healthPrice / 25) * (100 - Player.health);
                Player.health += (100 - Player.health);
            }
            audio.clip = cashout;
            audio.Play();
            StartCoroutine(purchased(0.1f, healthButton));
        }
        else
        {
            StartCoroutine(notEnough(0.1f, healthButton));
        }
    }

    public void buyAR()
    {
        if (Player.points >= arPrice)
        {
            arButton.Find("cost").GetComponent<TextMeshProUGUI>().SetText("");
            equip(AR, "AR", arButton);
            arButton.GetComponent<Button>().onClick.AddListener(() => equip(AR, "AR", arButton));
            arButton.GetComponent<Button>().onClick.RemoveListener(buyAR);

            Player.points -= arPrice;
            audio.clip = cashout;
            audio.Play();
        }
        else
        {
            StartCoroutine(notEnough(0.1f, arButton));
        }
    }

    public void buyDeagle()
    {
        if (Player.points >= deaglePrice)
        {
            deagleButton.Find("cost").GetComponent<TextMeshProUGUI>().SetText("");
            equip(Deagle, "Deagle", deagleButton);
            deagleButton.GetComponent<Button>().onClick.AddListener(() => equip(Deagle, "Deagle", deagleButton));
            deagleButton.GetComponent<Button>().onClick.RemoveListener(buyDeagle);

            Player.points -= deaglePrice;
            audio.clip = cashout;
            audio.Play();
        }
        else
        {
            StartCoroutine(notEnough(0.1f, deagleButton));
        }
    }

    public void buySMG()
    {
        if (Player.points >= smgPrice)
        {
            smgButton.Find("cost").GetComponent<TextMeshProUGUI>().SetText("");
            equip(SMG, "SMG", smgButton);
            smgButton.GetComponent<Button>().onClick.AddListener(() => equip(SMG, "SMG", smgButton));
            smgButton.GetComponent<Button>().onClick.RemoveListener(buySMG);

            Player.points -= smgPrice;
            audio.clip = cashout;
            audio.Play();
        }
        else
        {
            StartCoroutine(notEnough(0.1f, smgButton));
        }
    }

    public void buyShotgun()
    {
        if (Player.points >= shotgunPrice)
        {
            shotgunButton.Find("cost").GetComponent<TextMeshProUGUI>().SetText("");
            equip(Shotgun, "Shotgun", shotgunButton);
            shotgunButton.GetComponent<Button>().onClick.AddListener(() => equip(Shotgun, "Shotgun", shotgunButton));
            shotgunButton.GetComponent<Button>().onClick.RemoveListener(buyShotgun);

            Player.points -= shotgunPrice;
            audio.clip = cashout;
            audio.Play();
        }
        else
        {
            StartCoroutine(notEnough(0.1f, shotgunButton));
        }
    }

    public void buyLMG()
    {
        if (Player.points >= lmgPrice)
        {
            lmgButton.Find("cost").GetComponent<TextMeshProUGUI>().SetText("");
            equip(LMG, "LMG", lmgButton);
            lmgButton.GetComponent<Button>().onClick.AddListener(() => equip(LMG, "LMG", lmgButton));
            lmgButton.GetComponent<Button>().onClick.RemoveListener(buyLMG);

            Player.points -= lmgPrice;
            audio.clip = cashout;
            audio.Play();
        }
        else
        {
            StartCoroutine(notEnough(0.1f, lmgButton));
        }
    }

    public void buyRay()
    {
        if (Player.points >= rayPrice)
        {
            rayButton.Find("cost").GetComponent<TextMeshProUGUI>().SetText("");
            equip(Ray, "Ray", rayButton);
            rayButton.GetComponent<Button>().onClick.AddListener(() => equip(Ray, "Ray", rayButton));
            rayButton.GetComponent<Button>().onClick.RemoveListener(buyRay);

            Player.points -= rayPrice;
            audio.clip = cashout;
            audio.Play();
        }
        else
        {
            StartCoroutine(notEnough(0.1f, rayButton));
        }
    }
    // ----------------------------------------------------------------------------------------------------


    public void equip(GameObject gun, string weaponName, Transform button)
    {
        // Replace Current Player with new Player with equiped weapon
        if (!updatingText)
        {
            AmmoManager.storeAmmo(Player.currentWeapon);
            AmmoManager.setAmmo(weaponName);

            // Set Weapons Mag and Reserve Size
            player = Instantiate(gun, person.transform.position, person.transform.rotation);

            BasicEnemy.player = player;
            Player.user = player;
            Weapon.player = player;
            WaveGeneration.player = player;
            Weapon.ammoDisplay = ammoDisplay;
            Weapon.crosshair = crosshair;
            CameraFollow.target = player.transform;
            PlayerController.crosshair = crosshair.rectTransform;

            Destroy(person.gameObject);

            Player.currentWeapon = weaponName;
            updatingText = true;
            updateStoreAmmoPrice();
            StartCoroutine(resetStatusAfterDelay(0.25f, button));
        } 
    }

    private void updateStoreAmmoPrice()
    {
        // Update store ammo upon opening store
        switch (Player.currentWeapon)
        {
            case "Pistol":
                ammoPrice = 100;
                ammoButton.Find("gunImg").GetComponent<RawImage>().texture = pistolImg;
                ammoButton.Find("ammo").GetComponent<TextMeshProUGUI>().SetText("+" + (AmmoManager.pistolMagazine * numClip).ToString());
                currentWeapon.texture = pistolImg;
                break;
            case "AR":
                ammoPrice = arPrice / 5;
                ammoButton.Find("gunImg").GetComponent<RawImage>().texture = arImg;
                ammoButton.Find("ammo").GetComponent<TextMeshProUGUI>().SetText("+" + (AmmoManager.arMagazine * numClip).ToString());
                currentWeapon.texture = arImg;
                break;
            case "Deagle":
                ammoPrice = deaglePrice / 5;
                ammoButton.Find("gunImg").GetComponent<RawImage>().texture = deagleImg;
                ammoButton.Find("ammo").GetComponent<TextMeshProUGUI>().SetText("+" + (AmmoManager.deagleMagazine * numClip).ToString());
                currentWeapon.texture = deagleImg;
                break;
            case "SMG":
                ammoPrice = smgPrice / 5;
                ammoButton.Find("gunImg").GetComponent<RawImage>().texture = smgImg;
                ammoButton.Find("ammo").GetComponent<TextMeshProUGUI>().SetText("+" + (AmmoManager.smgMagazine * numClip).ToString());
                currentWeapon.texture = smgImg;
                break;
            case "Shotgun":
                ammoPrice = shotgunPrice / 5;
                ammoButton.Find("gunImg").GetComponent<RawImage>().texture = shotgunImg;
                ammoButton.Find("ammo").GetComponent<TextMeshProUGUI>().SetText("+" + (AmmoManager.shotgunMagazine * numClip).ToString());
                currentWeapon.texture = shotgunImg;
                break;
            case "LMG":
                ammoPrice = lmgPrice / 5;
                ammoButton.Find("gunImg").GetComponent<RawImage>().texture = lmgImg;
                ammoButton.Find("ammo").GetComponent<TextMeshProUGUI>().SetText("+" + (AmmoManager.lmgMagazine * numClip).ToString());
                currentWeapon.texture = lmgImg;
                break;
            case "Ray":
                ammoPrice = rayPrice / 5;
                ammoButton.Find("gunImg").GetComponent<RawImage>().texture = rayImg;
                ammoButton.Find("ammo").GetComponent<TextMeshProUGUI>().SetText("+" + (AmmoManager.rayMagazine * numClip).ToString());
                currentWeapon.texture = rayImg;
                break;
        }
        ammoButton.Find("cost").GetComponent<TextMeshProUGUI>().SetText("$ " + ammoPrice.ToString());
    }




    private IEnumerator resetStatusAfterDelay(float delay, Transform button)
    {
        // Reset the status text
        TextMeshProUGUI statusText = button.Find("status").GetComponent<TextMeshProUGUI>();
        statusText.fontSize = 30;
        statusText.SetText("EQUIPPED");
        statusText.color = Color.green;

        yield return new WaitForSeconds(delay);
        statusText.color = Color.white;
        statusText.fontSize = 40;
        statusText.SetText(Weapon.ammoLeft + " / " + Weapon.reserveAmmo);
        updatingText = false;
    }

    private IEnumerator purchased(float delay, Transform button)
    {
        // Reset the status text
        button.Find("cost").GetComponent<TextMeshProUGUI>().color = Color.green;

        yield return new WaitForSeconds(delay);
        button.Find("cost").GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    private IEnumerator notEnough(float delay, Transform button)
    {
        // Reset the status text
        button.Find("cost").GetComponent<TextMeshProUGUI>().color = Color.red;

        yield return new WaitForSeconds(delay);
        button.Find("cost").GetComponent<TextMeshProUGUI>().color = Color.white;
    }
}
