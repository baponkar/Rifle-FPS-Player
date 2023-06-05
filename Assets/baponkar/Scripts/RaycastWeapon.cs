using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RaycastWeapon : MonoBehaviour
{
    #region Variables
    public int ammoCount;
    public int clipSize;

    //public ActiveWeapon.WeaponSlot weaponSlot;
    public string weaponName;
    public float maxBulletLifeTime  = 3.0f;
    public float bulletSpeed = 1000f;
    public float bulletDrop = 0.0f;
    List<Bullet> bullets = new List<Bullet>();
    public bool isFireing = false;

    public int fireRate = 25;

    public ParticleSystem [] muzzleFlash;

    [Tooltip("Ray start point.")]
    public Transform raycastOrigin;

    public AudioSource audioSource;

    public AudioClip[] fireClips;
    public AudioClip hitClip;
    public AudioClip reloadClip;
    public AudioClip emptyClip;

    public bool reloading;

    public float damage = 10.0f;

    Ray ray;
    RaycastHit hitInfo;

    [Tooltip("The CrossHairTarget which is continuously updating by main camera.Dont need to assign as it will assign by ActiveWeapon Script")]
    public Transform raycastDestination;

    public GameObject bulletHole;


    public TrailRenderer tracerEffect;

    [SerializeField] float accumulatedTime;
    #endregion

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    Vector3 GetPosition(Bullet bullet)
    {
        //Get Bullet position
        //p + ut + 0.5gt^2
        Vector3 gravity = Vector3.down * bulletDrop;
        Vector3 position = bullet.innitialPosition + bullet.initialVelocity * bullet.time + 0.5f * gravity * bullet.time * bullet.time;
        return position;
    }

    private void FireBullet()
    {
        if(ammoCount <= 0 | reloading)
        {
            return;
        }

        PlayBulletFireSound(audioSource,fireClips[Random.Range(0,fireClips.Length)]);
        ammoCount--;
        foreach (ParticleSystem muzzle in muzzleFlash)
        {
                muzzle.Emit(10);
        }
        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.time = 0;
        bullet.innitialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);

        return bullet;
    }

    public void UpdateBullet(float deltaTime)
    {
        SimulateBullet(deltaTime);
        DestroyBullet();
    }

    void SimulateBullet(float deltaTime)
    {
        bullets.ForEach(bulet => {
            Vector3 p0 = GetPosition(bulet);
            bulet.time += deltaTime;
            Vector3 p1 = GetPosition(bulet);
            RaycastSegment(p0, p1, bulet);
        });
    }

    void DestroyBullet()
    {
        if(bullets.Count > 0)
        {
            foreach(Bullet bullet in bullets)
            {
                if(bullet.time > maxBulletLifeTime && bullet.tracer != null)
                {
                    Destroy(bullet.tracer.gameObject);
                }
            }
            bullets.RemoveAll(bullet => bullet.time > maxBulletLifeTime);
        }
    }

    public void UpdateWeapon(float deltaTime)
    {
        if(isFireing)
        {
            UpdateFireing(deltaTime);
        }
        UpdateBullet(deltaTime);
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = (end - start);
        float distance = Vector3.Distance(start, end);
        ray.origin = start;
        ray.direction = direction;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            Debug.Log(hitInfo.transform.name);
            if(hitInfo.transform.GetComponent<AudioSource>() != null)
            {
                hitInfo.transform.gameObject.AddComponent<AudioSource>();
            }
            
            if(hitInfo.transform != this.transform)
            {
                PlayBulletFireSound(hitInfo.transform.GetComponent<AudioSource>(), hitClip);

                Instantiate(bulletHole, hitInfo.point + new Vector3(0.01f,0.01f,-0.05f), Quaternion.LookRotation(hitInfo.normal));
               
                bullet.tracer.transform.position = hitInfo.point;
                bullet.time = maxBulletLifeTime;

                Rigidbody rb = hitInfo.transform.GetComponent<Rigidbody>();
                if( rb != null)
                {
                    hitInfo.transform.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed * Random.Range(0.01f,0.03f));
                }
            }
        }
        else
        {
            if(bullet.tracer != null)
                bullet.tracer.AddPosition(end);
        }
    }

    public void StartFireing()
    {
        isFireing = true;
        accumulatedTime = 0.0f;
        FireBullet();
    }

    public void UpdateFireing(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while(accumulatedTime >= fireInterval)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    public void StopFireing()
    {
        isFireing = false;
    }

    void PlayBulletFireSound(AudioSource audioSource, AudioClip clip)
    {
        if(ammoCount >0)
        {
            //Playing fire sound if there has a audio source, fire sound and audio source is not playing!
            if(audioSource && clip && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(fireClips[Random.Range(0,fireClips.Length)]);
            }
        }
        else
        {
            //Playing fire sound if there has a audio source, fire sound and audio source is not playing!
            if (audioSource && emptyClip && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(emptyClip);
            }
        }
    }
}
