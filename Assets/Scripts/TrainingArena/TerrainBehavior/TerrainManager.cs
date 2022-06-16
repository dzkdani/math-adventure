using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public string PreviousSet;

    public TerrainIDList SetList;
    public GameObject PlaceHolderTerrain;
    public List<SpawnedSet> spawnedSet;
    public List<int> listNumbers;
    public int SetPassed = 0;
    public int maxSet;

    float OffsetPositionZ = 0;
    bool firstSpawnedSet = true;
    string LastSet;

    [System.Serializable]
    public class SpawnedSet
    {
        public string name;
        public float PanjangSet;
        public float FirstJarakTerrain, LastJarakTerrain;
        [SerializeField]GameObject TerrainParent;
        [SerializeField]List<SpawnedTerrain> Terrain = new List<SpawnedTerrain>();

        public SpawnedSet(string name){this.name = name;}
        public void AddTerrain(GameObject terrain, TerrainBehavior info){ Terrain.Add(new SpawnedTerrain(terrain, info)); }
        public List<SpawnedTerrain> GetAllTerrain() { return Terrain; }
        public GameObject GetTerrainAt(int a) { return Terrain[a].Object; }
        public void PlaceSetInParent(GameObject t) { TerrainParent.transform.parent = t.transform; }
        public Transform GetTerrainParentTransform() { return TerrainParent.transform; }
        public GameObject GetTerrainParent() { return TerrainParent; }
        public void AddTerrainParent(GameObject t) { t.name = this.name; TerrainParent = t; }
        public void ChangePosition(float z, float x=0, float y=0){ TerrainParent.transform.position = new Vector3(x, y, z); }
        public void ResetTerrainBehaviors() { Terrain.ForEach(x => { x.info.ResetTerrain(); });}
    }

    [System.Serializable]
    public class SpawnedTerrain
    {
        public GameObject Object;
        public TerrainBehavior info;

        public SpawnedTerrain(GameObject terrain, TerrainBehavior info) { Object = terrain; this.info = info; }
    }

    public TerrainIDList.TerrainID GetPrefab(int Set, int index)
    {
        int idPrefab = SetList.terrainSets[Set].tSets[index];
        return SetList.terrainIDs[idPrefab];
    }

    public int TerrainSetsCapacity(){ return SetList.terrainSets.Capacity; }
    public int PerTerrainSetCapacity(int Set) { return SetList.terrainSets[Set].tSets.Capacity; }

    public SpawnedSet ConjureTerrain(int indexSet, GameObject parent, int indexSpawnedSet)
    {
        float OffsetPositionZ = 0;
        SpawnedSet newSet = new SpawnedSet("Set " + indexSet);
        newSet.AddTerrainParent(parent);
        bool first = true;
        for (int i = 0; i < PerTerrainSetCapacity(indexSet); i++)
        {
            TerrainIDList.TerrainID temp = GetPrefab(indexSet, i);
            GameObject newTerrain = Instantiate(temp.terrain, parent.transform);
            newTerrain.name = "Set " + indexSet + "+" +i+"+"+temp.nama;

            TerrainBehavior info = newTerrain.GetComponentInChildren<TerrainBehavior>();
            
            float distance = OffsetPositionZ;
            distance += temp.jarak;

            if (first.Equals(true)) { distance = 0; newSet.FirstJarakTerrain = temp.jarak; first = false; }
            newSet.LastJarakTerrain = temp.jarak;

            newTerrain.transform.SetPositionAndRotation(new Vector3(0, 0, distance), Quaternion.identity);
            newSet.AddTerrain(newTerrain, info);
            OffsetPositionZ = distance + temp.jarak;
        }
        newSet.PanjangSet = OffsetPositionZ;
        return newSet;
    }

    public SpawnedSet ArrangeTheSets(SpawnedSet newSet, int i)
    {
        float distance = 15;
        if (firstSpawnedSet.Equals(false))
        {
            int index = i - 1;
            distance = OffsetPositionZ + spawnedSet[index].FirstJarakTerrain;
            OffsetPositionZ += (newSet.PanjangSet+newSet.FirstJarakTerrain);
        }
        else { firstSpawnedSet = false; OffsetPositionZ += newSet.PanjangSet; }
        newSet.ChangePosition(distance);

        return newSet;
    }

    public void ConjureSet()
    {
        OffsetPositionZ = 15;
        for (int i = 0; i < TerrainSetsCapacity(); i++)
        {
            GameObject temp = new GameObject();
            SpawnedSet newSet = ConjureTerrain(listNumbers[i], temp, i);
            newSet = ArrangeTheSets(newSet, i);
            newSet.PlaceSetInParent(PlaceHolderTerrain);

            if (i.Equals(0)) PreviousSet = newSet.name;

            spawnedSet.Add(newSet);
        }
    }
    
    public void GenerateUniqueRandomNumber()
    {
        int number;
        for (int i = 0; i < TerrainSetsCapacity(); i++)
        {
            do
            {
                number = Random.Range(0, TerrainSetsCapacity());
            } while (listNumbers.Contains(number));
            listNumbers.Add(number);
        }
    }

    public void CheckSet(string curr)
    {
        //Debug.Log("curr " + curr + " Previous " + PreviousSet + " last " + LastSet);
        if(curr != PreviousSet && curr !="")
        {
            SetPassed++;
            if (SetPassed >= 2)
            {
                SetPassed = 0;
                for (int i = 0; i < TerrainSetsCapacity(); i++)
                {
                    if (spawnedSet[i].name.Equals(LastSet))
                    {
                        SpawnedSet temp = spawnedSet[i];
                        spawnedSet.RemoveAt(i);
                        temp = ArrangeTheSets(temp, TerrainSetsCapacity()-1);
                        temp.ResetTerrainBehaviors();
                        spawnedSet.Add(temp);

                        LastSet = PreviousSet;
                        PreviousSet = curr;
                        TurnOFFSets();
                        TurnONSets();
                        break;
                    }
                }
            }
            else
            {
                LastSet = PreviousSet;
                PreviousSet = curr;
            }
        } 
    }

    void TurnOFFSets()
    {
        if (TerrainSetsCapacity() > maxSet)
        {
            for (int i = maxSet; i < TerrainSetsCapacity(); i++) { spawnedSet[i].GetTerrainParent().SetActive(false); }
        }
    }
    void TurnONSets()
    {
        for (int i = 0; i < maxSet; i++) { spawnedSet[i].GetTerrainParent().SetActive(true); }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetList = TrainingArenaSettingManager.Instance.SetList;
        GenerateUniqueRandomNumber();
        ConjureSet();
        TurnOFFSets();
    }

}
