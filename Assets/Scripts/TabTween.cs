using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TabTween : MonoBehaviour {

	public int tabID;
	public GameObject controller;
	private TabController tabController;
	private InputBoxFinder inputBox;

	public PostController postMon;
	public PostController postTue;
	public PostController postWed;
	public PostController postThu;
	public PostController postFri;

	void Start(){
		//find the controller scripts
		tabController = controller.GetComponent<TabController>();
		inputBox = controller.GetComponent<InputBoxFinder>();
		
		//tell all posts about the inputBox
		postMon.inputBox = inputBox;
		postTue.inputBox = inputBox;
		postWed.inputBox = inputBox;
		postThu.inputBox = inputBox;
		postFri.inputBox = inputBox;
	}

	public void SelectThisTab(){
		tabController.SelectTab(tabID);
	}

	public void PlayTab(bool direction){

		//play all the posts
		postMon.PlayPost(direction);
		postTue.PlayPost(direction);
		postWed.PlayPost(direction);
		postThu.PlayPost(direction);
		postFri.PlayPost(direction);

		//activate all the add buttons if forwards
		if(direction){
			postMon.ShowAdd();
			postTue.ShowAdd();
			postWed.ShowAdd();
			postThu.ShowAdd();
			postFri.ShowAdd();
		}

		//deactivate all the add buttons if backwards
		else{
			postMon.HideAdd();
			postTue.HideAdd();
			postWed.HideAdd();
			postThu.HideAdd();
			postFri.HideAdd();
		}
	}

	public void ReposHours(float width, float tabWidth, bool setNow){

		//resize all the post's hours
		postMon.ReposHour(width, tabWidth, setNow);
		postTue.ReposHour(width, tabWidth, setNow);
		postWed.ReposHour(width, tabWidth, setNow);
		postThu.ReposHour(width, tabWidth, setNow);
		postFri.ReposHour(width, tabWidth, setNow);
	}
}
