using UnityEngine;

public class CharacterCreationSelection : MonoBehaviour
{
    public GameObject starterSpell;

    private void Start()
    {
        // Add starter spell
        //FindFirstObjectByType<UIManager>().chestItemImage.sprite = starterSpell.GetComponent<Spell>().sprite;
        //FindFirstObjectByType<PlayerCharacter>().AddItem(starterSpell.GetComponent<Spell>());
    }
}
