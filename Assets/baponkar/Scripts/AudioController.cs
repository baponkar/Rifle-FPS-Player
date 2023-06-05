using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{  
    public AudioClip shellHitGroundClip;

    public AudioClip gunDrawClip;

    public AudioSource gunAudioSource;
    public AudioSource shellAudioSource;

    ActiveWeapon weapon;


    void Start()
    {
        weapon = GetComponent<ActiveWeapon>();
    }

    
    void Update()
    {
        
    }

    public void GunShoot()
    {
        if(weapon.weapon.ammoCount > 0)
        {
            gunAudioSource.PlayOneShot(weapon.weapon.fireClips[Random.Range(0, weapon.weapon.fireClips.Length)]);
        }
        else
        {
            gunAudioSource.PlayOneShot(weapon.weapon.emptyClip);
        }
    }

    public void ShellHitGround()
    {
        gunAudioSource.PlayOneShot(weapon.weapon.hitClip);
    }

    public void Reload()
    {
        gunAudioSource.PlayOneShot(weapon.weapon.reloadClip);
    }

    public void GunDraw()
    {
        gunAudioSource.PlayOneShot(gunDrawClip);
    }
}
