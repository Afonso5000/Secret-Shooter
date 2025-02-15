using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet Variables")]
    public float bulletSpeed;
    public float fireRate, bulletDamage;
    public bool isAuto;
    public int maxAmmo = 30;
    public float reloadTime = 2f;

    [Header("UI Elements")]
    public Text reloadMessage;
    public Text ammoText;

    [Header("Initial Setup")]
    public Transform bulletSpawnTransform;
    public GameObject bulletPrefab;

    [Header("Audio Sources")]
    public AudioSource shootAudioSource;  // Assign in Inspector
    public AudioSource reloadAudioSource; // Assign in Inspector

    private float timer;
    private int currentAmmo;
    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = maxAmmo;
        reloadMessage.enabled = false;
        UpdateAmmoUI();
    }

    private void Update()
    {
        if (isReloading)
            return;

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

        if (currentAmmo <= 5)
        {
            reloadMessage.enabled = true;
            reloadMessage.text = currentAmmo == 0 ? "Click 'R' to reload" : "Press 'R' to Reload";
        }
        else
        {
            reloadMessage.enabled = false;
        }

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

        currentAmmo--;
        UpdateAmmoUI();
        timer = 1;

        // **Play Shoot Sound**
        if (shootAudioSource != null)
        {
            shootAudioSource.Play();
        }
    }

    IEnumerator Reload()
    {
        if (isReloading) yield break;

        isReloading = true;
        reloadMessage.enabled = true;
        reloadMessage.text = "Reloading...";
        Debug.Log("Reloading...");

        // **Play Reload Sound**
        if (reloadAudioSource != null)
        {
            reloadAudioSource.Play();
        }

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoUI();
        reloadMessage.enabled = false;
        Debug.Log("Reload Complete!");
    }

    void UpdateAmmoUI()
    {
        ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}";
    }
}