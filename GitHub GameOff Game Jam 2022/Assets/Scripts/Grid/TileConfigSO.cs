using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tile/New TileConfigSO")]
public class TileConfigSO : ScriptableObject
{
    [Header("harvest notification sound effects")]
    [SerializeField] public EventReference livestockHarvestFmodEventReference;
    [SerializeField] public EventReference cropHarvestFmodEventReference;

}
