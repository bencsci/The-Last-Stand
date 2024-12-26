using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lore : MonoBehaviour
{
    // Reference to the lore display GameObject
    public GameObject loreDisplay;

    // Reference to the lore text UI element
    public TextMeshProUGUI loreText;

    // Reference to the close text UI element
    public TextMeshProUGUI closeText;

    // Reference to the read text Canvas
    public Canvas readText;

    // Reference to the crosshair UI element
    public RawImage crosshair;

    // Flag indicating if the player has entered an area
    private bool entered;

    // Flag indicating if the game is currently paused
    private bool isPaused;

    // Flag indicating if an action has occurred at least once
    private bool once = true;

    private void Start()
    {
        isPaused = false;
        readText.enabled = false;
    }

    void Update()
    {
        if (entered)
        {
            // Open Lore within range
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isPaused)
                {
                    closeLore();
                }
                else
                {
                    openLore();
                    if (once)
                    {
                        Player.points += 100;
                        once = false;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            readText.enabled = true;
            entered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            entered = false;
            readText.enabled = false;
        }
    }

    private void openLore()
    {
        // Display Lore
        readText.enabled = false;
        loreDisplay.SetActive(true);
        loreText.gameObject.SetActive(true);
        closeText.gameObject.SetActive(true);
        loreText.enabled = true;
        closeText.enabled = true;
        crosshair.enabled = false;
        PlayerController.active = false;
        Weapon.active = false;
        Cursor.visible = true;
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void closeLore()
    {
        // Close Lore
        closeText.enabled = false;
        loreText.enabled = false;
        loreDisplay.SetActive(false);
        closeText.gameObject.SetActive(false);
        loreText.gameObject.SetActive(false);
        Cursor.visible = false;
        crosshair.enabled = true;
        PlayerController.active = true;
        Weapon.active = true;
        Time.timeScale = 1f;
        isPaused = false;
        entered = false;
    }
}
