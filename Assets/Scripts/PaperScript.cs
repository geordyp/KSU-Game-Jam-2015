using UnityEngine;
using System.Collections;

public class PaperScript : MonoBehaviour {
	
	public float targetScale = 0.1f;
	public float shrinkSpeed = 0.1f;
	public Vector3 target;

	// Use this for initialization
	void Start () {
		target = new Vector3(0,-400,0);
	}
	
	void Update() {

		this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(targetScale, targetScale, targetScale), Time.deltaTime*shrinkSpeed);
	}
}
