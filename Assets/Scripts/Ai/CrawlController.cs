using AI;
using UnityEngine;

public class CrawlController : MonoBehaviour/*, IInteract*/
{
    [Tooltip("Set the distance how long the crawl should be")]
    [SerializeField] private float crawlDistance = 1f;

    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Vector3 endPosition;

    private void Awake()
    {
        startPosition = transform.position - (transform.forward);
        endPosition = startPosition + transform.forward + (transform.forward * crawlDistance);
    }

    public void Crawl()
    {
        AISystem AISystem = FindObjectOfType<AISystem>();
        AISystem.OnCrawl(this);
        //CompanionController ai = FindObjectOfType<CompanionController>();
        //ai.SetIsBusy(true);
        //ai.WalkTo(startPosition, () => //Bahabahbahb new stuff pls push plssss
        //{
        //    ai.animationHandler.TurnTowards(transform.position, () => ai.StartCrawl(this));
        //});
    }

    //public void Interact(CharacterInteraction interactor)
    //{
    //    Crawl();
    //}

    private void OnDrawGizmos()
    {
        Vector3 startPos = transform.position - (transform.forward);
        Vector3 endPos = startPos + transform.forward + (transform.forward * crawlDistance);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(startPos, endPos);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPos, .5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(endPos, .5f);
    }
}
