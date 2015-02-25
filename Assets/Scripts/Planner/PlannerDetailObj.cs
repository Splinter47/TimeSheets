using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlannerDetailObj : Data_General {

	public PlannerDetailFeed theFeed;
	public GameObject refTask;
	public PlannerNameToggle nameToggle;

	//prefab labels
	public int id;
	public GameObject taskNameObj;
	public GameObject taskDescriptionObj;
	public GameObject taskColourObj;

	//Job List
	public GameObject taskNameList;

	public override void Create(){
		//TO DO
	}

	public void UpdateTasksThoughFeed(){
		theFeed.UpdateTasksLocally();
		theFeed.UpdateTasksWeb();
	}

	public void SetNameLabel(){
		UIPopupList popUpList = taskNameList.GetComponent<UIPopupList>();
		SetLabel(taskNameObj, popUpList.value);
		//set colour aswell
		foreach(PlannerJobStore job in theFeed.jobOptions){
			//find the matching job name
			string detailTaskName = GetLabel(taskNameObj);
			if(job.name.Equals(detailTaskName)){
				//set sprite and default list colours
				taskColourObj.GetComponent<UISprite>().color = job.colour;
				taskNameList.GetComponent<UIButton>().defaultColor = job.colour;
			}
		}
	}

	public void SetJobList(List<PlannerJobStore> jobs){
		UIPopupList list = taskNameList.GetComponent<UIPopupList>();

		//create list
		List<string> jobNames = new List<string>();
		foreach(PlannerJobStore job in jobs){
			jobNames.Add(job.name);
		}

		//change pop up list
		list.items = jobNames;
		//set the default value
		list.value = GetLabel(taskNameObj);
	}
}
