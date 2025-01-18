using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseMode : MonoBehaviour
{
    public static bool mode = false; // easy=false;hard=true;
    public void OnClickEasyButton()
    {
        mode = false;
        SceneManager.LoadScene("1-1");
    }
    public void OnClickHardButton()
    {
        mode = true;
        SceneManager.LoadScene("1-1");
    }
    public void OnClickTitleButton()
    {
        SceneManager.LoadScene("Title");
    }
    public void OnClick1vs1Button()
    {
        SceneManager.LoadScene("Online");
    }
}
