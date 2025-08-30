using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float xOffset;
    [SerializeField]
    private float yOffset;
    [SerializeField]
    private float zOffset;
    [SerializeField]
    private float xRot;

    public static PlayerCameraController playerCameraInstance { get; private set; }

    private void Awake()
    {
        if (playerCameraInstance != null && playerCameraInstance != this)
        {
            Destroy(this);
        }
        else
        {
            playerCameraInstance = this;
        }
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerMovementController>().gameObject;
        canvas = FindFirstObjectByType<Canvas>();
        canvas.worldCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (player == null) { return; }

        transform.position = new Vector3(player.transform.position.x + xOffset,
            player.transform.position.y + yOffset,
            player.transform.position.z + zOffset);
    }
}
