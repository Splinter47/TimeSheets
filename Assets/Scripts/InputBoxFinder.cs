using UnityEngine;
using System.Collections;

public class InputBoxFinder : MonoBehaviour {

	//link to the input
	public GameObject inputBoxObj;
	public GameObject greybackObj;
	public GameObject hoursInput;
	public GameObject funcInput;
	public GameObject descrText;
	public GameObject descrInput;

	//current post
	private PostController post;
	//hold the values
	private string hours;
	private string function;
	private string description;

	public void GetPost(PostController postRef){
		//activate the edit window
		inputBoxObj.GetComponent<TweenPosition>().RemoveOnFinished(new EventDelegate(DeactivateInput));
		inputBoxObj.SetActive(true);

		//set the post 
		post = postRef;

		//get its values
		hours = GetText(postRef.hoursObj);
		function = GetText(postRef.functionObj);
		description = GetText(postRef.textObj);

		//set the values
		SetLabel(funcInput, function);
		SetUIInput(hoursInput, hours);
		SetUIInput(descrInput, description);

		//set the new tween start
		TweenPosition twPos = inputBoxObj.GetComponent<TweenPosition>();
		//set global position
		Vector3 newGlobal = post.transform.position;
		inputBoxObj.transform.position = newGlobal;
		//convert to loacal position
		Vector3 newLocal = inputBoxObj.transform.localPosition;
		//offset and assign to start
		twPos.from = new Vector3(newLocal.x-26, newLocal.y + 171, newLocal.z);
	}

	public string GetText(GameObject from){
		UILabel lbl = from.transform.GetComponent<UILabel>();
		return lbl.text;
	}

	public void SetLabel(GameObject to, string newText){
		UILabel lbl = to.transform.GetComponent<UILabel>();
		lbl.text = newText;
		lbl.MarkAsChanged();
	}

	public void SetUIInput(GameObject to, string newText){
		UIInput input = to.transform.GetComponent<UIInput>();
		input.value = newText;
		input.UpdateLabel();
	}

	public void ReturnPost(){
		//deactivate on finish tween
		inputBoxObj.GetComponent<TweenPosition>().AddOnFinished(DeactivateInput);

		//play backwards
		PlayTween(false);

		//get the windows values
		hours = GetText(hoursInput);
		function = GetText(funcInput);
		description = GetText(descrText);

		//set the post's values
		SetLabel(post.hoursObj, hours);
		SetLabel(post.functionObj, function);
		SetLabel(post.textObj, description);
	}

	public void DeactivateInput(){
		print ("deactivating");
		inputBoxObj.SetActive(false);
	}

	public void PlayTween(bool direction){
		//play tween pos
		TweenPosition twPos = inputBoxObj.GetComponent<TweenPosition>();
		twPos.Play(direction);

		//play tween scale
		TweenScale twSca = inputBoxObj.GetComponent<TweenScale>();
		twSca.Play(direction);

		//play tween alpha grey back
		TweenAlpha twAlpha = greybackObj.GetComponent<TweenAlpha>();
		twAlpha.Play(direction);
	}
}
