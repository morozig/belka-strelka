using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Wave : ScriptableObject
{
    public GameObject platoonUnitPrefab;
    public int platoonColumns;
    public int platoonRows;
    public bool isPlatoonFull;
    public GameObject roamingUnitPrefab;
    public int roamingUnitsCount;
}
