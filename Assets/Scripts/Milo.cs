using System.Collections;
using UnityEngine;

public class Milo : CharacterBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override IEnumerator PerformUniqueAction()
    {
        int randomEmotion = Random.Range(1,4);
        switch (randomEmotion)
        {
            case 1:
                animator.SetTrigger("Love");
            break;

            case 2:
                animator.SetTrigger("Joy");
                break;

            case 3:
                animator.SetTrigger("Sad");
                break;

            case 4:
                animator.SetTrigger("Angry");
                break;
        }

        yield return new WaitForSeconds(1);
    }
}
