using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab of the bullet
    public Transform gunNozzle; // Transform representing the gun nozzle position
    public float bulletSpeed = 10f; // Speed of the bullet

    private Vector3 targetPosition; // Midpoint of the screen

    public RaycastWeapon weapon;

    float timer;
    Animator anim;

    void Start()
    {
        // Calculate the midpoint of the screen
        targetPosition = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        targetPosition = Camera.main.ScreenToWorldPoint(targetPosition);
        anim = GetComponent<Animator>();
    }

  
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            weapon.StartFireing();
        }

        anim.SetBool("shooting", weapon.isFireing);

        if (weapon.isFireing)
        {
            weapon.UpdateFireing(Time.deltaTime);
        }

        weapon.UpdateWeapon(Time.deltaTime);

        if (Input.GetButtonUp("Fire1"))
        {
            weapon.StopFireing();
        }

        if(Input.GetKeyDown(KeyCode.R) && weapon.ammoCount < weapon.clipSize && !weapon.reloading)
        {
            anim.SetTrigger("reload");
            StartCoroutine(Reload(1f));
        }
    }

   

    private IEnumerator Reload(float waitTime)
    {
        weapon.reloading = true;
        yield return new WaitForSeconds(waitTime);
        weapon.ammoCount = weapon.clipSize;
        weapon.reloading = false;
    }
}
