using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private CheckpointHandle checkpointHandle;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);   
        }
        else
            Destroy(gameObject);
        
        checkpointHandle = new CheckpointHandle();
    }

    public void SetPlayerPosition(Transform playerTransform)
    {
        checkpointHandle.SetPlayerPosition(playerTransform.position, playerTransform.rotation);
    }

    public void SetKidPosition(Transform kidTransform)
    {
        checkpointHandle.SetKidPosition(kidTransform.position, kidTransform.rotation);
    }

    public bool IsPlayerPositionSet()
    {
        return checkpointHandle.IsPlayerPositionSet;
    }

    public bool IsKidPositionSet()
    {
        return checkpointHandle.IsKidPositionSet;
    }

    public void GetPlayerPosition(Transform playerTransform)
    {
        if (checkpointHandle.IsPlayerPositionSet)
        {
            playerTransform.position = checkpointHandle.PlayerPosition;
            playerTransform.rotation = checkpointHandle.PlayerRotation;
        }
    }

    public void GetKidPosition(Transform kidTransform)
    {
        if (checkpointHandle.IsKidPositionSet)
        {
            kidTransform.position = checkpointHandle.KidPosition;
            kidTransform.rotation = checkpointHandle.KidRotation;
        }
    }
}
