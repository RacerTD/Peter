using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int Amount = 10;
    public AmmoType Type = AmmoType.DimA;
}

public enum AmmoType
{
    DimA,
    DimB
}