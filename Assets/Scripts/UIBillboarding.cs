using UnityEngine;

public class UIBillboarding : MonoBehaviour
{
    // Camera
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Make Text face camera
        transform.forward = cam.transform.forward;  
    }
}
