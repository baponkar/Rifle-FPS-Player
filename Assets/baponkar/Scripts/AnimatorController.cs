using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Baponkar.FPS.Simple;


public class AnimatorController : MonoBehaviour
{
    PlayerMovement movement;

   
    bool draw = true;

    Animator anim;

    float smoothTime = 0.3f;
    float velocity = 0.0f;
    float speed;

    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    
    void Update()
    {
        speed = Mathf.Lerp(0, 1, movement.movement.magnitude*10f);
        anim.SetFloat("speed", speed);
        

        if (Input.GetKeyDown(KeyCode.Q))
            draw = !draw;

        if(Input.GetKeyDown(KeyCode.T))
            anim.SetTrigger("inspect");
       
        anim.SetBool("draw", draw);
        if (Input.GetKeyDown(KeyCode.R))
            anim.SetTrigger("reload");
        
    }
}
