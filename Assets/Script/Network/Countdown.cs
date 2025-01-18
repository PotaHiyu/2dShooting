using System.Collections;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public int countdownTime = 3;
    private int currentTime = 0;
    protected TextMeshProUGUI text;

    void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        currentTime = countdownTime;
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
        OnlineGameManager.instance.StartPlaying();
    }
}
