using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        //GameObject p = GameObject.Find("Player");
        //if (p == null)
        //{
        //    Debug.LogError("Enemy.Start() " + this.name + " player could not be found!");
        //    Debug.Break();
        //    return;
        //}
        //Debug.Log(this.name + " Start()!");

        navMeshAgent = this.GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("Enemy.Start() " + this.name + " component 'NavMeshAgent' isn't found!");
            Debug.Break();
            return;
        }
    }

    private void OnSpawned()
    {
        if (target == null)
            target = Player.Instance;
        //Debug.Log(this.name + " is spawned!");
    }

    private void FixedUpdate()
    {
        navMeshAgent.destination = target.position;
    }

}
