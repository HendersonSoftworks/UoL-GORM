using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private enum agentDirections { up, down, left, right };

    [Header("Cleanup")]
    [SerializeField]
    private List<Vector2> agentPath = new List<Vector2>();
    [SerializeField] 
    private float cleanupRaycastDistance = 2f;
    [SerializeField] 
    private LayerMask wallLayerMask;
    [SerializeField]
    float rayRadius = 1f;

    [Header("Dungeon Colours")]
    [SerializeField]
    private Color dungeonCol;
    [SerializeField]
    private Color swampCol;
    [SerializeField]
    private Color infernalCol;
    [SerializeField]
    private Color dreamCol;

    [Header("Config")]
    [SerializeField]
    private float wallDistFromAgent = 1.51f;
    //[SerializeField]
    //private float generationDelayInSecods = 1;
    [SerializeField]
    private int maxSteps = 100;
    [SerializeField]
    private int currentStep = 0;
    [SerializeField]
    private agentDirections agentDirection = agentDirections.right;
    [SerializeField]
    private agentDirections agentPrevDirection = agentDirections.right;

    [SerializeField]
    private int agentStepDistance = 1;
    [SerializeField][Range(0, 100)]
    private float changeDirPercentage = 0;
    [SerializeField][Range(0, 100)]
    private float placeRoomPercentage = 0;
    [SerializeField]
    private bool delayPlaceWalls = false;

    [Header("Lists")]
    public List<GameObject> corridors;
    public List<GameObject> rooms;
    public List<GameObject> walls;
    public List<GameObject> chests;
    public List<GameObject> enemies;

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
    private FollowPlayerAndRescan setBoundsAndScan;
    [SerializeField]
    private DungeonPlacer dungeonPlacer;
    [SerializeField]
    private GameManager gameManager;

    void Start()
    {
        dungeonPlacer = GetComponent<DungeonPlacer>();
        gameManager = FindFirstObjectByType<GameManager>();

        InvokePlacement();
        CleanupExtraWalls();
        
        setBoundsAndScan.SetGraphBoundsAndRescan();
        
        dungeonPlacer.PlaceStairs();
        dungeonPlacer.RandomlyPlaceEnemies(rooms, corridors);
        dungeonPlacer.RandomlyPlaceRoomChests(rooms);
        dungeonPlacer.RandomlyPlaceCorridorChests(corridors, 2);
        dungeonPlacer.PopulateChestItems(chests);
        dungeonPlacer.ReplaceDuplicateChestItems(chests);

        SetCorridorTexture(corridors);
        SetWallTexture(walls);
        SetRoomTexture(rooms);
    }

    private void SetEnemyTarget()
    {
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<EnemyMovementController>().SetPlayerObject();
        }
    }

    private void ProcessGenerationStep()
    {
        PlaceCorridor();
        PlaceRoom();
        MoveAgent();
        ChooseRandomDirection();
    }

    private void InvokePlacement()
    {
        for (int i = 0; i < maxSteps; i++)
        {
            ProcessGenerationStep();
        }
    }

    private void CleanupExtraWalls()
    {
        // Corridor walls
        if (agentPath.Count < 2) return;

        for (int i = agentPath.Count - 1; i > 0; i--)
        {
            Vector2 currentPos = agentPath[i];
            Vector2 direction = (agentPath[i - 1] - currentPos).normalized;

            RaycastHit2D[] hits = Physics2D.CircleCastAll(
                currentPos,
                rayRadius,
                direction,
                cleanupRaycastDistance,
                wallLayerMask
            );

            foreach (var hit in hits)
            {
                if (hit.collider != null && walls.Contains(hit.collider.gameObject))
                {
                    GameObject wall = hit.collider.gameObject;
                    walls.Remove(wall);
                    Destroy(wall);
                }
            }

            // Debug visualization
            Debug.DrawRay(currentPos, direction * cleanupRaycastDistance, Color.red, 2f);
            Debug.DrawLine(currentPos + Vector2.Perpendicular(direction) * rayRadius,
                           currentPos + direction * cleanupRaycastDistance + Vector2.Perpendicular(direction) * rayRadius,
                           Color.blue, 2f);
        }

        // Corridor walls in rooms
        CleanCorridorWallsInRooms();
    }

    private void CleanCorridorWallsInRooms()
    {
        // TODO
        // 1, Agent backtracks and reaches middle of room
        // 2. Fire boxcast out and delete all walls with boxcast

        // Iterate through all rooms
        foreach (GameObject room in rooms)
        {
            if (room == null) continue;

            // Get room bounds
            BoundsInt roomBounds = room.GetComponent<RoomData>().bounds;
            int xOffset = 0;
            int yOffset = 0;
            Vector2 roomCenter = new Vector2(roomBounds.x + xOffset,
                                            roomBounds.y + yOffset);

            // Calculate boxcast size based on room dimensions
            Vector2 boxSize = new Vector2(3, 3);

            // Fire boxcast in all four directions from room center
            // We'll use a small distance since we're casting from center to edges
            float castDistance = Mathf.Max(roomBounds.size.x, roomBounds.size.y) / 2f;

            // Cast in all four directions (though any direction should work for a boxcast)
            RaycastHit2D[] hits = Physics2D.BoxCastAll(
                roomCenter,
                boxSize,
                0f,
                Vector2.zero,
                castDistance,
                wallLayerMask
            );

            // Remove any walls found inside the room
            foreach (var hit in hits)
            {
                if (hit.collider != null && walls.Contains(hit.collider.gameObject))
                {
                    GameObject wall = hit.collider.gameObject;
                    walls.Remove(wall);
                    Destroy(wall);
                }
            }

            // Debug visualization
            Debug.DrawLine(roomCenter - new Vector2(boxSize.x / 2, boxSize.y / 2),
                          roomCenter + new Vector2(boxSize.x / 2, boxSize.y / 2),
                          Color.cyan, 60f);
        }
    }

    private void PlaceTurnWall()
    {
        delayPlaceWalls = true;

        // Place wall when agent turns
        switch (agentDirection)
        {
            case agentDirections.up:
                // Place bottom wall
                PlaceWall(wallBottomPrefab, new Vector2(transform.position.x, transform.position.y - wallDistFromAgent));

                // Place turn wall depending on previous direction
                if (agentPrevDirection == agentDirections.left)
                {
                    // Place left wall
                    PlaceWall(wallSidePrefab, new Vector2(transform.position.x - wallDistFromAgent, transform.position.y));
                }
                else if (agentPrevDirection == agentDirections.right)
                {
                    // Place right wall
                    PlaceWall(wallSidePrefab, new Vector2(transform.position.x + wallDistFromAgent, transform.position.y));
                }

                break;
            case agentDirections.down:
                // Place top wall
                PlaceWall(wallTopPrefab, new Vector2(transform.position.x, transform.position.y + wallDistFromAgent));

                // Place turn wall depending on previous direction
                if (agentPrevDirection == agentDirections.left)
                {
                    // Place left wall
                    PlaceWall(wallSidePrefab, new Vector2(transform.position.x - wallDistFromAgent, transform.position.y));
                }
                else if (agentPrevDirection == agentDirections.right)
                {
                    // Place right wall
                    PlaceWall(wallSidePrefab, new Vector2(transform.position.x + wallDistFromAgent, transform.position.y));
                }

                break;
            case agentDirections.left:
                // Place right wall
                PlaceWall(wallSidePrefab, new Vector2(transform.position.x + wallDistFromAgent, transform.position.y));

                // Place turn wall depending on previous direction
                if (agentPrevDirection == agentDirections.up)
                {
                    // Place top wall
                    PlaceWall(wallTopPrefab, new Vector2(transform.position.x, transform.position.y + wallDistFromAgent));
                }
                else if (agentPrevDirection == agentDirections.down)
                {
                    // Place bottom wall
                    PlaceWall(wallBottomPrefab, new Vector2(transform.position.x, transform.position.y - wallDistFromAgent));
                }

                break;
            case agentDirections.right:
                // Place left wall
                PlaceWall(wallSidePrefab, new Vector2(transform.position.x - wallDistFromAgent, transform.position.y));

                // Place turn wall depending on previous direction
                if (agentPrevDirection == agentDirections.up)
                {
                    // Place top wall
                    PlaceWall(wallTopPrefab, new Vector2(transform.position.x, transform.position.y + wallDistFromAgent));
                }
                else if (agentPrevDirection == agentDirections.down)
                {
                    // Place bottom wall
                    PlaceWall(wallBottomPrefab, new Vector2(transform.position.x, transform.position.y - wallDistFromAgent));
                }

                break;
            default:
                break;
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

        // Store previous direction
        agentPrevDirection = agentDirection;

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
                    PlaceTurnWall();
                    return;
                }

                agentDirection = agentDirections.up;
                PlaceTurnWall();
                break;
            case 1:
                if (agentDirection == agentDirections.down ||
                    agentDirection == agentDirections.up)
                {
                    agentDirection = agentDirections.right;
                    PlaceTurnWall();
                    return;
                }

                agentDirection = agentDirections.down;
                PlaceTurnWall();
                break;
            case 2:
                if (agentDirection == agentDirections.left ||
                    agentDirection == agentDirections.right)
                {
                    agentDirection = agentDirections.up;
                    PlaceTurnWall();
                    return;
                }

                agentDirection = agentDirections.left;
                PlaceTurnWall();
                break;
            case 3:
                if (agentDirection == agentDirections.right ||
                    agentDirection == agentDirections.left)
                {
                    agentDirection = agentDirections.down;
                    PlaceTurnWall();
                    return;
                }

                agentDirection = agentDirections.right;
                PlaceTurnWall();
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

        newCor.GetComponent<CorridorData>().bounds = newBoundsInt;

        corridors.Add(newCor);

        // Increase chance of dir change & room placement
        changeDirPercentage += 1;
        placeRoomPercentage += 1;

        if (currentStep == 0)
        {
            //Place left wall
            PlaceWall(wallSidePrefab, new Vector2(transform.position.x - wallDistFromAgent, transform.position.y));
        }

        // Do not place walls when a turn is made
        if (delayPlaceWalls)
        {
            delayPlaceWalls = false;
            return;
        }
        else if (agentDirection == agentDirections.right || agentDirection == agentDirections.left)
        {
            // Place top wall
            PlaceWall(wallTopPrefab, new Vector2(transform.position.x, transform.position.y + wallDistFromAgent));
            // Place bottom wall
            PlaceWall(wallBottomPrefab, new Vector2(transform.position.x, transform.position.y - wallDistFromAgent));
        }
        else if (agentDirection == agentDirections.up || agentDirection == agentDirections.down)
        {
            // Place left wall
            PlaceWall(wallSidePrefab, new Vector2(transform.position.x - wallDistFromAgent, transform.position.y));
            // Place right wall
            PlaceWall(wallSidePrefab, new Vector2(transform.position.x + wallDistFromAgent, transform.position.y));
        }
    }

    private void SetWallTexture(List<GameObject> walls)
    {
        foreach (var wall in walls)
        {
            var wallRenderer = wall.GetComponent<SpriteRenderer>();

            switch (gameManager.floorType)
            {
                case floorTypes.dungeon:
                    wallRenderer.color = dungeonCol;

                    break;
                case floorTypes.swamp:
                    wallRenderer.color = swampCol;

                    break;
                case floorTypes.infernal:
                    wallRenderer.color = infernalCol;

                    break;
                case floorTypes.dream:
                    wallRenderer.color = dreamCol;

                    break;

                default:
                    break;
            }
        }
    }

    private void SetCorridorTexture(List<GameObject> corridors)
    {
        foreach (var cor in corridors)
        {
            var corRenderer = cor.GetComponent<SpriteRenderer>();

            switch (gameManager.floorType)
            {
                case floorTypes.dungeon:
                    corRenderer.color = dungeonCol;

                    break;
                case floorTypes.swamp:
                    corRenderer.color = swampCol;

                    break;
                case floorTypes.infernal:
                    corRenderer.color = infernalCol;

                    break;
                case floorTypes.dream:
                    corRenderer.color = dreamCol;

                    break;

                default:
                    break;
            }
        }
    }

    private void SetRoomTexture(List<GameObject> rooms)
    {
        foreach (var room in rooms)
        {
            var roomRenderer = room.GetComponent<SpriteRenderer>();

            switch (gameManager.floorType)
            {
                case floorTypes.dungeon:
                    roomRenderer.color = dungeonCol;

                    break;
                case floorTypes.swamp:
                    roomRenderer.color = swampCol;

                    break;
                case floorTypes.infernal:
                    roomRenderer.color = infernalCol;

                    break;
                case floorTypes.dream:
                    roomRenderer.color = dreamCol;

                    break;

                default:
                    break;
            }
        }
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
                //print("Room clash - removing room");
                Destroy(newRoom);
                return;
            }
        }
        
        rooms.Add(newRoom);
        placeRoomPercentage = 0;

        // Place walls around the room
        PlaceRoomWalls(newRoom, roomW, roomH);
    }

    private void PlaceRoomWalls(GameObject room, int width, int height)
    {
        Vector2 roomOffset = new Vector2(room.transform.position.x,
            room.transform.position.y);

        // Calculate exact wall positions relative to room center
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        // Wall positions (1 unit outside the room)
        float leftWallX = roomOffset.x - halfWidth - 0.1f;
        float rightWallX = roomOffset.x + halfWidth + 0.1f;
        float topWallY = roomOffset.y + halfHeight + 0.1f;
        float bottomWallY = roomOffset.y - halfHeight - 0.1f;

        // Place top and bottom walls
        for (float x = roomOffset.x - halfWidth + 0.5f; x <= roomOffset.x + halfWidth - 0.5f; x += 1f)
        {
            // Check for corridors at top
            bool topWallNeeded = true;
            foreach (var corridor in corridors)
            {
                if (Vector2.Distance(corridor.transform.position, new Vector2(x, topWallY)) < 0.1f)
                {
                    topWallNeeded = false;
                    break;
                }
            }
            if (topWallNeeded)
            {
                PlaceWall(wallTopPrefab, new Vector2(x, topWallY));
            }

            // Check for corridors at bottom
            bool bottomWallNeeded = true;
            foreach (var corridor in corridors)
            {
                if (Vector2.Distance(corridor.transform.position, new Vector2(x, bottomWallY)) < 0.1f)
                {
                    bottomWallNeeded = false;
                    break;
                }
            }
            if (bottomWallNeeded)
            {
                PlaceWall(wallBottomPrefab, new Vector2(x, bottomWallY));
            }
        }

        // Place left and right walls
        for (float y = roomOffset.y - halfHeight + 0.5f; y <= roomOffset.y + halfHeight - 0.5f; y += 1f)
        {
            // Check for corridors at left
            bool leftWallNeeded = true;
            foreach (var corridor in corridors)
            {
                if (Vector2.Distance(corridor.transform.position, new Vector2(leftWallX, y)) < 0.1f)
                {
                    leftWallNeeded = false;
                    break;
                }
            }
            if (leftWallNeeded)
            {
                PlaceWall(wallSidePrefab, new Vector2(leftWallX, y));
            }

            // Check for corridors at right
            bool rightWallNeeded = true;
            foreach (var corridor in corridors)
            {
                if (Vector2.Distance(corridor.transform.position, new Vector2(rightWallX, y)) < 0.1f)
                {
                    rightWallNeeded = false;
                    break;
                }
            }
            if (rightWallNeeded)
            {
                PlaceWall(wallSidePrefab, new Vector2(rightWallX, y));
            }
        }
    }

    private void MoveAgent()
    {
        Vector2 previousPosition = transform.position;

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

        agentPath.Add(previousPosition);

        currentStep++;
    }

    public bool BoundsIntOverlap(BoundsInt a, BoundsInt b)
    {
        bool xOverlap = a.x < b.x + b.size.x && a.x + a.size.x > b.x;
        bool yOverlap = a.y < b.y + b.size.y && a.y + a.size.y > b.y;

        return xOverlap && yOverlap;
    }

    private void OnDrawGizmos()
    {
        foreach (var room in rooms)
        {

            //BoundsInt roomBounds = room.GetComponent<RoomData>().bounds;
            //Vector2 roomCenter = new Vector2(roomBounds.x + roomBounds.size.x / 2f - 2,
            //                                roomBounds.y + roomBounds.size.y / 2f - 2);

            //// Calculate boxcast size based on room dimensions
            //Vector2 boxSize = new Vector2(roomBounds.size.x - 1, roomBounds.size.y - 1);

            //// Debug visualization
            //Debug.DrawLine(roomCenter - new Vector2(boxSize.x / 2, boxSize.y / 2),
            //              roomCenter + new Vector2(boxSize.x / 2, boxSize.y / 2),
            //              Color.green, 2f);
        }
    }
}
