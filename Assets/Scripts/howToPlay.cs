using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class howToPlay : MonoBehaviour
{
    // Reference to the RawImage component
    public RawImage image;

    // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI text;

    // Array of textures to cycle through
    public Texture[] textures;

    // Array of texts corresponding to each texture
    private string[] texts;

    // Index of the current texture/text
    private int currentIndex = 0;


    private string introText = "How to Play:\n" +
        "Goal: Survive for as long as possible. There’s is no chance of you making out alive.\n" +
        "A round ends after killing all enemies in the area. After the end of the round a 10 second timer starts before the next round begins.\n\n" +
        "Controls:\n" +
        "“W” = move up\n" +
        "“S” = move down\n" +
        "“A” = move left\n" +
        "“D” = move right\n" +
        "“LMB” = fire\n" +
        "“R” = reload\n" +
        "“ESC” = pause\n" +
        "“F” = open shop menu (when in range)";

    private string bossText = "Beware of the boss spawning every 5 rounds.\n" +
     "He moves swiftly and inflicts critical damage upon contact!\n" +
     "Stay vigilant as each round introduces new and unique enemies.\n" +
     "\nThings to Note:" +
     "\nHidden notes are scattered throughout the\n map each granting 100 points upon discovery.\n" +
     "\nHealth regenerates 10 HP every\n15 seconds for the initial 9 rounds.\n" +
     "\nAmmo can drop on the ground\nafter the 5th round.\n";


    private string borderText = "Be mindful of stone and wood fences representing the map perimeter\n" +
    "straying into these barriers can quickly lead to being cornered by the undead hordes\n" +
    "exercise caution as you explore the surroundings to evade potential entrapment.\n" +
    "\nQuick Tips:\n" +
    "\nMaintain awareness of your surroundings to\nidentify safe routes and escape strategies.\n" +
    "\nRemember, survival hinges on strategic\nnavigation and avoiding fatal dead ends.\n";


    private string damageText =
    "CRITICAL - Land 3 consecutive shots for bonus damage and points.\n " +
    "FATAL - Secure the final blow for a substantial score boost.\n" +
    "EXECUTION - Critical shot on the final blow grants massive points!\n" +
    "\nSpecial damage types offer unique advantages!\n" +
    "\nMaster the use of these special attacks to\novercome even the toughest adversaries.\n" +
    "\nBy strategically employing them, you can\nnot only defeat stronger opponents\n" +
    "but also earn more points to unlock\na wider array of devastating weapons.\n";


    private string shootText = "The crosshair aids in hit prediction. " +
    "Green denotes hits, white signals misses. " +
    "You don't need to aim the crosshair directly at them. " +
    "You can aim in front of the enemy or behind them. " +
    "Stay focused, stay precise, and survive against the relentless onslaught of the undead!\n" +
    "\nIn intense horde encounters adaptability\nis paramount.\n" +
    "\nRemember to conserve ammunition and aim\nto hit critical shots to improve efficiency.\n" +
    "\nPrecision is key; aim strategically!\n" 
    ;




    void Start()
    {
        image.enabled = false;

        texts = new string[]
        {
            introText,
            bossText,
            borderText,
            damageText,
            shootText
        };

        ShowCurrent();
    }

    public void next()
    {
        // Move to the next page, cycling if end is reached
        currentIndex = (currentIndex + 1) % texts.Length;

        ShowCurrent();
    }

    public void previous()
    {
        // Move to the previous page, cycling if end is reached
        currentIndex = (currentIndex - 1 + texts.Length) % texts.Length;

        ShowCurrent();
    }

    private void ShowCurrent()
    {
        // Update image and text depending on given page
        if (currentIndex != 0)
        {
            // Show the current image
            image.texture = textures[currentIndex];
            image.enabled = true;
            text.SetText(texts[currentIndex]);
        } else
        {
            image.enabled = false;
            text.SetText(texts[currentIndex]);
        }
        
    }
}
