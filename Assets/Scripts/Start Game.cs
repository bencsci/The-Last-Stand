using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    // UI element for displaying points
    public TextMeshProUGUI pointsDisplay;

    // UI element for displaying health
    public TextMeshProUGUI healthDisplay;

    // UI element for displaying ammunition
    public TextMeshProUGUI ammoDisplay;

    // Reference to the crosshair RawImage
    public RawImage crosshair;

    // Reference to the player GameObject
    public GameObject player;

    // Reference to the store UI GameObject
    public GameObject storeUI;

    // Reference to the popup GameObject
    public GameObject popup;

    // Reference to the font asset
    public TMP_FontAsset font;

    // UI element for displaying damage taken
    public RawImage damageTakenDisplay;

    // Reference to the death screen GameObject
    public GameObject deathScreen;

    // Reference to the AudioSource component
    public AudioSource audio;

    // Sound clip for start
    public AudioClip startSound;

    // Sound clip for end
    public AudioClip endSound;

    // Flag to indicate if an action has occurred at least once
    public bool once;


    // Start is called before the first frame update
    void Awake()
    {
        // Initiliaze Game Objects and Variables for Start of Game
        WaveGeneration.waveNumber = 0;

        once = false;
        audio = GetComponent<AudioSource>();
        audio.clip = startSound;
        audio.Play();

        deathScreen.SetActive(false);
     
        crosshair.enabled = true;
        storeUI.SetActive(false);
        Cursor.visible = false;
        BasicEnemy.player = player;
        CameraFollow.target = player.transform;
        WaveGeneration.player = player;
        
        PlayerController.active = true;
        PlayerController.crosshair = crosshair.rectTransform;
        
        Weapon.active = true;
        Weapon.ammoDisplay = ammoDisplay;
        Weapon.crosshair = crosshair;
        Weapon.player = player;

        AmmoManager.resetAmmo(true);

        Player.currentWeapon = "Pistol";
        Player.user = player;
        Player.health = 100;
        Player.points = 0;
        Player.damageTakenDisplay = damageTakenDisplay;
        Player.healthDisplay = healthDisplay;
        Player.pointsDisplay = pointsDisplay;
        Player.game = this;

        
        AmmoManager.setAmmo(Player.currentWeapon);

        DisplayPopup.popupText = popup;
        DisplayPopup.newFont = font;

    }

    public void endGame()
    {
        PlayerController.active = false;
        Weapon.active = false;
        crosshair.enabled = false;
        Cursor.visible = true;
        if (!once)
        {
            audio.clip = endSound;
            audio.Play();
            once = true;
        }
        StartCoroutine(showDeathScreen());
        
    }

    private IEnumerator showDeathScreen()
    {
        yield return new WaitForSeconds(4f);
        deathScreen.SetActive(true);
    }
}
