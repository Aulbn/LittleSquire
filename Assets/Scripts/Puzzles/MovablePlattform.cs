using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MovablePlattform : MonoBehaviour
{
	private Vector3 targetPosition;
	private Transform cachedTransform;
	private bool isMovingUp;
	private PuzzleEventInvoker eventInvoker;

	public Vector3 upPosition;
	public Vector3 downPosition;
	[SerializeField] private float moveSpeed = 2f;
	[SerializeField] private BoxCollider firstCollider, secondCollider;
	[SerializeField] private bool drawGizmos;
	private void Start()
	{
		cachedTransform = transform;
		cachedTransform.position = downPosition;
		targetPosition = downPosition;
		eventInvoker = GetComponent<PuzzleEventInvoker>();
	}

	private void FixedUpdate()
	{
		float distance = (cachedTransform.position - targetPosition).magnitude;

		cachedTransform.position = Vector3.MoveTowards(cachedTransform.position,
			targetPosition,
			Time.deltaTime * moveSpeed * distance);

		if (!(distance <= 0.1f)) return;
		transform.position = targetPosition;
		if (!isMovingUp) return;
		SetCollidersActive(false);
		if (eventInvoker)
			eventInvoker.InvokeActivateEvents();
		isMovingUp = false;
	}
	[ContextMenu("Move Up")]
	public void MoveUp()
	{
		targetPosition = upPosition;
		isMovingUp = true;
	}

	[ContextMenu("Move Down")]
	public void MoveDown()
	{
		targetPosition = downPosition;
		SetCollidersActive(true);
	}

	private void SetCollidersActive(bool active)
	{
		if(firstCollider)
			firstCollider.enabled = active;
		if (secondCollider)
			secondCollider.enabled = active;
	}
	
	private void OnDrawGizmos()
	{
		if (!drawGizmos)
			return;
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(downPosition, 0.5f);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(upPosition, 0.5f);
		Gizmos.color = default;
	}
}
