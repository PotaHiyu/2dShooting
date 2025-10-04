using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public float moveDistance = 3f;
    public float moveSpeed = 2f;
    public float rotationSpeed = 90f;
    public int totalCycles = 4;

    private enum MovementState
    {
        Moving,
        Rotating
    }

    private MovementState currentState = MovementState.Moving;
    private int currentCycle = 0;
    private Vector3 moveStartPos;
    private Vector3 moveTargetPos;
    private float currentRotation = 0f;
    private float targetRotation = 0f;
    private float moveProgress = 0f;
    private float rotationProgress = 0f;

    void Start()
    {
        SetupNextMove();
    }

    void Update()
    {
        if (currentCycle >= totalCycles)
            return;
        switch (currentState)
        {
            case MovementState.Moving:
                HandleMovement();
                break;
            case MovementState.Rotating:
                HandleRotation();
                break;
        }
    }

    void HandleMovement()
    {
        moveProgress += Time.deltaTime * moveSpeed / moveDistance;

        if (moveProgress >= 1f)
        {
            moveProgress = 1f;
            transform.position = moveTargetPos;
            currentState = MovementState.Rotating;
            targetRotation = currentRotation + 90f;
            rotationProgress = 0f;
        }
        else
        {
            transform.position = Vector3.Lerp(moveStartPos, moveTargetPos, moveProgress);
        }
    }

    void HandleRotation()
    {
        rotationProgress += Time.deltaTime * rotationSpeed / 90f;

        if (rotationProgress >= 1f)
        {
            rotationProgress = 1f;
            currentRotation = targetRotation;
            transform.rotation = Quaternion.Euler(0, 0, currentRotation);

            currentCycle++;

            if (currentCycle < totalCycles)
            {
                currentState = MovementState.Moving;
                SetupNextMove();
            }
        }
        else
        {
            float lerpedRotation = Mathf.Lerp(currentRotation, targetRotation, rotationProgress);
            transform.rotation = Quaternion.Euler(0, 0, lerpedRotation);
        }
    }

    void SetupNextMove()
    {
        moveStartPos = transform.position;
        
        Vector3 rightDirection = GetCurrentRightDirection();
        moveTargetPos = moveStartPos + rightDirection * moveDistance;
        
        moveProgress = 0f;
    }

    Vector3 GetCurrentRightDirection()
    {
        float radians = currentRotation * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);
    }
}