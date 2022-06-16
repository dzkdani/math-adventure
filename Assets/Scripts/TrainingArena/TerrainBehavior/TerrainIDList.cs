using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;

[CreateAssetMenu(fileName = "Terrain_ID_List", menuName = "TrainingArena/TerrainIDList")]
public class TerrainIDList : ScriptableObject
{
    [Header("TERRAIN ID")]
    [Space(5)]
    public List<TerrainID> terrainIDs;

    [Header("TERRAIN ID SETS")]
    [Space(5)]
    public List<terrainSet> terrainSets;

    [System.Serializable]
    public class TerrainID
    {
        public GameObject terrain;
        public string nama;
        public float jarak;
    }

    [System.Serializable]
    public class terrainSet
    {
        public List<int> tSets;
    }
}
