using UnityEngine;
using System.Collections;

public class CharacterBlockboxController : MonoBehaviour
{
    // No longer needed - may re-enable when revamping block system
    private IEnumerator DisableHitboxForSeconds(BoxCollider2D boxCollider, float seconds = 1)
    {
        boxCollider.enabled = false;
        yield return new WaitForSeconds(seconds);
        boxCollider.enabled = true;
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.tag == "eHitbox")
    //    {
    //        print("BLOCKED STAY");
    //        BoxCollider2D weaponCollider = collision.GetComponent<BoxCollider2D>();
    //        weaponCollider.enabled = false;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "eHitbox")
        {
            print("BLOCKED");
            BoxCollider2D weaponCollider = collision.GetComponent<BoxCollider2D>();
            //StartCoroutine(DisableHitboxForSeconds(weaponCollider));
            // TODO block sparks effect
            // TODO block audio
        }
    }
}
