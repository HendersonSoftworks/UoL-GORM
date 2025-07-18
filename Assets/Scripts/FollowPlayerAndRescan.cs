using UnityEngine;
using Pathfinding;

public class FollowPlayerAndRescan : MonoBehaviour
{
    [SerializeField]
    private Vector2 graphTopLeft;
    [SerializeField]
    private Vector2 graphBottomRight;

    private DungeonGenerator dungeonGenerator;
    private GridGraph gridGraph;

    public void SetGraphBoundsAndRescan()
    {
        dungeonGenerator = FindAnyObjectByType<DungeonGenerator>();
        gridGraph = AstarPath.active.data.gridGraph; // Current grid graph reference   
        SetGraphBounds();
        ReScan(gridGraph);
    }

    private void SetGraphBounds()
    {
        var listOfCors = dungeonGenerator.corridors;

        // Set the bounds to the top-left and
        // bottom-right most corridors

        if (listOfCors == null || listOfCors.Count == 0)
        {
            Debug.LogWarning("No corridors found in dungeon generator");
            return;
        }

        // Initialize with first corridor's position
        float minX = listOfCors[0].transform.position.x;
        float maxX = listOfCors[0].transform.position.x;
        float minY = listOfCors[0].transform.position.y;
        float maxY = listOfCors[0].transform.position.y;

        // Find the top-most coordinates from all corridors
        foreach (var corridor in listOfCors)
        {
            Vector2 pos = corridor.transform.position;
            minX = Mathf.Min(minX, pos.x);
            maxX = Mathf.Max(maxX, pos.x);
            minY = Mathf.Min(minY, pos.y);
            maxY = Mathf.Max(maxY, pos.y);
        }

        // Add some padding to the bounds
        float padding = 5f;
        graphTopLeft = new Vector2(minX - padding, maxY + padding);
        graphBottomRight = new Vector2(maxX + padding, minY - padding);

        // Update the grid graph dimensions
        Vector3 center = new Vector3((graphTopLeft.x + graphBottomRight.x) * 0.5f,
            (graphTopLeft.y + graphBottomRight.y) * 0.5f, 0);

        Vector3 size = new Vector3(
            Mathf.Abs(graphBottomRight.x - graphTopLeft.x),
            Mathf.Abs(graphTopLeft.y - graphBottomRight.y),
            0);

        gridGraph.center = center;
        gridGraph.SetDimensions(
            Mathf.RoundToInt(size.x / gridGraph.nodeSize),
            Mathf.RoundToInt(size.y / gridGraph.nodeSize),
            gridGraph.nodeSize);
    }

    private void ReScan(GridGraph _gridGraph)
    {
        _gridGraph.Scan();
    }
}