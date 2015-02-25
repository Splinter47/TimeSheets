using UnityEngine;
using System.Collections;
using System;

public class PlannerWeek : MonoBehaviour {

	public DateTime weekBegininng;
	public PlannerDetailFeed detailsFeed;

	//parent panel
	public UIPanel parentPanel;

	public GameObject monday;
	public GameObject tuesday;
	public GameObject wednesday;
	public GameObject thurday;
	public GameObject friday;
	public GameObject saturday;
	public GameObject sunday;

	void Start(){
		//enable panel after creation to avoid clipping issue
		parentPanel = gameObject.GetComponent<UIPanel>();
		parentPanel.enabled = true;

		//set the date for each day
		SetDay(monday, 0);
		SetDay(tuesday, 1);
		SetDay(wednesday, 2);
		SetDay(thurday, 3);
		SetDay(friday, 4);
		SetDay(saturday, 5);
		SetDay(sunday, 6);
	}

	private void SetDay(GameObject dayObj, int dayNumber){
		PlannerDay plannerDay = dayObj.GetComponent<PlannerDay>();
		plannerDay.date = weekBegininng.AddDays(dayNumber);
		//set link to details window
		plannerDay.detailsFeed = detailsFeed;
	}
}
