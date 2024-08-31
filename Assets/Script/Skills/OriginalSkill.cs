using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalSkill : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 finishPos;
    private float speed;
    public GameObject skill;
    public bool skillStartPosUpdate;

    void Update()
    {
        if (skillStartPosUpdate)
        {
            startPos = skill.transform.position;
            if (Input.GetKeyDown(KeyCode.V))
            {
                skill.transform.position = Vector2.Lerp(startPos, finishPos, speed);
            }
        }
    }
}
