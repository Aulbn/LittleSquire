using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandle
{
    private Vector3 playerPosition;
    private Vector3 kidPosition;
    private Quaternion playerRotation;
    private Quaternion kidRotation;
    private bool isPlayerPositionSet;
    private bool isKidPositionSet;

    public Vector3 PlayerPosition => playerPosition;
    public Vector3 KidPosition => kidPosition;
    public Quaternion PlayerRotation => playerRotation;
    public Quaternion KidRotation => kidRotation;
    public bool IsPlayerPositionSet => isPlayerPositionSet;
    public bool IsKidPositionSet => isKidPositionSet;

    public void SetPlayerPosition(Vector3 playerCurrentPosition, Quaternion playerCurrentRotation)
    {
        if (playerCurrentPosition == Vector3.zero) return;
        playerPosition = playerCurrentPosition;
        playerRotation = playerCurrentRotation;
        isPlayerPositionSet = true;
    }

    public void SetKidPosition(Vector3 kidCurrentPosition, Quaternion kidCurrentRotation)
    {
        if (kidCurrentPosition == Vector3.zero) return;
        kidPosition = kidCurrentPosition;
        kidRotation = kidCurrentRotation;
        isKidPositionSet = true;
    }
}
