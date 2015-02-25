using UnityEngine;
using System.Collections;
using System;

public class PeopleFeed : GeneralFeed {

	public GameObject detailWindow;
	public GameObject details1;
	public GameObject details2;

	public GameObject nameObj;
	public GameObject salutationObj;
	public GameObject firstNameObj;
	public GameObject surnameObj;
	public GameObject jobObj;
	public GameObject companyObj;
	public GameObject emailObj;
	public GameObject mobileNumObj;
	public GameObject homeNumObj; 

	public GameObject filterFirstNameObj;
	public GameObject filterSurnameObj;

	//alphas to tween
	public GameObject[] editButtonAlphas;

	private Data_People data;

	void Start(){
		grid = gridObject.GetComponent<UIGrid>();
		grid.Reposition();
		Download();
	}

	protected override void SetFeedReference(GameObject feedObject){
		feedObject.GetComponent<Data_People>().theFeed = this;
	}
		
	public void ShowDetails(Data_People dataRef){
		//get the data
		data = dataRef;

		//the fade out loads the details
		FadeOutDetails();

		//change the grid to 1 column
		SetGridColumns(1);
		Invoke("CentreOnCurrentData", 0.25f);

		//move the window
		PlayTween(detailWindow, true);
	}

	public void CentreOnCurrentData(){

		UICenterOnChild center = NGUITools.FindInParents<UICenterOnChild>(data.gameObject);
		UIPanel panel = NGUITools.FindInParents<UIPanel>(data.gameObject);
		
		if (center != null)
		{
			if (center.enabled)
				center.CenterOn(transform);
		}
		else if (panel != null && panel.clipping != UIDrawCall.Clipping.None)
		{
			UIScrollView sv = panel.GetComponent<UIScrollView>();
			Vector3 offset = -panel.cachedTransform.InverseTransformPoint(data.transform.position);
			if (!sv.canMoveHorizontally) offset.x = panel.cachedTransform.localPosition.x;
			if (!sv.canMoveVertically) offset.y = panel.cachedTransform.localPosition.y;
			SpringPanel.Begin(panel.cachedGameObject, offset, 6f);
		}
	}

	public void HideDetails(){
		//change the grid to 2 column
		SetGridColumns(2);
		Invoke("CentreOnCurrentData", 0.2f);
		
		//move the window
		PlayTween(detailWindow, false);
	}

	private void FadeOutDetails(){
		//load the details in
		details1.GetComponent<UITweener>().AddOnFinished(LoadDetails);
		//fade in on finish
		details1.GetComponent<UITweener>().AddOnFinished(FadeInDetails);
		//fade in
		PlayTween(details1, false);
		PlayTween(details2, false);
	}

	private void FadeInDetails(){
		//remove load
		details1.GetComponent<UITweener>().RemoveOnFinished(new EventDelegate (LoadDetails));
		//remover fade in
		details1.GetComponent<UITweener>().RemoveOnFinished(new EventDelegate (FadeInDetails));
		//fade in
		PlayTween(details1, true);
		PlayTween(details2, true);
	}

	private void LoadDetails(){
		//set label (it has no input)
		SetLabel(nameObj, GetLabel(data.nameObj));
		//set labels and UIInput valus
		SetLabelAndInput(salutationObj, data.salutation);
		SetLabelAndInput(firstNameObj, data.firstName);
		SetLabelAndInput(surnameObj, data.surname);
		SetLabelAndInput(jobObj, GetLabel(data.jobObj));
		SetLabelAndInput(companyObj, GetLabel(data.companyObj));
		SetLabelAndInput(emailObj, GetLabel(data.emailObj));
		SetLabelAndInput(mobileNumObj, GetLabel(data.mobileNumObj));
		SetLabelAndInput(homeNumObj, GetLabel(data.homeNumObj));
	}

	public void ShowEditDetails(){
		//show all the input boxes
		EditDetailsVisibility(true);
	}

	public void HideEditDetails(){
		//show all the input boxes
		EditDetailsVisibility(false);
	}

	private void EditDetailsVisibility(bool isVisable){
		//diable full name
		nameObj.SetActive(!isVisable);

		//enable name objects
		salutationObj.SetActive(isVisable);
		firstNameObj.SetActive(isVisable);
		surnameObj.SetActive(isVisable);

		//enable UIInputs
		SetInputActive(salutationObj, isVisable);
		SetInputActive(firstNameObj, isVisable);
		SetInputActive(surnameObj, isVisable);
		SetInputActive(jobObj, isVisable);
		SetInputActive(companyObj, isVisable);
		SetInputActive(emailObj, isVisable);
		SetInputActive(mobileNumObj, isVisable);
		SetInputActive(homeNumObj, isVisable);

		//tween aplphas
		foreach(GameObject alphaObj in editButtonAlphas){
			PlayTween(alphaObj, isVisable);
		}
	}

	public void SetInputActive(GameObject inputObj, bool isActive){
		UIInput input = inputObj.GetComponent<UIInput>();
		input.enabled = isActive;
	}


	//-------------- FILTERS ----------------------

	public void FilterByFirstName(){
		//get filter box string
		string filterString = GetLabel(filterFirstNameObj);

		//number indicates a preset method in PHP
		FilterBy(1, "%" + filterString + "%");

		StartCoroutine(DownloadBlocks(filterList, downloadURL, 0));
	}

	public void FilterBySurname(){
		//get filter box string
		string filterString = GetLabel(filterSurnameObj);
		
		//number indicates a preset method in PHP
		FilterBy(2, "%" + filterString + "%");

		StartCoroutine(DownloadBlocks(filterList, downloadURL, 0));
	}

	public void UploadData(){
		WWWForm form = new WWWForm();

		//get the strings from detail objects
		data.salutation = GetLabel(salutationObj);
		data.firstName = GetLabel(firstNameObj);
		data.surname = GetLabel(surnameObj);
		SetLabel(data.nameObj, data.salutation + " " + data.firstName + " " + data.surname);
		SetLabel(data.jobObj, GetLabel(jobObj));
		SetLabel(data.companyObj, GetLabel(companyObj));
		SetLabel(data.emailObj, GetLabel(emailObj));
		SetLabel(data.mobileNumObj, GetLabel(mobileNumObj));
		SetLabel(data.homeNumObj, GetLabel(homeNumObj));

		//get the new data
		form.AddField("id", data.id);
		form.AddField("salutation", data.salutation);
		form.AddField("firstName", data.firstName);
		form.AddField("surname", data.surname);
		form.AddField("job", GetLabel(data.jobObj));
		form.AddField("company", GetLabel(data.companyObj));
		form.AddField("email", GetLabel(data.emailObj));
		form.AddField("mobile", GetLabel(data.mobileNumObj));
		form.AddField("landline", GetLabel(data.homeNumObj));

		StartCoroutine(UploadBlock(form, uploadURL));
	}


	//-------------- SORTING ----------------------
	public void SortByName(){
		SortByFunction(GetPersonName);
	}

	public void SortByJob(){
		SortByFunction(GetPersonJob);
	}

	private void SortByFunction(Func<Data_People, string> GetAttribute){
		foreach(GameObject person in displayedBlocks){
			//get the data for according to the function
			Data_People personData = person.GetComponent<Data_People>();
			string newName = GetAttribute(personData);
			//change the object name
			person.name = newName;
		}
		//sort grid by object name
		grid.Reposition();
	}

	private string GetPersonName(Data_People person){
		return person.nameObj.GetComponent<UILabel>().text;
	}

	private string GetPersonJob(Data_People person){
		return person.jobObj.GetComponent<UILabel>().text;
	}
	//-----------------------------------------

}
