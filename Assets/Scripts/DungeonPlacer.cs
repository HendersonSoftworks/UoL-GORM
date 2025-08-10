using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonPlacer : MonoBehaviour
{

    [Header("Setup - Loaded on start")]
    [SerializeField]
    private DungeonGenerator dungeonGenerator;

    [Header("Config")]
    [SerializeField]
    private List<GameObject> enemies;
    [SerializeField]
    private GameObject stairs;

    [Header("Difficulty Logic")]
    public uint maxNumberOfEnemiesInRooms = 1;

    void Start()
    {
        dungeonGenerator = GetComponent<DungeonGenerator>();
    }

    public void PlaceStairs()
    {
        Instantiate(stairs, transform.position, Quaternion.identity);
    }

    public void RandomlyPlaceEnemies()
    {
        foreach (var item in dungeonGenerator.rooms)
        {
            int randNumberOfEnemiesToSpawnInRoom = Random.Range(0, (int)maxNumberOfEnemiesInRooms+1);
            for (int i = 0; i < randNumberOfEnemiesToSpawnInRoom; i++)
            {
                Instantiate(enemies[0], item.transform.position, Quaternion.identity);
            }
        }
    }
}
