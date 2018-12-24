using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour {

    public Transform Target;

    NavMeshAgent _agent;
    Animator _animator;

	// Use this for initialization
	void Start () {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        _agent.destination = Target.position;

        var v = _agent.desiredVelocity;

        var localV = transform.InverseTransformVector(v);

        _animator.SetFloat("speedX", localV.x);
        _animator.SetFloat("speedZ", localV.z);
    }
}
