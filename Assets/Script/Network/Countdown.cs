using System.Collections;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    private int currentTime = 0;
    protected TextMeshProUGUI text;

    void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        currentTime = OnlineGameManager.instance.countdownTime;
        if (text != null) StartCoroutine(StartCount());
    }

    IEnumerator StartCount()
    {
        while (currentTime > 0)
        {
            text.text = $"{currentTime}...";
            yield return new WaitForSeconds(1f);
            currentTime--;
        }
    }
}
