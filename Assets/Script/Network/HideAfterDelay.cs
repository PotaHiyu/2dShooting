using System.Collections;
using UnityEngine;

public class HideAfterDelay : MonoBehaviour
{
    public float delay = 1f;

    void OnEnable()
    {
        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
