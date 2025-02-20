using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkAfterDamage : MonoBehaviour {

    public int numberOfBlinks;
    public float timeBetweenBlinks;

    public void Blink()
    {
        StopAllCoroutines();
        StartCoroutine(BlinkAfterDamageCorroutine());
    }

    IEnumerator BlinkAfterDamageCorroutine()
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().color = Color.red;

            for (int i = 0; i < numberOfBlinks; i++)
            {
                GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
                yield return new WaitForSeconds(timeBetweenBlinks);
            }

            GetComponent<SpriteRenderer>().color = Color.white;
            GetComponent<SpriteRenderer>().enabled = true;
        }

        yield return null;
    }
}
