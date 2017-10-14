using UnityEngine;
using System.Collections;

public class RuleWindow : MonoBehaviour {

	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickOk()
	{
		gameObject.SetActive (false);
	}

	public void Open()
	{
		gameObject.SetActive (true);
	}
}
