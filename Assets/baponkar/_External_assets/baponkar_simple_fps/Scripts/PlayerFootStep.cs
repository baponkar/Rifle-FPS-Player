using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baponkar.FPS.Simple
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerFootStep : MonoBehaviour
    {
        #region Variables
        AudioSource audioSource;
        PlayerMovement playerMovement;
        public AudioClip[] grassFootSteps, gravelFootSteps, metalFootSteps, normalFootSteps;
        AudioClip clip;
        public AudioClip jumpClip;

        [Tooltip("0- grass,1- gravel,2- metal,3- normal")]
        [Range(0,3)]
        public int groundType; // grass, gravel, metal, normal;
        #endregion



        void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
            audioSource = GetComponent<AudioSource>();
        }

        
        void Update()
        {
           

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(jumpClip);
                }
            }
        }

        public void FootStep()
        {
            switch (groundType)
            {
                case 0:
                    clip = grassFootSteps[Random.Range(0, grassFootSteps.Length)];
                    break;
                case 1:
                    clip = gravelFootSteps[Random.Range(0, gravelFootSteps.Length)];
                    break;
                case 2:
                    clip = metalFootSteps[Random.Range(0, metalFootSteps.Length)];
                    break;
                case 3:
                    clip = normalFootSteps[Random.Range(0, normalFootSteps.Length)];
                    break;

                default:
                    clip = normalFootSteps[Random.Range(0, normalFootSteps.Length)];
                    break;
            }
            

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
