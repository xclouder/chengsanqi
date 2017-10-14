using UnityEngine;
using System.Collections;

public class MainWindow : MonoBehaviour {

	public RuleWindow ruleWindow;

	public void Open()
	{
		gameObject.SetActive (true);
	}

	public void ClickShowRole()
	{
		ruleWindow.Open ();
	}

}
