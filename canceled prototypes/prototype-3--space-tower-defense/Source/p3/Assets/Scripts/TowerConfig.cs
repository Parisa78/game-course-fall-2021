using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameConfs/TowerConf")]
public class TowerConfig : ScriptableObject
{
    public float radius;
    public float maxHitPoint;
    public float minHitPoint;
}
