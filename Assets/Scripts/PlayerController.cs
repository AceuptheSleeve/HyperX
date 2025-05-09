using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    
    //Compontents
    AudioSource PlayerAs;
    public AudioClip[] SoundBank;
    Rigidbody PlayerRb;

    //Refrences
    public ShipBase PlayerShipBase;
    public FauxGravity GravityCore;
    public GameManager GameManager;

    //Elements
    public Vector3 ResPos = new Vector3();
    public Quaternion ResRot = new Quaternion();
    public bool GravityTilt = false;
    public bool InControl;
    private bool SlipState;

    //Race Mechanics
    public int Lap;
    public int BoostGauge;
    float BoostDelay = 0;
    public int Lives;


    // Start is called before the first frame update
    void Start()
    {
        PlayerRb = GetComponent<Rigidbody>();
        PlayerAs = GetComponent<AudioSource>();
        ResPos = PlayerRb.transform.position;
        ResRot = PlayerRb.transform.rotation;
        Lap = 0;
        BoostGauge = 0;
        InControl = false;
        Lives = 5;
}

    void Update()
    {
        if (GravityTilt)
        { 
            GravityCore.Attract(transform);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float FowardInput = Input.GetAxis("Vertical");

        //Movement
        if (FowardInput != 0 && InControl)
        {
            PlayerRb.transform.Translate(Vector3.forward * FowardInput * PlayerShipBase.Speed * Time.deltaTime);
        }

        if (HorizontalInput != 0 && InControl)
        {
            PlayerRb.transform.Rotate(Vector3.up * HorizontalInput * PlayerShipBase.TurnSpeed);
        }

        //Boosting and Slipsteaming
        if (Time.time >= BoostDelay && InControl)
        {
            if (BoostGauge > 0)
            {
                //Boost
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    Boost(Vector3.forward, FowardInput);
                    BoostDelay = Time.time + 1f / 2f;
                }

                /*Slipstream, not yet implemeneted
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                {
                    Slipstream();
                    BoostDelay = Time.time + 1f / 2f;
                }
                */
            }

            else if (Input.GetKeyDown(KeyCode.LeftShift) && BoostGauge <= 0 && InControl)
            {
                Debug.Log("The Player tried to use their boost... but they didn't have anything in their gauge");
            }

            /*
            else if (Input.GetKeyDown(KeyCode.Q) && BoostGauge <= 0 && InControl)
            {
                Debug.Log("The Player tried to use their slipstream... but they didn't have anything in their gauge");
            }
            */
        }


        //Reset position
        if (Input.GetKeyDown(KeyCode.Space) && InControl)
        {
            Reset();
            Debug.Log("The Player reset their position");
        }

        else if (transform.position.y < -1f)
        {
            Reset();
            Debug.Log("The Player fell too far down");
        }
    }

    //Collisions
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            Reset();
            Debug.Log("The Player touched the planet... resetting position and rotation to most recent checkpoint");
        }

        if (other.gameObject.CompareTag("Boost"))
        {
            BoostGauge++;
            Debug.Log("The Player touched the boostpad... the boost gauge is now at " + BoostGauge);
        }

        if (other.gameObject.CompareTag("Racer"))
        {
            Rigidbody OtherRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 Knockback = other.gameObject.transform.position - transform.position;
            Vector3 SelfKnockback = transform.position - other.gameObject.transform.position;
            int Index = UnityEngine.Random.Range(8, 11);

            OtherRigidbody.AddForce(Knockback * GameManager.Difficulty * 6, ForceMode.Impulse);
            PlayerRb.AddForce(SelfKnockback * 3, ForceMode.Impulse);
            PlayerAs.PlayOneShot(SoundBank[Index]);
        }
    }

    void Reset()
    {
        int Index = UnityEngine.Random.Range(0, 4);
        PlayerRb.isKinematic = true;
        transform.position = ResPos;
        transform.rotation = ResRot;
        PlayerAs.PlayOneShot(SoundBank[Index]);
        PlayerRb.isKinematic = false;
        Lives--;
        Debug.Log("The Player has " +Lives+ " lives left");
    }

    void Boost(Vector3 Direction, float Input)
    {
        BoostGauge--;
        PlayerAs.PlayOneShot(SoundBank[6]);
        PlayerRb.transform.Translate(Direction * Input * PlayerShipBase.BoostFactor);
        Debug.Log("The Player used their boost! Their boost gauge is now at " + BoostGauge);
    }

    //Not in use yet, needs work
    void Slipstream()
    {
        BoostGauge--;
        PlayerAs.PlayOneShot(SoundBank[7]);
        PlayerRb.transform.Rotate(Vector3.up * 360 * Time.deltaTime);
        Debug.Log("The Player used their slipstream! Their boost gauge is now at " + BoostGauge);

        while (4f >= Time.time)
        {
            SlipState = true;
            InControl = false;
            Debug.Log(Time.time);
        }

        SlipState = false;
        InControl = true;
    }
}
