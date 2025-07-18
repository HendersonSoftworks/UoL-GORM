using UnityEngine;
using Pathfinding;

public class FollowPlayerAndRescan : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private float timer = 0;
    [SerializeField] 
    private float graphSize = 10; // Adjust based on your needs

    private GridGraph gridGraph;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerObject = FindAnyObjectByType<PlayerMovementController>().gameObject;
        //gridGraph = AstarPath.active.data.gridGraph; // Current grid graph reference

        //InvokeRepeating("MoveToPlayerAndRescanPathfiningGraph", 3, timer);
    }

    //private void MoveGrid()
    //{
    //    var gg = AstarPath.active.data.gridGraph;

    //    gg.center = playerObject.transform.position;

    //    gg.Scan();
    //}

    private void MoveToPlayerAndRescanPathfiningGraph()
    {
        MoveToPlayer(playerObject.transform.position);
        RescanPathfiningGraph();
    }

    private void MoveToPlayer(Vector3 newCenter)
    {
        if (gridGraph != null)
        {
            // Set the new center position
            gridGraph.center = newCenter;
        }
    }

    private void RescanPathfiningGraph()
    {
        gridGraph.Scan();
    }
}