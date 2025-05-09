using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraScript : MonoBehaviour
{
    private GameObject Player;
    private Vector3 DefPos = new Vector3(0, 9f, 0);
    public GameObject Planet;
    public GameManager GameManager;

    public AudioClip[] Music;
    private AudioSource CameraAs;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        CameraAs = GetComponent<AudioSource>();
        CameraAs.loop = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 PlayerPos = Player.transform.position;
        Quaternion PlayerRot = Player.transform.rotation;
        Transform PlayerTrans = Player.transform;

        transform.position = DefPos + PlayerPos;

        if (!CameraAs.isPlaying && GameManager.GameActive)
        {
            CameraAs.clip = GetRandomClip();
            CameraAs.Play();
        }
    }

    private AudioClip GetRandomClip()
    {
        int Index = Random.Range(0, Music.Length);
        Debug.Log("Now playing " +Music[Index].name);
        return Music[Index];
    }
}

