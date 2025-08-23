using UnityEngine;

public class Chest : MonoBehaviour
{
    public Item chestItem;

    [SerializeField]
    private GameObject dialogueIcon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            dialogueIcon.SetActive(true);
            collision.GetComponent<PlayerCharacter>().inContactWithChest = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            dialogueIcon.SetActive(false);
            collision.GetComponent<PlayerCharacter>().inContactWithChest = false;
        }
    }
}
