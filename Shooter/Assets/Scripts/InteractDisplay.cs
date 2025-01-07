using UnityEngine;
using UnityEngine.UI;

public class InteractDisplay : MonoBehaviour
{
    public Camera playerCamera;  // The camera from which we cast the ray
    public float interactionRange = 2f;  // Maximum distance for interaction
    public Text interactionText;  // The UI text that will appear when looking at an interactable object

    void Start()
    {
        // Ensure the text is initially hidden
        interactionText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Cast a ray from the center of the camera
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Check if the ray hits something within the interaction range
        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            // Check if the object has the tag "Interactable"
            if (hit.collider.CompareTag("Interactable"))
            {
               
                // Show the interaction text
                interactionText.gameObject.SetActive(true);
            }
            else
            {
                // Hide the text if we are not looking at an interactable object
                interactionText.gameObject.SetActive(false);
            }

        }
        else
        {
            // Hide the text if nothing is hit
            interactionText.gameObject.SetActive(false);
        }
    }
}
