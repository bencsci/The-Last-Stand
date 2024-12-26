using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DisplayPopup : MonoBehaviour
{
    // Reference to the new font asset
    public static TMP_FontAsset newFont;

    // Reference to the popup text GameObject
    public static GameObject popupText;

    // Animation curve for controlling opacity
    public AnimationCurve opacityCurve;

    // Reference to the TextMeshProUGUI component
    private static TextMeshProUGUI textComponent;

    // Reference to the popup GameObject
    private static GameObject popup;

    // Default color for the text
    private static Color textColor;

    // Movement vector for the popup text
    private static Vector3 move;

    // Sorting order for the popup text
    private static int sortingOrder;


    public static void show(GameObject enemy, string text)
    {
        Vector3 offset = new Vector3(0, 1f, 0);
        popup = Instantiate(popupText, enemy.transform.position + offset, Quaternion.identity);
        textComponent = popup.GetComponentInChildren<TextMeshProUGUI>();
        textColor = textComponent.color;
        textComponent.text = text;
        float randomX = Random.Range(0.3f, 0.8f);
        float randomY = Random.Range(0.8f, 2f);
        sortingOrder++;
        popup.GetComponent<Canvas>().sortingOrder = sortingOrder;
        move = new Vector3(randomX, randomY) * 60f;
        Destroy(popup, 1f);
    }

    // Display popup with a given color
    public static void show(GameObject enemy, string text, Color color)
    {
        Vector3 offset = new Vector3(0, 1f, 0);
        popup = Instantiate(popupText, enemy.transform.position + offset, Quaternion.identity);
        textComponent = popup.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.font = newFont;
        textComponent.outlineWidth = 0.15f;
        textComponent.fontSize = 40;
        textComponent.color = color;
        textColor = textComponent.color;
        textComponent.text = text;
        float randomX = Random.Range(0.3f, 0.8f);
        float randomY = Random.Range(0.8f, 2f);
        sortingOrder++;
        popup.GetComponent<Canvas>().sortingOrder = sortingOrder;
        move = new Vector3(randomX, randomY) * 60f;
        Destroy(popup, 1f);
    }

    private void Update()
    {
        // Make Pop up move and enlarge
        if (popup != null && textComponent != null)
        {
            float disappearSpeed = 1f;
            popup.transform.position += move * Time.deltaTime;
            move -= move * 30f * Time.deltaTime;
            
            popup.transform.localScale += Vector3.one * Time.deltaTime;

            textColor.a -= disappearSpeed * Time.deltaTime;
            textComponent.color = textColor; 
        }
        
    }
}
