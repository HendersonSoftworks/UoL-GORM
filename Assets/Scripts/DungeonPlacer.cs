using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

        SetMaxNumberOfEnemiesInRooms();
    }

    public void PlaceStairs()
    {
        Instantiate(stairsPrefab, transform.position, Quaternion.identity);
    }

    public void PopulateChestItems(List<GameObject> _chests)
    {
        foreach (var _chest in _chests)
        {
            int randItemTypeInt = Random.Range(0, 2+1);
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
                case 2: // Place potion
                    int randPotionInt = Random.Range(0, dungeonItems.potionsArr.Length);
                    _chest.GetComponent<Chest>().chestItem =
                        dungeonItems.potionsArr[randPotionInt].GetComponent<Item>();

                    break;
                default:
                    Debug.LogError("Error placing chest item in: " + _chest.name);

                    break;
            }
        }
    }

    public void ReplaceDuplicateChestItems(List<GameObject> _chests)
    {
        // Bug - Seems to not work or error - cannot figure out why

        foreach (var _chest in _chests)
        {
            foreach (var _spell in playerCharacter.spells)
            {
                if (_spell == null) { continue; }

                if (_chest.GetComponent<Chest>().chestItem.itemName == _spell.itemName)
                {
                    print("Item replaced");

                    // Set item to Ring of Power if duplicate spell
                    // Seems broken
                    _chest.GetComponent<Chest>().chestItem = dungeonItems.ringsArr[6].GetComponent<Ring>();
                }
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

                if (Database.currentFloor == 2)
                {
                    _chance = 6;
                }
                else if (Database.currentFloor == 3)
                {
                    _chance = 7;
                }
                else if (Database.currentFloor == 4)
                {
                    _chance = 8;
                }

                if (dist >= 7 && (_randRoll <= _chance)) // check enemies not spawning in same room as player
                {
                    var _tempchest = Instantiate(chestPrefab, _cor.transform.position, Quaternion.identity);
                    dungeonGenerator.chests.Add(_tempchest);
                }
            }
        }
    }

    public void SetMaxNumberOfEnemiesInRooms()
    {
        if (Database.currentFloor == 1 || Database.currentFloor == 2)
        {
            maxNumberOfEnemiesInRooms = 1;
        }
        else if (Database.currentFloor == 3 || Database.currentFloor == 4)
        {
            maxNumberOfEnemiesInRooms = 2;
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

                        // Make sure room enemies are appropriate to difficulty
                        if ((Database.currentFloor == 1 || Database.currentFloor == 2) &&
                            getRandEnemy == 2)
                        {
                            getRandEnemy = 1;
                        }
                        
                        var enemy = Instantiate(enemies[getRandEnemy], item.transform.position, Quaternion.identity);
                        dungeonGenerator.enemies.Add(enemy);
                    }
                }
            }
        }

        foreach (var item in _corridors)
        {
            if (item != null)
            {
                float spawnRoll = 0.05f; // only spawn in ~5% of corridors
                float spawnChance = Random.Range(0f, 1f);

                if (Database.currentFloor == 4) // increase difficulty for last room
                {
                    spawnRoll = 0.075f;
                }

                if (spawnChance <= spawnRoll) 
                {
                    float dist = Vector2.Distance(item.transform.position, playerCharacter.transform.position);
                    if (dist >= 7) // check enemies not spawning in same room as player
                    {
                        int getRandEnemy = Random.Range(0, enemies.Count);
                        var enemy = Instantiate(enemies[getRandEnemy], item.transform.position, Quaternion.identity);
                        dungeonGenerator.enemies.Add(enemy);
                    }
                }
            }
        }
    }
}
