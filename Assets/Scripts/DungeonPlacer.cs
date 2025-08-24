using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonPlacer : MonoBehaviour
{

    [Header("Setup - Loaded on start")]
    [SerializeField]
    private DungeonGenerator dungeonGenerator;
    [SerializeField]
    private PlayerCharacter playerCharacter;
    [SerializeField]
    private DungeonItems dungeonItems;

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
        dungeonGenerator = GetComponent<DungeonGenerator>();
        dungeonItems = GetComponent<DungeonItems>();
        playerCharacter = FindFirstObjectByType<PlayerCharacter>();
    }

    public void PlaceStairs()
    {
        Instantiate(stairsPrefab, transform.position, Quaternion.identity);
    }

    public void PopulateChestItems(List<GameObject> _chests)
    {
        foreach (var _chest in _chests)
        {
            int randRingInt = Random.Range(0, dungeonItems.ringsArr.Length);
            _chest.GetComponent<Chest>().chestItem = 
                dungeonItems.ringsArr[randRingInt].GetComponent<Item>();
        }
    }

    public void RandomlyPlaceRoomChests(List<GameObject> _rooms, int _chance = 25)
    {
        foreach (var _room in _rooms)
        {
            if (_room != null)
            {
                int _randRoll = Random.Range(0, 100);
                float dist = Vector2.Distance(_room.transform.position, playerCharacter.transform.position);
                if (dist >= 7 && (_randRoll <= _chance)) // check enemies not spawning in same room as player
                {
                    var _tempchest = Instantiate(chestPrefab, _room.transform.position, Quaternion.identity);
                    dungeonGenerator.chests.Add(_tempchest);
                }
            }
        }
    }

    public void RandomlyPlaceCorridorChests(List<GameObject> _corridors, int _chance = 5)
    {
        foreach (var _cor in _corridors)
        {
            if (_cor != null)
            {
                int _randRoll = Random.Range(0, 100);
                float dist = Vector2.Distance(_cor.transform.position, playerCharacter.transform.position);
                if (dist >= 7 && (_randRoll <= _chance)) // check enemies not spawning in same room as player
                {
                    var _tempchest = Instantiate(chestPrefab, _cor.transform.position, Quaternion.identity);
                    dungeonGenerator.chests.Add(_tempchest);
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
