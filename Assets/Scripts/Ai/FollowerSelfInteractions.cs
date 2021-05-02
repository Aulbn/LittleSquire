using UnityEngine;

public class FollowerSelfInteractions : MonoBehaviour
{
	//private Follower follower;
	[SerializeField] private GameObject interactionGameObject;
	private bool activated;
	
	private void Start()
	{
		//follower = FindObjectOfType<Follower>();
	}

	public void GetFollowerToInteractWithGameobject()
	{
		if (interactionGameObject && !activated)
		{
			Transform leverTransform = interactionGameObject.transform;
			//follower.MoveToObject(leverTransform.position + leverTransform.forward, interactionGameObject);
			activated = true;
		}
	}
}
