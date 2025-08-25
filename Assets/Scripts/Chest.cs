using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Chest : MonoBehaviour
{
    public Item chestItem;
    public bool empty = false;

    [SerializeField]
    private GameObject dialogueIcon;

    public void SetDialogue(Collider2D collision, bool value)
    {
        dialogueIcon.SetActive(value);
        collision.GetComponent<PlayerCharacter>().inContactWithChest = value;
    }

    public void SetDialogue(bool value)
    {
        dialogueIcon.SetActive(value);
    }

    public void MarkChestAsEmpty()
    {
        empty = true;
        GetComponent<Light2D>().enabled = false;
        SetDialogue(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" &&
            empty != true)
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
