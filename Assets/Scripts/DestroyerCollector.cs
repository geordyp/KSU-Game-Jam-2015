using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class DestroyerCollector : MonoBehaviour {

	public GameObject scoreTextBox;
	public int score;

	void Start()
	{
		scoreTextBox = GameObject.Find ("ScoreTextBox");

		score = 0;
	}

	void Update()
	{
		scoreTextBox.GetComponent<Text> ().text = score.ToString () + " MB of data saved";

		/*
		if (Input.GetKey (KeyCode.Space) && _desktopIsVisible) {

		}
		else {

		}
		*/
	}

	void OnTriggerEnter2D(Collider2D other)
	{	
		if (other.gameObject.transform.parent)
		{
			Destroy (other.gameObject.transform.parent.gameObject);

			score += 1;
		}
		else
		{
			Destroy (other.gameObject);

			score += 1;
		}
	}
}
