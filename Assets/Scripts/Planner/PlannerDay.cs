using UnityEngine;
using System.Collections;
using System;

public class PlannerDay : GeneralFeed {

	//dates
	public DateTime date;

	//gameobjects
	public GameObject dateDisplayObj;
	public PlannerDetailFeed detailsFeed;

	//visual elements for our names
	public GameObject[] employeePrefabs;

	void Start(){
		//set the displayed date
		string dateString = date.ToString("m");
		string dateStringShort = dateString.Substring(dateString.Length-2);
		//keep the month if the day is the first
		if(dateStringShort.Equals("01")){
			SetLabel(dateDisplayObj, dateString);
		}else{
			SetLabel(dateDisplayObj, dateStringShort);
		}

		//filter by this date
		FilterBy(1, ConvertDateToSting(date));

		grid = gridObject.GetComponent<UIGrid>();

		//hard code the upload URL
		uploadURL = "http://www.samdavies.info/Systech/UpdateTask.php";

		Download();
	}

	protected override void SetFeedReference(GameObject feedObject){
		feedObject.GetComponent<PlannerTask>().theFeed = this;
	}

	private string ConvertDateToSting(DateTime inDate){
		string outDate = date.ToString("u");
		//cut off the time
		outDate = outDate.Substring(0,10);
		return outDate;
	}

	public void LoadDetails(){
		detailsFeed.SetCurrentDay(this);
		detailsFeed.LoadBlocks(displayedBlocks, date);
	}

	public void UploadTask(PlannerTask task){
		uploadQueue.Clear();

		//update task
		WWWForm form = new WWWForm();
		form.AddField("id", task.id);
		form.AddField("Tsk_Name", task.taskName);
		form.AddField("Tsk_Description", task.taskDescription);
		form.AddField("Tsk_Fee", BoolToString(task.fee));
		form.AddField("Tsk_Sam", BoolToString(task.sam));
		form.AddField("Tsk_Chris", BoolToString(task.chris));
		form.AddField("Tsk_Terence", BoolToString(task.terence));

		uploadQueue.AddLast(form);
	}

	private string BoolToString(bool isTrue){
		if(isTrue) { return "1"; }
		else { return "0"; }
	}

	public void CreateTask(){
		WWWForm form = new WWWForm();
		form.AddField("id", "");
		form.AddField("Tsk_Name", "New Task");
		form.AddField("Tsk_Date", date.ToString("u").Substring(0,10));
		form.AddField("Tsk_Description", "");

		string creationURL = "http://www.samdavies.info/Systech/CreateTask.php";
		StartCoroutine(InsertTask(form, creationURL));
	}
	
	protected IEnumerator InsertTask(WWWForm form, string URL){
		print ("inserting...");
		
		//add auth feilds
		form.AddField("userCookie", PlayerPrefs.GetString("userCookie"));
		form.AddField("passCookie", PlayerPrefs.GetString("passCookie"));
		
		WWW upload = new WWW(URL, form);
		yield return upload;
		
		if(upload.error != null){
			print(upload.error);
			StartCoroutine(InsertTask(form, URL));
		}else{
			if(upload.text.Equals("updated")){
				print ("Insert sucessful");
				Download();
			}else{
				print (upload.text);
				//you must login in again
				print ("Authorisation invalid");
			}
		}
	}
}
