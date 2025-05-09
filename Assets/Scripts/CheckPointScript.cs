using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CheckPointScript : MonoBehaviour
{
    private PlayerController PlayerController;
    private GameObject Player;
    private ShipScript ShipScript;
    public GameManager GameManager;
    public GameObject OtherChecker;
    public bool LapPoint;
    float TimeDecrease;

    public float[] DistanceArrays;

    public Transform[] Ships;
    public Transform PlayerTrans;

    float First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eigth;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerController = Player.GetComponent<PlayerController>();

        Ships[0] = GameManager.TotalRacers[0].transform;
        PlayerTrans = Ships[0];
    }

    void Start()
    {
        /*
        for (int i = 1; i < GameManager.TotalRacers.Count; i++)
        {
            Ships[i] = GameManager.TotalRacers[i].transform;
        }
        */

        TimeDecrease = GameManager.Timer / 6;
    }

    void Update()
    {
        /*
        DistanceArrays[0] = Vector3.Distance(transform.position, Ships[0].position);
        DistanceArrays[1] = Vector3.Distance(transform.position, Ships[1].position);
        DistanceArrays[2] = Vector3.Distance(transform.position, Ships[2].position);
        DistanceArrays[3] = Vector3.Distance(transform.position, Ships[3].position);
        DistanceArrays[4] = Vector3.Distance(transform.position, Ships[4].position);
        DistanceArrays[5] = Vector3.Distance(transform.position, Ships[5].position);
        DistanceArrays[6] = Vector3.Distance(transform.position, Ships[6].position);
        DistanceArrays[7] = Vector3.Distance(transform.position, Ships[7].position);

        Array.Sort(DistanceArrays);

        First = DistanceArrays[0];
        Second = DistanceArrays[1];
        Third = DistanceArrays[2];
        Fourth = DistanceArrays[3];
        Fifth = DistanceArrays[4];
        Sixth = DistanceArrays[5];
        Seventh = DistanceArrays[6];
        Eigth = DistanceArrays[7];
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the player triggers
        if (other.name == "Player")
        {
            //Setting new spawn point
            PlayerController.ResPos = transform.position;
            PlayerController.ResRot = transform.rotation;
            PlayerController.ResPos.y -= 2f;

            //enabling the next checkpoint
            GameManager.TimeCut(TimeDecrease);
            OtherChecker.SetActive(true);
            gameObject.SetActive(false);
            Debug.Log("The Player passed " + gameObject.name+ " their new checkpoint is now " +OtherChecker.name);
            GameManager.CurrentCheckPoint = OtherChecker.transform.position;
            Debug.Log("The Player's new reset position is " +PlayerController.ResPos+ " and their rotation is " +PlayerController.ResRot);

            if (LapPoint == true)
            {
                PlayerController.Lap++;
                Debug.Log("The Player is now on Lap: " +PlayerController.Lap);
            }
        }
    }
    
    /* Maybe for a race mode?
    public void RankingSystem(int id)
    {
        string Name = Ships[id].name;
        float ShipDistance = Vector3.Distance(transform.position, Ships[id].position);

        if (id == 0)
        { 
            if (ShipDistance == First)
            {
                Debug.Log("The Player is in 1st place!!!");
            }

            else if (ShipDistance == Second)
            {
                Debug.Log("The Player is in 2nd place!!!");
            }

            else if (ShipDistance == Third)
            {
                Debug.Log("The Player is in 3rd place!!!");
            }

            else if (ShipDistance == Fourth)
            {
                Debug.Log("The Player is in 4th place!!!");
            }

            else if (ShipDistance == Fifth)
            {
                Debug.Log("The Player is in 5th place!!!");
            }

            else if (ShipDistance == Sixth)
            {
                Debug.Log("The Player is in 6th place!!!");
            }

            else if (ShipDistance == Seventh)
            {
                Debug.Log("The Player is in 7th place!!!");
            }

            else if (ShipDistance == Eigth)
            {
                Debug.Log("The Player is in 8th place!!!");
            }
        }

        else if (id > 0)
        {
            if (ShipDistance == First)
            {
                Debug.Log(Name+ " is in 1st place!!!");
            }

            else if (ShipDistance == Second)
            {
                Debug.Log(Name+ " is in 2nd place!!!");
            }

            else if (ShipDistance == Third)
            {
                Debug.Log(Name+ " is in 3rd place!!!");
            }

            else if (ShipDistance == Fourth)
            {
                Debug.Log(Name+ " is in 4th place!!!");
            }

            else if (ShipDistance == Fifth)
            {
                Debug.Log(Name+ " is in 5th place!!!");
            }

            else if (ShipDistance == Sixth)
            {
                Debug.Log(Name+ " is in 6th place!!!");
            }

            else if (ShipDistance == Seventh)
            {
                Debug.Log(Name+ " is in 7th place!!!");
            }

            else if (ShipDistance == Eigth)
            {
                Debug.Log(Name+  " is in 8th place!!!");
            }
        }
    }

    void RecountRanks()
    {
        Debug.Log("The current ranks are: ");
        RankingSystem(0);
        RankingSystem(1);
        RankingSystem(2);
        RankingSystem(3);
        RankingSystem(4);
        RankingSystem(5);
        RankingSystem(6);
        RankingSystem(7);
    }
    */


}
