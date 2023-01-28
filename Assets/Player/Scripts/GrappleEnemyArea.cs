using System.Collections.Generic;
using UnityEngine;

public class GrappleEnemyArea : MonoBehaviour
{
    [SerializeField] private RaycastHit2D[] possiblePoints;
    [SerializeField] private List<GameObject> usedPoints;
    [SerializeField] private GameObject targetPoint;
    public GameObject closestGrapplePoint;
    [SerializeField] private GameObject pl;

    [SerializeField] private float dis = 1f;
    // Start is called before the first frame update
    void Start()
    {
        pl = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        searchForEnemies();
        farthestPoint();
        if (closestGrapplePoint != null)
        {
            targetPoint.SetActive(true);
            if (targetPoint.transform.position != closestGrapplePoint.transform.position)
            {
                targetPoint.transform.position = closestGrapplePoint.transform.position;
            }
        }
        else
        {
            targetPoint.SetActive(false);
        }
        
    }

    private void searchForEnemies()
    {
        possiblePoints = Physics2D.CircleCastAll(transform.position, dis, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Enemy"));
    }

    private void farthestPoint()
    {
        if(possiblePoints.Length > 0) 
        {
            closestGrapplePoint = possiblePoints[0].collider.gameObject;
            foreach (var point in possiblePoints)
            {
                if (point.collider.gameObject.activeSelf)
                {
                    if (Vector3.Distance(pl.transform.position, point.transform.position) >= Vector3.Distance(pl.transform.position, closestGrapplePoint.transform.position))
                    {
                        closestGrapplePoint = point.collider.gameObject;
                    }
                }
            }
        }
        else
        {
            closestGrapplePoint = null;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, dis);
    }
}
