using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.Progress;
using static UnityEngine.ParticleSystem;

public class GameManager : MonoBehaviour
{
    //Lists and Arrays
    public List<GameObject> TotalRacers = new List<GameObject>();
    private List<string> Names = new List<string>();
    public GameObject[] ShipCatalog;
    public GameObject[] Checkers;

    //Floats, Bools, and Ints
    public float Score;
    public float Timer;
    public int Difficulty;
    private int NameIndex;
    private int ShipIndex;
    public bool GameActive;

    //UI Elements
    public GameObject GameUI;
    public TextMeshProUGUI TimerDisplay, CheckpointDisplay, BoostDisplay, LapDisplay;
    public GameObject TitleUI;
    public GameObject GameOverUI;
    public GameObject RestartButton;


    //External refrences and others
    private GameObject Player;
    private PlayerController PlayerController;
    public FauxGravity FauxGravity;
    public Vector3 CurrentCheckPoint;

    // Start is called before the first frame update
    void Awake()
    {
        //Racer Name Generator
        string[] lines = System.IO.File.ReadAllLines(@"C:\Users\24CarterK\Unity Stuff\Gamer Game R\Assets\Names.txt");
        foreach (string line in lines)
        {
            string[] subs = line.Split(",");
            Names.Add(subs[0]);
        }

        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerController = Player.GetComponent<PlayerController>();
        TotalRacers.Add(Player);
    }

    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Timer = Time.time;
        TimerDisplay.text = "Timer: " +Timer;
        CheckpointDisplay.text = "Checkpoint: " +Vector3.Distance(Player.transform.position, CurrentCheckPoint);
        BoostDisplay.text = "Boost Gauge: " + PlayerController.BoostGauge;
        LapDisplay.text = "Lap: " +PlayerController.Lap;

        if (Input.GetKeyDown(KeyCode.Q) && TotalRacers.Count != 8)
        {
            SpawnRacer(Player.transform.position, Player.transform.rotation);
        }

        if (PlayerController.Lap == 3 || PlayerController.Lives == 0)
        {
            EndGame();
        }
    }

    //Spawns a new racer
    void SpawnRacer(Vector3 Position, Quaternion Rotation)
    {
        GameObject SpawnShip;

        ShipIndex = UnityEngine.Random.Range(0, 5);
        SpawnShip = ShipCatalog[ShipIndex];

        NameIndex = UnityEngine.Random.Range(0, Names.Count);
        SpawnShip.name = Names[NameIndex];
        Names.RemoveAt(NameIndex);

        Instantiate(SpawnShip, Position, Rotation);
        //AddRacer(SpawnShip);
    }

    /*Adds a racer to the system
    void AddRacer(GameObject Racer)
    {
        TotalRacers.Add(Racer);
        Debug.Log(Racer.name+ " has joined the race!");
      
    }

    private void RecallRacers()
    {
        Debug.Log("Starting racer recall...");
        foreach (GameObject racer in TotalRacers)
        {
            Debug.Log("There's " +racer.name);
        }
        Debug.Log("There are " +TotalRacers.Count+ " total racers!");
    }

    
        //Spawning the other racers
        SpawnRacer(new Vector3(-50f, 0.25f, 2.6f), new Quaternion(0, 180, 0, 0));
        SpawnRacer(new Vector3(-52f, 0.25f, 2.6f), new Quaternion(0, 180, 0, 0));
        SpawnRacer(new Vector3(-54f, 0.25f, 2.6f), new Quaternion(0, 180, 0, 0));

        SpawnRacer(new Vector3(-48f, 0.25f, 4.6f), new Quaternion(0, 180, 0, 0));
        SpawnRacer(new Vector3(-50f, 0.25f, 4.6f), new Quaternion(0, 180, 0, 0));
        SpawnRacer(new Vector3(-52f, 0.25f, 4.6f), new Quaternion(0, 180, 0, 0));
        SpawnRacer(new Vector3(-54f, 0.25f, 4.6f), new Quaternion(0, 180, 0, 0));

        //List all the racers
        RecallRacers();
    */

    public float TimeCut(float Deductable)
    {
        Timer = Timer -= Deductable;
        return Timer;
    }

    public void StartGame(int Difficulty)
    {
        switch (Difficulty)
        {
            case 1:
                Debug.Log("The Player has chosen the easy mode with normal speed");
                GameUI.SetActive(true);
                PlayerController.InControl = true;
                TitleUI.SetActive(false);
                Time.timeScale = 1;
                break;
            case 2:
                Debug.Log("The Player has chosen the medium mode with increased speed");
                //SpawnRacer(new Vector3(-51f, 0.25f, 6.6f), new Quaternion(0, 180, 0, 0));
                GameUI.SetActive(true);
                PlayerController.InControl = true;
                TitleUI.SetActive(false);
                Time.timeScale = 3;
                PlayerController.Lives *= 2;
                break;
            case 3:
                Debug.Log("The Player has chosen the hard mode with further increased speed");
                //SpawnRacer(new Vector3(-51f, 0.25f, 6.6f), new Quaternion(0, 180, 0, 0));
                //SpawnRacer(new Vector3(-52f, 0.25f, 6.6f), new Quaternion(0, 180, 0, 0));
                GameUI.SetActive(true);
                PlayerController.InControl = true;
                TitleUI.SetActive(false);
                Time.timeScale = 5;
                PlayerController.Lives *= 3;
                break;
        }
        GameActive = true;
    }

    public void EndGame()
    {
        GameOverUI.SetActive(true);
        GameActive = false;
        Time.timeScale = 0;
        PlayerController.InControl = false;
        CheckpointDisplay.text = "Checkpoint: 0";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
