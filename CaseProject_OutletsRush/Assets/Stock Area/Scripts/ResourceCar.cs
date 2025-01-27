using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResourceCar : MonoBehaviour
{
    [SerializeField] NavMeshAgent navmeshAgent;
    public Stand stand;
    public Transform standPos;
    public Transform carGoPos;
    bool arrived = false;

    public Animator vehicleAnim;
    private void Start()
    {
        navmeshAgent.SetDestination(standPos.position);
    }
    private void Update()
    {
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(standPos.position.x, standPos.position.z)) < 40f && arrived == false)
        {
            arrived = true;
            navmeshAgent.enabled = false;
            StartCoroutine(SetRot());
        }
    }

    IEnumerator SetRot()
    {
        navmeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        Vector3 firstPos = transform.position;
        Vector3 targetPos = new Vector3(standPos.position.x, transform.position.y, standPos.position.z);
        Quaternion firstRot = transform.rotation;
        float counter = 0f;
        while (counter < 3f)
        {
            counter += Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, 10 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, standPos.rotation, 5 * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = standPos.rotation;
        navmeshAgent.enabled = true;
        navmeshAgent.SetDestination(transform.position);
        stand.StandReActive();

        if(vehicleAnim != null)
        {
            vehicleAnim.SetBool("openactive", true);
        }
        if (stand.GetComponent<ResourceCreator>() != null)
        {
            stand.GetComponent<ResourceCreator>().CarArrivedStockArea();
        }

        vehicleAnim.SetBool("openactive", true);
    }
    public void CarGoOut()
    {
        StartCoroutine(CarGoOurDelay());
        vehicleAnim.SetBool("openactive", false);
    }

    IEnumerator CarGoOurDelay()
    {
        if (vehicleAnim != null)
        {
            vehicleAnim.SetBool("openactive", false);
        }
        yield return new WaitForSeconds(1f);

        navmeshAgent.enabled = true;
        navmeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        navmeshAgent.SetDestination(carGoPos.position);
        Destroy(gameObject, 4f);
    }
}
