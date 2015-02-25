using UnityEngine;
using System.Collections;

public class LoadLevels : MonoBehaviour {

	public void LoadPlanner(){
		Application.LoadLevel("Planner");
	}

	public void LoadPeople(){
		Application.LoadLevel("People");
	}
}
