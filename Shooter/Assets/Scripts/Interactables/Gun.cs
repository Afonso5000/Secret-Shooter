using UnityEngine;

public class Gun : MonoBehaviour, IInteractable
{
    public GameObject gun;
    public Transform handPosition;

    // This method is called when the player interacts with the gun
    public void Interact()
    {
        PickUpGun();
    }

    // Logic for picking up the gun
    private void PickUpGun()
    {
        gun.SetActive(false);  // Hide gun from the stand

        // Instantiate the gun in the player's hand position
        GameObject newGun = Instantiate(gun, handPosition.position, handPosition.rotation);
        newGun.SetActive(true);

        // Make the gun follow the hand's position
        newGun.transform.SetParent(handPosition);
    }
}