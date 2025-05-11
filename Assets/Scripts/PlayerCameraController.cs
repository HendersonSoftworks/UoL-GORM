using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
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

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x + xOffset,
            player.transform.position.y + yOffset,
            player.transform.position.z + zOffset);

        transform.rotation = new Quaternion(xRot, transform.rotation.y, transform.rotation.z, transform.rotation.w);

    }
}
