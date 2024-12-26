
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreInterface : MonoBehaviour
{
    // Transfrom for each button to be pressed
    public Transform arButton;
    public Transform pistolButton;
    public Transform deagleButton;
    public Transform smgButton;
    public Transform shotgunButton;
    public Transform lmgButton;
    public Transform rayButton;
    public Transform healthButton;
    public Transform ammoButton;

    // Reference to the AudioSource component
    private AudioSource audio;

    // Reference to the crosshair UI element
    public RawImage crosshair;

    // Reference to the store UI GameObject
    public GameObject storeUI;

    // Reference to the shop text Canvas
    public Canvas shopText;

    // Flag indicating if the player has entered a store
    public bool entered;

    // Static flag indicating if the player is currently in a store
    public static bool inStore;

    private void Start()
    {
        shopText.enabled = false;
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (entered)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Open Shop within range
                shopText.enabled =  false;
                PlayerController.active = false;
                Weapon.active = false;
                crosshair.enabled = false;
                storeUI.SetActive(true);
                Cursor.visible = true;
                updateStoreAmmo();
                if (!inStore)
                {
                    audio.Play();
                }
                inStore = true;
            } 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!inStore)
            {
                shopText.enabled = true;
            }
            entered = true;
            StoreManager.person = other;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shopText.enabled = false;
            entered = false;
            inStore = false;
        }
    }

    public void exit()
    {
        // Close Shop
        if (!StoreManager.updatingText)
        {       
            PlayerController.active = true;
            Weapon.active = true;
            crosshair.enabled = true;
            Cursor.visible = false;
            storeUI.SetActive(false);   
        }
    }

    private void updateStoreAmmo()
    {
        switch (Player.currentWeapon)
        {
            case "Pistol":
                pistolButton.Find("status").GetComponent<TextMeshProUGUI>().SetText(Weapon.ammoLeft + " / " + Weapon.reserveAmmo);
                break;
            case "AR":
                arButton.Find("status").GetComponent<TextMeshProUGUI>().SetText(Weapon.ammoLeft + " / " + Weapon.reserveAmmo);
                break;
            case "Deagle":
                deagleButton.Find("status").GetComponent<TextMeshProUGUI>().SetText(Weapon.ammoLeft + " / " + Weapon.reserveAmmo);
                break;
            case "SMG":
                smgButton.Find("status").GetComponent<TextMeshProUGUI>().SetText(Weapon.ammoLeft + " / " + Weapon.reserveAmmo);
                break;
            case "Shotgun":
                shotgunButton.Find("status").GetComponent<TextMeshProUGUI>().SetText(Weapon.ammoLeft + " / " + Weapon.reserveAmmo);
                break;
            case "LMG":
                lmgButton.Find("status").GetComponent<TextMeshProUGUI>().SetText(Weapon.ammoLeft + " / " + Weapon.reserveAmmo);
                break;
            case "Ray":
                rayButton.Find("status").GetComponent<TextMeshProUGUI>().SetText(Weapon.ammoLeft + " / " + Weapon.reserveAmmo);
                break;
        }
        ammoButton.Find("cost").GetComponent<TextMeshProUGUI>().SetText("$ " + StoreManager.ammoPrice.ToString());
    }


}
