using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Current weapon equipped
    public static string currentWeapon = "Pistol";

    // Current points accumulated
    public static float points = 0;

    // Current health level
    public static float health = 100;

    // Reference to the player GameObject
    public static GameObject user;

    // References to UI elements for displaying game information

    // UI element for displaying points
    public static TextMeshProUGUI pointsDisplay;

    // UI element for displaying health
    public static TextMeshProUGUI healthDisplay;

    // UI element for displaying damage taken
    public static RawImage damageTakenDisplay;

    // Current color for damageTakenDisplay
    private static Color currentColor;

    // Reference to a MonoBehaviour instance
    private static MonoBehaviour monoBehaviourInstance;

    // Flag to indicate if an action has occurred at least once
    private bool once;

    // Flags to control game mechanics

    // Flag indicating if the player can heal
    private bool canHeal;

    // Reference to the StartGame script
    public static StartGame game;

    private void Awake()
    {
        currentColor = damageTakenDisplay.color;
        monoBehaviourInstance = this;
        canHeal = true;
    }
    void Update()
    {
        // Update player health and points
        pointsDisplay.text =  "$ " + points.ToString();
        healthDisplay.text = health.ToString() + " HP";

        // Display blood screen depending on player health
        if (health <= 25 && !once)
        {
            once = true;
            damageTakenDisplay.enabled = true;
            currentColor.a = 1f;
            damageTakenDisplay.color = currentColor;
            StartCoroutine(flashingBlood());
        }
        else if (health < 100)
        {
            damageTakenDisplay.enabled = true;
            currentColor.a = 1.0f - health/100;
            damageTakenDisplay.color = currentColor;
        } else
        {
            once = false;
            damageTakenDisplay.enabled = false;
        }

        if (health<=100 && canHeal && WaveGeneration.waveNumber < 10 && health > 0)
        {
            StartCoroutine(autoHeal());
        }

    }

    private IEnumerator autoHeal()
    {
        canHeal = false;
        if ((100 - health) >= 10)
        {

            health += 10;

        }
        else
        {

            health += (100 - health);
        }
        yield return new WaitForSeconds(15f);
        canHeal = true;
    }


    public static void takeDamage(float dmg)
    {
        if (health>0)
        {
            if ((health - dmg) <= 0) 
            {
                health = 0;
            } else
            {

                health -= dmg;
            }
            monoBehaviourInstance.StartCoroutine(flashBlood());
        }

        user.GetComponent<Animator>().SetTrigger("hit");

        if (health <= 0)
        {
            Die();
        }
    }

    private static void Die()
    {
        // Make player not interactable
        user.GetComponent<Rigidbody>().isKinematic = true; 

        // Play Death animaion
        user.GetComponent<Animator>().SetTrigger("dead");

        // End the Game
        game.endGame();
        Destroy(user, 3.5f);
    }

    private IEnumerator flashingBlood()
    {
        //flash blood effect while health <= 25 HP
        while (health <= 25)
        {
            for (float t = 0; t < 1f; t += Time.deltaTime / 1f)
            {
                Color lerpedColor = Color.Lerp(currentColor, new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f), t);
                damageTakenDisplay.color = lerpedColor;
                yield return null;
            }

            for (float t = 0; t < 1f; t += Time.deltaTime / 1f)
            {
                Color lerpedColor = Color.Lerp(new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f), currentColor, t);
                damageTakenDisplay.color = lerpedColor;
                yield return null;
            }
        }
    }

    private static IEnumerator flashBlood()
    {
        //flash blood effect
        for (float t = 0; t < 1f; t += Time.deltaTime / 1f)
        {
            Color lerpedColor = Color.Lerp(currentColor, new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f), t);
            damageTakenDisplay.color = lerpedColor;
            yield return null;
        }

        for (float t = 0; t < 1f; t += Time.deltaTime / 1f)
        {
            Color lerpedColor = Color.Lerp(new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f), currentColor, t);
            damageTakenDisplay.color = lerpedColor;
            yield return null;
        }
    }
}
