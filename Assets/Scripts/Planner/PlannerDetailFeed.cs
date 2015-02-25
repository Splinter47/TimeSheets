using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlannerDetailFeed : MonoBehaviour {

	//store the current list of task
	private List<GameObject> displayedTasks = new List<GameObject>();
	//list of refs to tasks
	private List<GameObject> referencedTasks = new List<GameObject>();
	private int numberRefTasks = 0;
	//list of job options
	public List<PlannerJobStore> jobOptions = new List<PlannerJobStore>();
	//filter list
	protected Dictionary<int, string> filterList = new Dictionary<int, string>();
	private string downloadURL = "http://www.samdavies.info/Systech/DownloadJobOptions.php";

	private DateTime date;
	public PlannerDay currentDay;

	//GameObjects
	public GameObject dateObj;
	public UIScrollView scrollView;
	public GameObject gridObj;
	private UIGrid grid;
	public GameObject detailsPrefab;

	void Start(){
		grid = gridObj.GetComponent<UIGrid>();

		//download job option list
		StartCoroutine(DownloadJobOptions(filterList, downloadURL));
	}

	void Update(){

		//if the number of tasks changes, refresh
		int currentRefTasks = referencedTasks.Count;
		if(currentRefTasks != numberRefTasks){
			LoadBlocks(referencedTasks, date);
			numberRefTasks = currentRefTasks;
		}
	}

	public void SetCurrentDay(PlannerDay currentDayRef){
		currentDay = currentDayRef;
	}

	public void CreateTask(){
		currentDay.CreateTask();
	}

	public void LoadBlocks(List<GameObject> referencedTasksRef, DateTime newDate){
		//delete the old blocks
		DestroyTasks();

		//save the new blocks
		date = newDate;
		referencedTasks = referencedTasksRef;
		numberRefTasks = referencedTasks.Count;

		//set the date
		SetLabel(dateObj, FormatDate(date));
		//create the new blocks
		CreateTasks();
	}

	private void CreateTasks(){
		//create real objects
		scrollView.ResetPosition();
		foreach (GameObject refTask in referencedTasks){
			// create a new prefab
			GameObject detailTask = NGUITools.AddChild(gridObj, detailsPrefab);
			displayedTasks.Add(detailTask);
			detailTask.GetComponent<PlannerDetailObj>().theFeed = this;
			detailTask.GetComponent<PlannerDetailObj>().refTask = refTask;
			//set the values
			RefToDisplayed(refTask, detailTask);
			//set the fetched job list
			detailTask.GetComponent<PlannerDetailObj>().SetJobList(jobOptions);
		}
		grid.Reposition();
		scrollView.ResetPosition();
	}

	public void UpdateTasksLocally(){
		//print ("Updating tasks");
		//update thumbnails
		foreach (GameObject disTask in displayedTasks){
			//get the updated details' ref to its thumnails
			GameObject refTask = disTask.GetComponent<PlannerDetailObj>().refTask;
			DisplayedToRef(refTask, disTask);
		}
	}

	public void UpdateTasksWeb(){
		//upload data
		foreach (GameObject refTask in referencedTasks){
			refTask.GetComponent<PlannerTask>().UpdateThisTask();
		}
	}

	private void RefToDisplayed(GameObject refObj, GameObject displayedObj){
		//gets the variables from ref
		PlannerTask refTask = refObj.GetComponent<PlannerTask>();
		PlannerDetailObj disTask = displayedObj.GetComponent<PlannerDetailObj>();

		disTask.id = refTask.id;
		SetLabel(disTask.taskNameObj, refTask.taskName);
		SetLabel(disTask.taskDescriptionObj, refTask.taskDescription);
		disTask.taskColourObj.GetComponent<UISprite>().color = refTask.taskColour;

		//get name tags
		disTask.nameToggle.GetPersonNames();

		//set UIButton default colour
		disTask.taskNameList.GetComponent<UIButton>().defaultColor = refTask.taskColour;
	}

	private void DisplayedToRef(GameObject refObj, GameObject displayedObj){
		//gets the variables from displayed
		PlannerTask refTask = refObj.GetComponent<PlannerTask>();
		PlannerDetailObj disTask = displayedObj.GetComponent<PlannerDetailObj>();

		refTask.taskName = GetLabel(disTask.taskNameObj);
		refTask.taskDescription = GetLabel(disTask.taskDescriptionObj);
		refTask.taskColour = disTask.taskColourObj.GetComponent<UISprite>().color;

		//set name tags
		disTask.nameToggle.SetPersonNames();
		//show the edit in thumbnail
		refTask.SetDisplayedTask();
	}

	private void DestroyTasks(){
		foreach (GameObject disTask in displayedTasks){
			NGUITools.Destroy(disTask);
		}
		//empty the list
		displayedTasks.Clear();
	}

	private string FormatDate(DateTime toDate){
		string dateString = toDate.ToString("D");
		return dateString.Substring(0, dateString.Length-6);
	}

	private string GetLabel(GameObject obj){
		return obj.GetComponent<UILabel>().text;
	}
	
	private void SetLabel(GameObject to, string newText){
		UILabel lbl = to.transform.GetComponent<UILabel>();
		lbl.text = newText;
		lbl.MarkAsChanged();
	}
	
	private IEnumerator DownloadJobOptions(Dictionary<int, string> filters, string dataURL){
		print ("downloading...");
		string URL = dataURL;
		WWWForm form = new WWWForm();
		
		//add all filters
		string methods = "";
		foreach(var filter in filters){
			methods += filter.Key + "," + filter.Value + ";";
		}
		//send all methods in one string
		form.AddField(("methods"), methods);
		
		//add auth feilds
		form.AddField("userCookie", PlayerPrefs.GetString("userCookie"));
		form.AddField("passCookie", PlayerPrefs.GetString("passCookie"));
		
		WWW download = new WWW(URL, form);
		yield return download;
		
		if(download.error != null){
			print(download.error);
			StartCoroutine(DownloadJobOptions(filters, dataURL));
		}else{
			if(!download.text.Equals("failed")){
				print ("found data: " + download.text);
				SetJobOptions(download.text);
			}else{
				//you must login in again
				print ("Authorisation invalid");
			}
		}
	}

	private void SetJobOptions(string input){
		//divide into its data components
		string[] attStringSeparators = {"<!!>"};
		string[] data = input.Split(attStringSeparators, System.StringSplitOptions.None);

		//save them in the list
		foreach(string job in data){
			//remove the last empty thing
			if(job.Length>1){
				PlannerJobStore jobStore = new PlannerJobStore();
				jobStore.AddVariables(job);
				jobOptions.Add(jobStore);
				print (jobStore.name);
			}
		}

		//update active objects
		//set the fetched job list
		foreach (GameObject disTask in displayedTasks){
			disTask.GetComponent<PlannerDetailObj>().SetJobList(jobOptions);
		}
	}

	protected void FilterBy(int methodNum, string search){
		//remove old filter
		filterList.Remove(methodNum);
		//check search is not empty
		if(!search.Equals("")){
			//add the filter
			filterList.Add(methodNum, search);
		}
		//download with filter applied
		StartCoroutine(DownloadJobOptions(filterList, downloadURL));
	}
}
