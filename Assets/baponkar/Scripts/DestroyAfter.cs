using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float lifeTime = 1f;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
            Destroy(this.gameObject);
    }
}
