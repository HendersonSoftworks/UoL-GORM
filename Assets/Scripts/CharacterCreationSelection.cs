using UnityEngine;

public class CharacterCreationSelection : MonoBehaviour
{
    public GameObject starterSpell;

    private void Start()
    {
        // Add starter spell
        FindFirstObjectByType<PlayerCharacter>().AddItem(starterSpell.GetComponent<Spell>());
    }
}
