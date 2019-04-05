using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWeapons : MonoBehaviour
{
    public GameObject[] Weapons;
    public Transform SpawnPoints;
    public int WeaponCount = 10;

	// Use this for initialization
	void Start () {
	    for (int i = 0; i < WeaponCount; i++)
	    {
	        int spawnIndex = Random.Range(0, SpawnPoints.childCount);
	        var spawnPoint = SpawnPoints.GetChild(spawnIndex);

	        int index = Random.Range(0, Weapons.Length);
	        var prefab = Weapons[index];

	        var go = Instantiate(prefab);
	        go.transform.position = spawnPoint.position;
	    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
