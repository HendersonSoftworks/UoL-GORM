using UnityEngine;

public class Chest : MonoBehaviour
{
    public Item chestItem;

    [SerializeField]
    private GameObject dialogueIcon;

    public void SetDialogue(Collider2D collision, bool value)
    {
        dialogueIcon.SetActive(value);
        collision.GetComponent<PlayerCharacter>().inContactWithChest = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SetDialogue(collision, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SetDialogue(collision, false);
        }
    }
}
