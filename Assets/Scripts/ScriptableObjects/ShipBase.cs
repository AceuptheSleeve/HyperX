using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects", order = 1)]
public class ShipBase : ScriptableObject
{
    //Stats
    public string ShipName;
    public float Speed;
    public int Id;
    public float TurnSpeed;
    public int BoostFactor;
}
