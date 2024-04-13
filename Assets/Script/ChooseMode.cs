using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseMode : MonoBehaviour
{
    public static bool mode = false; // easy=false;hard=true;
    public void OnClickEasyBotton()
    {
        mode = false;
        SceneManager.LoadScene("1-1");
    }
    public void OnClickHardBotton()
    {
        mode = true;
        SceneManager.LoadScene("1-1");
    }
}
