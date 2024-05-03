using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Empty,
    Ball,
    Barrel,
    Stone,
    Box,
    Dynamit,
    Star
}
public class Item : MonoBehaviour
{
    public ItemType ItemType;
}
