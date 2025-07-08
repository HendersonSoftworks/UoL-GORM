using UnityEngine;

public class WallData : MonoBehaviour
{
    public BoundsInt bounds;
    public GameObject dungeonAgent;

    private DungeonGenerator dungeonGenerator;


    void Start()
    {
        //dungeonGenerator = FindFirstObjectByType<DungeonGenerator>();
        //dungeonGenerator.walls.Add(gameObject);

        //BoundsInt newBoundsInt = new BoundsInt((int)transform.position.x, (int)transform.position.y, 0,
        //    (int)transform.localScale.x, (int)transform.localScale.y, 0);

        //bounds = newBoundsInt;

        //foreach (var item in dungeonGenerator.walls)
        //{
        //    if (item == null)
        //    {
        //        return;
        //    }

        //    var isOverlapping = dungeonGenerator.BoundsIntOverlap(bounds, item.GetComponent<WallData>().bounds);
        //    if (isOverlapping)
        //    {
        //        print("destroy wall");
        //        //dungeonGenerator.walls.Remove(gameObject);
        //        Destroy(gameObject);
        //    }
        //}
    }
}
