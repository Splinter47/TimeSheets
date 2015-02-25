using UnityEngine;
using System.Collections;

public class Data_People : Data_General {
	
	public PeopleFeed theFeed; 

	//prefab labels
	public int id;
	public GameObject nameObj;
	public string salutation;
	public string firstName;
	public string surname;
	public GameObject jobObj;
	public GameObject companyObj;
	public GameObject emailObj;
	public GameObject mobileNumObj;
	public GameObject homeNumObj;
	
	public override void Create(){
		//find the feed
		//theFeed = GameObject.Find("Controller").GetComponent<PeopleFeed>();

		//set labels
		id = int.Parse(data[0]);
		SetLabel(nameObj, data[1] + " " + data[2] + " " + data[3]);
		salutation = data[1];
		firstName = data[2];
		surname = data[3];
		SetLabel(jobObj, data[4]);
		SetLabel(companyObj, data[5]);
		SetLabel(emailObj, data[6]);
		SetLabel(mobileNumObj, data[7]);
		SetLabel(homeNumObj, data[8]);
	}

	public void OpenDetails(){
		theFeed.ShowDetails(this);
	}
}
