using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    //DialogueManager dialogueManager;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


}
