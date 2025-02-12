using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet Variables")]
    public float bulletSpeed;
    public float fireRate, bulletDamage;
    public bool isAuto;
    public int maxAmmo = 30; // Max bullets per clip
    public float reloadTime = 2f; // Reload time in seconds

    [Header("UI Elements")]
    public Text reloadMessage; // Assign in Unity UI
    public Text ammoText; // Assign in Unity UI (for bullet counter)

    [Header("Initial Setup")]
    public Transform bulletSpawnTransform;
    public GameObject bulletPrefab;

    private float timer;
    private int currentAmmo;
    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = maxAmmo; // Start with full ammo
        reloadMessage.enabled = false; // Hide reload message initially
        UpdateAmmoUI(); // Update bullet counter on start
    }

    private void Update()
    {
        if (isReloading)
            return; // Stop shooting while reloading

        if (timer > 0)
            timer -= Time.deltaTime / fireRate;

        if (isAuto)
        {
            if (Input.GetButton("Fire1") && timer <= 0)
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && timer <= 0)
            {
                Shoot();
            }
        }

        // Show reload message when bullets are 5 or less (including 0)
        if (currentAmmo <= 5)
        {
            reloadMessage.enabled = true;
            reloadMessage.text = currentAmmo == 0 ? "Click 'R' reload" : "Press 'R' to Reload";
        }
        else
        {
            reloadMessage.enabled = false;
        }

        // Reload when 'R' is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo! Press 'R' to reload.");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnTransform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnTransform.forward * bulletSpeed, ForceMode.Impulse);
        bullet.GetComponent<Bullet1>().damage = bulletDamage;

        currentAmmo--; // Reduce bullet count
        UpdateAmmoUI(); // Update UI
        timer = 1;
    }

    IEnumerator Reload()
    {
        if (isReloading) yield break; // Prevent multiple reloads

        isReloading = true;
        reloadMessage.enabled = true;
        reloadMessage.text = "Reloading...";
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo; // Reset ammo
        isReloading = false;
        UpdateAmmoUI(); // Update UI after reload
        reloadMessage.enabled = false;
        Debug.Log("Reload Complete!");
    }

    void UpdateAmmoUI()
    {
        ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}"; // Example: "Ammo: 15/30"
    }
}