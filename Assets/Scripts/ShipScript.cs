using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShipScript : MonoBehaviour
{
    //Compontents
    Rigidbody AiRb;

    //Refrences
    public ShipBase ShipBase;
    public FauxGravity GravityCore;
    public GameManager GameManager;
    private PlayerController PlayerController;
    private GameObject Player;

    //Elements
    public Vector3 ResPos = new Vector3();
    public Quaternion ResRot = new Quaternion();
    public bool GravityTilt = false;

    /*Race Mechanics
    public int Lap;
    */

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerController = Player.GetComponent<PlayerController>();

        AiRb = GetComponent<Rigidbody>();
        ResPos = AiRb.transform.position;
        ResRot = AiRb.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (GravityTilt)
        {
            GravityCore.Attract(transform);
        }
    }

    void LateUpdate()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerController = Player.GetComponent<PlayerController>();
        PlayerController.ResPos = ResPos;
        PlayerController.ResRot = ResRot;

        //Movement
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, GameManager.Difficulty * 0.15f * ShipBase.Speed * Time.deltaTime).normalized;
        transform.rotation = new Quaternion(Player.transform.rotation.x, Player.transform.rotation.y, Player.transform.rotation.z, 0);

        //Reset position
        if (transform.position.y < -1f)
        {
            Reset();
            Debug.Log(gameObject.name+ " fell too far down");
        }
    }

    //Collisions
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            Reset();
            Debug.Log(gameObject.name+ " touched the planet... resetting position and rotation to most recent checkpoint");
        }

        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Racer"))
        {
            Rigidbody OtherRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 Knockback = other.gameObject.transform.position - transform.position;
            Vector3 SelfKnockback =  transform.position - other.gameObject.transform.position;

            OtherRigidbody.AddForce(Knockback * GameManager.Difficulty * 6, ForceMode.Impulse);
            AiRb.AddForce(SelfKnockback * 3, ForceMode.Impulse);
        }
    }

    void Reset()
    {
        AiRb.isKinematic = true;
        transform.position = ResPos;
        transform.rotation = ResRot;
        AiRb.isKinematic = false;
    }
}
