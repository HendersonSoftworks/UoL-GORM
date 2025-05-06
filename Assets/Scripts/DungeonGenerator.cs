using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    /*
    Start with rect
    split recursively (rand, Horiz or Vert)
    Choose random pos (X for vert, y for H)
    Split dungeon into two sub-dungeons

    map size
    iterations
    room vertical ratio
    room horizntal ratio
    draw rooms
    draw corridors
    */
    private bool?[,] map;

    private class Tree
    {
        // treenode root
        // list tree node leafs
    }

    private class Node
    {
        // treenode parent
        // treenode children new tree node[2];
    }

    public int size;
    public int iterations;
    public float roomVerticalRatio;
    public float roomHorizontalRatio;

    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateDungeon();
        }
    }

    public void GenerateDungeon()
    {
        int randSplit = GetRandomIntInclusive(0, 1);
        if (randSplit == 0)
        {
            print(randSplit + " splitting horizontally");
        }
        else if (randSplit == 1)
        {
            print(randSplit + " splitting vertically");
        }
    }

    private int GetRandomIntInclusive(int a, int b)
    {
        return Random.Range(a, b+1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position, new Vector3(size, size, 0));
    }
}
