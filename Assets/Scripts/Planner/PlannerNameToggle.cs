using UnityEngine;
using System.Collections;

public class PlannerNameToggle : MonoBehaviour {

	public PlannerDetailObj detailTask;

	public GameObject feeObj;
	public GameObject samObj;
	public GameObject chrisObj;
	public GameObject terenceObj;

	//stored toggles
	public bool fee = false;
	public bool sam = false;
	public bool chris = false;
	public bool terence = false;

	void Start(){

	}

	public void GetPersonNames(){
		PlannerTask refTask = detailTask.refTask.GetComponent<PlannerTask>();
		fee = refTask.fee;
		sam = refTask.sam;
		chris = refTask.chris;
		terence = refTask.terence;

		SetPerson("fee", feeObj, fee);
		SetPerson("sam", samObj, sam);
		SetPerson("chris", chrisObj, chris);
		SetPerson("terence", terenceObj, terence);
	}

	public void SetPersonNames(){
		PlannerTask refTask = detailTask.refTask.GetComponent<PlannerTask>();
		refTask.fee = fee;
		refTask.sam = sam;
		refTask.chris = chris;
		refTask.terence = terence;
	}

	private void SetPerson(string person, GameObject personObj, bool toBool){
		if(person.Equals("fee")){ fee = toBool; }
		else if(person.Equals("sam")){ sam = toBool; }
		else if(person.Equals("chris")){ chris = toBool; }
		else if(person.Equals("terence")){ terence = toBool; }
		PlayTween(personObj, toBool);
	}

	public void ToggleFee(){
		if(fee == true){
			SetPerson("fee", feeObj, false);
		}else{
			SetPerson("fee", feeObj, true);
		}
		SetPersonNames();
	}

	public void ToggleSam(){
		if(sam == true){
			SetPerson("sam", samObj, false);
		}else{
			SetPerson("sam", samObj, true);
		}
		SetPersonNames();
	}

	public void ToggleChris(){
		if(chris == true){
			SetPerson("chris", chrisObj, false);
		}else{
			SetPerson("chris", chrisObj, true);
		}
		SetPersonNames();
	}

	public void ToggleTerence(){
		if(terence == true){
			SetPerson("terence", terenceObj, false);
		}else{
			SetPerson("terence", terenceObj, true);
		}
		SetPersonNames();
	}

	private void PlayTween(GameObject obj, bool direction){
		obj.GetComponent<UITweener>().Play(direction);
	}
}
