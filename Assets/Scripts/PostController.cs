using UnityEngine;
using System.Collections;

public class PostController : MonoBehaviour {

	public GameObject hoursObj;
	public GameObject barObject;
	public GameObject textObj;
	public GameObject functionObj;
	public GameObject hiddenButtonObj;

	public GameObject addObj;

	public InputBoxFinder inputBox;

	void Start(){
		Deactivate();
	}

	public void LoadInput(){
		print ("loading");
		//load this post into input
		inputBox.GetPost(this);

		//play the input box
		inputBox.PlayTween(true);
	}

	public void Activate(){

		//enable all objects
		hoursObj.SetActive(true);
		barObject.SetActive(true);
		textObj.SetActive(true);
		hiddenButtonObj.SetActive(true);

		//play forward
		PlayPost(true);

		//hide add button
		addObj.SetActive(false);
	}

	public void Deactivate(){

		//clear all object data

		//disable all objects
		hoursObj.SetActive(false);
		barObject.SetActive(false);
		textObj.SetActive(false);
		hiddenButtonObj.SetActive(false);
		
		//show add button
		addObj.SetActive(true);
	}

	public void HideAdd(){
		addObj.GetComponent<BoxCollider>().enabled = false;
	}

	public void ShowAdd(){
		addObj.GetComponent<BoxCollider>().enabled = true;
	}

	public void PlayPost(bool direction){

		//play forward
		if(direction){
			PlayHours(direction);
			PlayColour(direction, barObject);
			PlayColour(direction, textObj);

			//activate the edit button
			hiddenButtonObj.SetActive(true);
		}
	
		//play backward
		else{
			PlayHours(direction);
			PlayColour(direction, barObject);
			PlayColour(direction, textObj);

			//deactivate the edit button
			hiddenButtonObj.SetActive(false);
		}
	}

	private void PlayHours(bool direction){
		//play tween pos
		TweenPosition twPos = hoursObj.GetComponent<TweenPosition>();
		twPos.Play(direction);
		
		//play tween colour
		TweenColor twCol = hoursObj.GetComponent<TweenColor>();
		twCol.Play(direction);
		
		//play tween scale
		TweenScale twSca = hoursObj.GetComponent<TweenScale>();
		twSca.Play(direction);
	}
	
	private void PlayColour(bool direction, GameObject toPlay){
		//play tween colour
		TweenColor twCol = toPlay.GetComponent<TweenColor>();
		twCol.Play(direction);
	}

	public void ReposHour(float width, float tabWidth, bool setNow){
		hoursObj.GetComponent<UILabel>();

		//new position
		float newXPos = -(tabWidth/2)+(width/2);
		float currentYPos = hoursObj.GetComponent<TweenPosition>().from.y;
		Vector3 newPos = new Vector3(newXPos, currentYPos, 0);

		//set pos
		if(setNow){
			hoursObj.transform.localPosition = newPos;
		}

		//set tween start
		hoursObj.GetComponent<TweenPosition>().from = newPos;
	}
}
