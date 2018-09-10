using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    private NavMeshAgent _agent;

	void Start ()
	{
	    _agent = GetComponent<NavMeshAgent>();
        _agent.destination = Vector3.one;
	}
}
