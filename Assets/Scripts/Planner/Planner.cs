using UnityEngine;
using System.Collections;
using System;

public class Planner : MonoBehaviour {

	private DateTime startDate;
	public GameObject grid;
	public GameObject weekPrefab;
	public PlannerDetailFeed detailsFeed;

	// Use this for initialization
	void Start () {
		//hard code the start date
		startDate = DateTime.Parse("2014-06-23");

		for(int i = 0; i<12; i++){
			//create a new week object
			GameObject week = NGUITools.AddChild(grid, weekPrefab);
			//set the week beginning from startDate
			PlannerWeek plannerWeek = week.GetComponent<PlannerWeek>();
			plannerWeek.weekBegininng = startDate.AddDays(i*7);
			//set link to details window
			plannerWeek.detailsFeed = detailsFeed;
		}

		//reposition the weeks
		grid.GetComponent<UIGrid>().Reposition();
	}
}
