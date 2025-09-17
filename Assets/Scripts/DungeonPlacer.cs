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
            int randItemTypeInt = Random.Range(0, 2);
            //randItemTypeInt = 1;
            switch (randItemTypeInt)
            {
                case 0: // Place ring
                    int randRingInt = Random.Range(0, dungeonItems.ringsArr.Length);
                    _chest.GetComponent<Chest>().chestItem =
                        dungeonItems.ringsArr[randRingInt].GetComponent<Item>();
                    
                    break;
                case 1: // Place spell
                    int randSpellInt = Random.Range(0, dungeonItems.spellsArr.Length);
                    _chest.GetComponent<Chest>().chestItem =
                        dungeonItems.spellsArr[randSpellInt].GetComponent<Item>();

                    break;
                default:
                    Debug.LogError("Error placing chest item in: " + _chest.name);

                    break;
            }
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

    public void RandomlyPlaceEnemies(List<GameObject> _rooms, List<GameObject> _corridors)
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
                        int getRandEnemy = Random.Range(0, enemies.Count);
                        Instantiate(enemies[getRandEnemy], item.transform.position, Quaternion.identity);
                    }
                }
            }
        }

        foreach (var item in _corridors)
        {
            if (item != null)
            {
                float spawnChance = Random.Range(0f, 1f);

                if (spawnChance <= 0.1f) // only spawn in ~10% of corridors
                {
                    float dist = Vector2.Distance(item.transform.position, playerCharacter.transform.position);
                    if (dist >= 7) // check enemies not spawning in same room as player
                    {
                        int getRandEnemy = Random.Range(0, enemies.Count);
                        Instantiate(enemies[getRandEnemy], item.transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }
}
