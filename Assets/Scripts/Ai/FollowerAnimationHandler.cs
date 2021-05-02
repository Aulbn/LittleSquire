using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerAnimationHandler : MonoBehaviour
{
    public float lookAtSpeed = 1f; //Might have to split this into two

    private Animator anim; 

    private enum POIType{
        None, Static, Moving
    }

    public bool IsPulling { get { return anim.GetCurrentAnimatorStateInfo(0).IsName("Pull Lever"); } }
    public bool IsRotating { get { return anim.GetCurrentAnimatorStateInfo(0).IsName("Turn Blend"); } }

    private POIType poiType;
    private Transform poiObject;
    private Vector3 poi;
    private float lookAtWeight;
    private bool isInterpolating;

    private float currentSpeed;
    private bool hasStepped = false;
    private float footCurve;
    private AudioManager footstepAudio;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        footstepAudio = GetComponentInParent<AudioManager>();
    }

    private void Update()
    {
        FootStepUpdate();

        if (poiType == POIType.Moving && !isInterpolating)
            poi = poiObject.position;
    }

    private void FootStepUpdate()
    {
        if (currentSpeed < 0.1f) return;

        footCurve = anim.GetFloat("FootCurve");
        if (hasStepped && footCurve < 0)
        {
            hasStepped = false;
        }
        else if (!hasStepped && footCurve > 0)
        {
            hasStepped = true;
            footstepAudio.Play();
        }
    }


    public void SetVelocity(float velocity)
    {
        currentSpeed = velocity;
        anim.SetFloat("Velocity", currentSpeed, .2f, Time.deltaTime);
    }

    public void SetIsCrawling(bool isCrawling)
    {
        anim.SetBool("IsCrawling", isCrawling);
    }

    public void PullLever()
    {
        anim.SetTrigger("PullLever");
    }



    public void LookAt(Transform target)
    {
        poiObject = target;
        StartLookAt(poiObject.position);
        poiType = POIType.Moving;
    }

    public void LookAt(Vector3 pointOfInterest)
    {
        StartLookAt(pointOfInterest);
        poiType = POIType.Static;
    }

    private void StartLookAt(Vector3 pointOfInterest)
    {
        StopAllCoroutines();
        if (poiType == POIType.None)
        {
            poi = pointOfInterest;
            StartCoroutine(SetLookAt(1));
        }
        else
        {
            StartCoroutine(SwitchLookAt(1, poi, pointOfInterest));
        }
    }

    public void TurnTowards(Vector3 pointOfInterest)
    {
        anim.SetFloat("TurnAngle", GetTurnAngle(pointOfInterest));
        anim.SetTrigger("Turn");
    }

    public void StopLookAt()
    {
        poiType = POIType.None;
        StopAllCoroutines();
        StartCoroutine(SetLookAt(0));
    }

    public float GetTurnAngle(Vector3 poi)
    {
        Transform parent = transform.parent;
        Vector3 turnToDir = new Vector3(poi.x, parent.position.y, poi.z) - parent.position;
        float angle = Vector3.Angle(parent.forward, turnToDir);
        if (Vector3.Cross(parent.forward, turnToDir).y < 0) angle = -angle;
        return angle;
    }

    private IEnumerator SetLookAt(float weight)
    {
        while (lookAtWeight != weight)
        {
            yield return new WaitForEndOfFrame();
            lookAtWeight = Mathf.Clamp01(lookAtWeight + Time.deltaTime * (lookAtWeight < weight ? 1 : -1) * lookAtSpeed);
        }
    }

    private IEnumerator SwitchLookAt(float weight, Vector3 oldPoi, Vector3 newPoi)
    {
        float startWeight = lookAtWeight;
        isInterpolating = true;

        float timer = 0;
        float timerThreashold = 0.98f;

        do
        {
            //Calculate transition
            yield return new WaitForEndOfFrame();
            if (timer < timerThreashold)
                timer += Time.deltaTime * lookAtSpeed;
            else
                timer = 1;

            //Move position
            if (poiType == POIType.Static)
            {
                poi = Vector3.Lerp(oldPoi, newPoi, timer);
            }
            else if (poiType == POIType.Moving)
            {
                poi = Vector3.Lerp(oldPoi, poiObject.position, timer);
            }

            //Set weight
            lookAtWeight = Mathf.Lerp(startWeight, weight, timer);

        } while (timer < timerThreashold);

        isInterpolating = false;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (lookAtWeight > 0)
        {
            anim.SetLookAtPosition(poi);
            anim.SetLookAtWeight(lookAtWeight, .05f, 1);
        }

    }
}
