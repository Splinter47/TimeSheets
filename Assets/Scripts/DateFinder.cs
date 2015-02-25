using UnityEngine;
using System.Collections;

public class DateFinder : MonoBehaviour {

	public int currentDate = 5;
	public GameObject labelObj;

	public void AddWeek () {
		currentDate += 7;
		SetLabel(labelObj, ("WC - " + currentDate + "th June 14"));
	}

	public void MinusWeek () {
		currentDate -= 7;
		SetLabel(labelObj, ("WC - " + currentDate + "th June 14"));
	}

	private void SetLabel(GameObject obj, string text){
		UILabel lbl = obj.GetComponent<UILabel>();
		lbl.text = text;
		lbl.MarkAsChanged();
	}
}
