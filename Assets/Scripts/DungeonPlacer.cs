using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonPlacer : MonoBehaviour
{

    [Header("Setup - Loaded on start")]
    //[SerializeField]
    //private DungeonGenerator dungeonGenerator;
    [SerializeField]
    private PlayerCharacter playerCharacter;

    [Header("Config")]
    [SerializeField]
    private List<GameObject> enemies;
    [SerializeField]
    private GameObject stairsPrefab;
    [SerializeField]
    private GameObject chestPrefab;

    [Header("Difficulty Logic")]
    public uint maxNumberOfEnemiesInRooms = 1;

    void Start()
    {
        //dungeonGenerator = GetComponent<DungeonGenerator>();
        playerCharacter = FindFirstObjectByType<PlayerCharacter>();
    }

    public void PlaceStairs()
    {
        Instantiate(stairsPrefab, transform.position, Quaternion.identity);
    }

    public void RandomlyPlaceChests(List<GameObject> _rooms, List<GameObject> _corridors)
    {
        float _chestSpawnChanceRooms = 1f;
        float _chestSpawnChanceCorridors = 1f;

        // Place chests in rooms
        foreach (var _room in _rooms)
        {
            float _chestSpawnChanceRoll = Random.Range(0f, 1f);

            if (_room == null && (_chestSpawnChanceRoll <= _chestSpawnChanceRooms))
            {
                float dist = Vector2.Distance(_room.transform.position, playerCharacter.transform.position);
                if (dist >= 7) // check enemies not spawning in same room as player
                {
                    Instantiate(chestPrefab, _room.transform.position, Quaternion.identity);
                    print("chest placed in room.");
                }
            }
        }

        // Place chests in hallways
        foreach (var _corridor in _corridors)
        {
            float _chestSpawnChanceRoll = Random.Range(0f, 1f);

            if (_corridor == null && (_chestSpawnChanceRoll <= _chestSpawnChanceCorridors))
            {
                float dist = Vector2.Distance(_corridor.transform.position, playerCharacter.transform.position);
                if (dist >= 7) // check enemies not spawning in same room as player
                {
                    Instantiate(chestPrefab, _corridor.transform.position, Quaternion.identity);
                    print("chest placed in corridor.");
                }
            }
        }
    }

    public void RandomlyPlaceEnemies(List<GameObject> _rooms)
    {
        foreach (var item in _rooms)
        {
            if (item != null)
            {
                int randNumberOfEnemiesToSpawnInRoom = Random.Range(0, (int)maxNumberOfEnemiesInRooms + 1);
                for (int i = 0; i < randNumberOfEnemiesToSpawnInRoom; i++)
                {
                    float dist = Vector2.Distance(item.transform.position, playerCharacter.transform.position);
                    if (dist >= 7) // check enemies not spawning in same room as player
                    {
                        Instantiate(enemies[0], item.transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }
}
