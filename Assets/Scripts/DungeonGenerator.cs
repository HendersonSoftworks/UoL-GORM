using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private enum agentDirections { up, down, left, right };

    [Header("Config")]
    [SerializeField]
    int minRoomCount = 5;
    [SerializeField]
    int maxRoomCount = 7;
    [SerializeField]
    private float generationDelayInSecods = 1;
    [SerializeField]
    private int maxSteps = 100;
    [SerializeField]
    private agentDirections agentDirection = agentDirections.right;
    [SerializeField]
    private int agentStepDistance = 1;
    [SerializeField][Range(0, 100)]
    private float changeDirPercentage = 0;
    [SerializeField][Range(0, 100)]
    private float placeRoomPercentage = 0;
    
    [Header("Lists")]
    public List<GameObject> corridors;
    public List<GameObject> rooms;
    public List<GameObject> walls;
    
    [Header("Prefabs")]
    [SerializeField]
    private GameObject roomPrefab;
    [SerializeField]
    private GameObject corridorPrefab;
    [SerializeField]
    private GameObject wallTopPrefab;
    [SerializeField]
    private GameObject wallBottomPrefab;
    [SerializeField]
    private GameObject wallSidePrefab;

    [Header("References")]
    [SerializeField]
    private TileOutlineGenerator outlineGenerator;

    void Start()
    {
        Generate();
    }

    private void Generate()
    {    
        //StartCoroutine(InvokeDelayedPlacement());
        InvokePlacement();

        foreach (var item in rooms)
        {
            if (item != null)
            {
                outlineGenerator.tilePositions.Add(new Vector2Int(Mathf.RoundToInt(item.transform.position.x),
                    Mathf.RoundToInt(item.transform.position.y)));
            }
        }
        foreach (var item in corridors)
        {
            if (item != null)
            {
                outlineGenerator.tilePositions.Add(new Vector2Int(Mathf.RoundToInt(item.transform.position.x),
                    Mathf.RoundToInt(item.transform.position.y)));
            }
        }

        outlineGenerator.StartOutlineGen();
    }

    private void InvokePlacement()
    {
        for (int i = 0; i < maxSteps; i++)
        {
            PlaceCorridor();
            PlaceRoom();
            MoveAgent();
            ChooseRandomDirection();
        }
    }

    private IEnumerator InvokeDelayedPlacement()
    {
        for (int i = 0; i < maxSteps; i++)
        {            
            PlaceCorridor();
            PlaceRoom();
            MoveAgent();
            ChooseRandomDirection();

            yield return new WaitForSeconds(generationDelayInSecods);
        }
    }

    private void ChooseRandomDirection()
    {
        // If random roll doesn't meet percentage, return
        float randFloat = UnityEngine.Random.Range(0f, 100f);
        if (randFloat > changeDirPercentage)
        {
            return;
        }

        // Otherwise, change direction and reset percentage chance
        // Added OR cases so that the agent cannot go back on itself
        int randInt = UnityEngine.Random.Range(0, 4);
        switch (randInt)
        {
            case 0:
                if (agentDirection == agentDirections.up ||
                    agentDirection == agentDirections.down)
                {
                    agentDirection = agentDirections.left;
                    return;
                }

                agentDirection = agentDirections.up;
                break;
            case 1:
                if (agentDirection == agentDirections.down ||
                    agentDirection == agentDirections.up)
                {
                    agentDirection = agentDirections.right;
                    return;
                }

                agentDirection = agentDirections.down;
                break;
            case 2:
                if (agentDirection == agentDirections.left ||
                    agentDirection == agentDirections.right)
                {
                    agentDirection = agentDirections.up;
                    return;
                }

                agentDirection = agentDirections.left;
                break;
            case 3:
                if (agentDirection == agentDirections.right ||
                    agentDirection == agentDirections.left)
                {
                    agentDirection = agentDirections.down;
                    return;
                }

                agentDirection = agentDirections.right;
                break;
            default:
                print("ERROR");
                break;
        }

        changeDirPercentage = 0f;
    }

    private void PlaceCorridor()
    {
        // instantiate the corridor
        var newCor = Instantiate(corridorPrefab, transform.position, Quaternion.identity);
        BoundsInt newBoundsInt = new BoundsInt((int)newCor.transform.position.x, (int)newCor.transform.position.y, 0,
            (int)newCor.transform.localScale.x, (int)newCor.transform.localScale.y, 0);

        //newWall.transform.localScale = new Vector3(newWall.transform.localScale.x, newWall.transform.localScale.y, 0);
        newCor.GetComponent<CorridorData>().bounds = newBoundsInt;

        corridors.Add(newCor);

        // Increase chance of dir change & room placement
        changeDirPercentage += 1;
        placeRoomPercentage += 1;

        // Place top wall
        //PlaceWall(wallTopPrefab, new Vector2(transform.position.x, transform.position.y + 2));
        // Place bottom wall
        //PlaceWall(wallBottomPrefab, new Vector2(transform.position.x, transform.position.y - 2));
        // Place left wall
        //PlaceWall(wallSidePrefab, new Vector2(transform.position.x + 2, transform.position.y));
        // Place right wall
        //PlaceWall(wallSidePrefab, new Vector2(transform.position.x - 2, transform.position.y));

    }

    private void PlaceWall(GameObject _wallType, Vector2 _pos)
    {
        var newWall = Instantiate(_wallType, _pos, Quaternion.identity);

        BoundsInt newBoundsInt = new BoundsInt((int)newWall.transform.position.x, (int)newWall.transform.position.y, 0,
            (int)newWall.transform.localScale.x, (int)newWall.transform.localScale.y, 0);

        //newWall.transform.localScale = new Vector3(newWall.transform.localScale.x, newWall.transform.localScale.y, 0);
        newWall.GetComponent<WallData>().bounds = newBoundsInt;

        walls.Add(newWall);
        EvaluateWallOverlaps(newWall);
    }

    private void EvaluateWallOverlaps(GameObject _newWall)
    {
        if (_newWall == null)
        {
            return;
        }

        // Check wall doesn't cross with any other wall
        foreach (var wall in walls)
        {
            if (wall != null)
            {
                return;
            }
            
            if (BoundsIntOverlap(_newWall.GetComponent<WallData>().bounds, wall.GetComponent<WallData>().bounds))
            {
                print("Wall clash with wall");
                Destroy(_newWall);
                return;
            }
        }
        // Check wall doesn't cross with any other room
        foreach (var room in rooms)
        {
            if (BoundsIntOverlap(
                _newWall.GetComponent<WallData>().bounds,
                room.GetComponent<RoomData>().bounds) && _newWall != null)
            {
                print("Wall clash with room");
                Destroy(_newWall);
                return;
            }
        }
        // Check wall doesn't cross with any other corridor
        foreach (var corridor in corridors)
        {
            if (BoundsIntOverlap(
                _newWall.GetComponent<WallData>().bounds,
                corridor.GetComponent<CorridorData>().bounds) && _newWall != null)
            {
                print("Wall clash with corridor");
                Destroy(_newWall);
                return;
            }
        }
    }

    private void PlaceRoom()
    {
        int roomW = 7;
        int roomH = 5;

        // If random roll doesn't meet percentage, return
        float randFloat = UnityEngine.Random.Range(0f, 100f);
        if (randFloat > placeRoomPercentage)
        {
            return;
        }

        var newRoom = Instantiate(roomPrefab, transform.position, Quaternion.identity);

        BoundsInt newBoundsInt = new BoundsInt((int)transform.position.x, (int)transform.position.y, 0,
            roomW, roomH, 0);

        newRoom.transform.localScale = new Vector3(roomW, roomH, 0);
        newRoom.GetComponent<RoomData>().bounds = newBoundsInt;

        // instantiate new room
        foreach (var room in rooms)
        {
            // Check room doesn't cross with any other room
            if (BoundsIntOverlap(
                newRoom.GetComponent<RoomData>().bounds,
                room.GetComponent<RoomData>().bounds))
            {
                print("Room clash - removing room");
                Destroy(newRoom);
                return;
            }
        }
        
        rooms.Add(newRoom);
        placeRoomPercentage = 0;
    }

    private void MoveAgent()
    {
        switch (agentDirection)
        {
            case agentDirections.up:
                transform.Translate(Vector2.up * + agentStepDistance);
                break;
            case agentDirections.down:
                transform.Translate(Vector2.down * +agentStepDistance);
                break;
            case agentDirections.left:
                transform.Translate(Vector2.left * +agentStepDistance);
                break;
            case agentDirections.right:
                transform.Translate(Vector2.right * +agentStepDistance);
                break;
            default:
                print("ERROR");
                break;
        }
    }

    public bool BoundsIntOverlap(BoundsInt a, BoundsInt b)
    {
        bool xOverlap = a.x < b.x + b.size.x && a.x + a.size.x > b.x;
        bool yOverlap = a.y < b.y + b.size.y && a.y + a.size.y > b.y;

        return xOverlap && yOverlap;
    }
}
