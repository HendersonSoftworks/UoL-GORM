using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private enum agentDirections { up, down, left, right };

    [SerializeField]
    private float generationDelayInSecods = 1;
    [SerializeField]
    private GameObject roomPrefab;
    [SerializeField]
    private GameObject corridorPrefab;
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
    [SerializeField]
    private List<GameObject> corridors;
    [SerializeField]
    private List<GameObject> rooms;

    void Start()
    {
        Generate();
    }

    private void Generate()
    {    
        StartCoroutine(InvokeDelayedPlacement());   
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
        corridors.Add(newCor);

        // Increase chance of dir change & room placement
        changeDirPercentage += 1;
        placeRoomPercentage += 1;
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

    private bool BoundsIntOverlap(BoundsInt a, BoundsInt b)
    {
        bool xOverlap = a.x < b.x + b.size.x && a.x + a.size.x > b.x;
        bool yOverlap = a.y < b.y + b.size.y && a.y + a.size.y > b.y;

        return xOverlap || yOverlap;
    }
}
