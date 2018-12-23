using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIK : MonoBehaviour {

    public GameObject[] Rifles;
    GameObject _currentGun;
    private Animator _animator;

    int _gunIndex = 0;

    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();

        Rifles[_gunIndex].SetActive(true);
        _currentGun = Rifles[_gunIndex];
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.Q))
        {
            _currentGun.SetActive(false);

            _gunIndex++;
            if (_gunIndex >= Rifles.Length)
            {
                _gunIndex = 0;
            }

            Rifles[_gunIndex].SetActive(true);
            _currentGun = Rifles[_gunIndex];
        }		
	}

    private void OnAnimatorIK(int layerIndex)
    {
        var left = _currentGun.transform.Find("Left");

        _animator.SetIKPosition(AvatarIKGoal.LeftHand, left.position);
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);

        _animator.SetIKRotation(AvatarIKGoal.LeftHand, left.rotation);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
    }
}
