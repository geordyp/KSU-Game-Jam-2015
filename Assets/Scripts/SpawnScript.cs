using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {
	
	public GameObject[] obj;
	public float min = 1.0f;
	public float max = 1.0f;

	// Use this for initialization
	void Start () {
		Spawn ();
	}

	void Update () {

	}

	void Spawn()
	{
		Instantiate (obj[0], transform.position, Quaternion.identity);
		Invoke ("Spawn", Random.Range(min, max));
	}

	void HideAllChildren()
	{

	}
}
