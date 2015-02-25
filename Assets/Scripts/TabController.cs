using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TabController : MonoBehaviour {

	public GameObject tabPrefab;
	public GameObject parentPanel;

	public float tabAreaWidth = 404;
	private int totalTabs = 0;
	private float tabWidth = 200;

	private int currentTab;

	private Dictionary<int, GameObject> tabObjects = new Dictionary<int, GameObject>();

	void Start(){
		//disable input box on phones
		if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer){
			//TouchScreenKeyboard.hideInput = true;
		}

		//create the initial tab
		currentTab = 1;
		createTab();
		AddTab();
	}

	public void AddTab(){
		//create tab and add to list
		createTab();

		//find the new unselected tab widths
		float unselectedWidth = 0;
		if((totalTabs-1) != 0){
			unselectedWidth = (tabAreaWidth - tabWidth)/(totalTabs-1);
		}

		//go through all tabs and reposition them
		foreach(var tabInList in tabObjects){
			int index = tabInList.Key;
			GameObject tab = tabInList.Value;
			RepositionTab(index, tab, unselectedWidth);
		}

		//select the newly created tab
		SelectTab(totalTabs);
	}

	private void createTab(){
		//add a new tab to the list and create it
		totalTabs++;
		tabObjects.Add(totalTabs, NGUITools.AddChild(parentPanel, tabPrefab));
		
		//set the tags knowlage of its own id and this controller
		GameObject newTab;
		tabObjects.TryGetValue(totalTabs, out newTab);
		newTab.GetComponent<TabTween>().tabID = totalTabs;
		newTab.GetComponent<TabTween>().controller = gameObject;

		//set the widget depth
		newTab.GetComponent<UIPanel>().depth = totalTabs;
	}

	public void SelectTab(int tabIndex){
		if(tabIndex < currentTab){
			//play between tabIndex and currentTab (including currentTab)
			for(int i = (tabIndex+1); i <=currentTab; i++){
				GameObject aTab;
				if(tabObjects.TryGetValue(i, out aTab)){
					//play tween forward
					aTab.transform.GetComponent<UITweener>().PlayForward();
				}
			}
		}else{
			//play between currentTab and tabIndex (including tabIndex)
			for(int i = (currentTab+1); i <=tabIndex; i++){
				GameObject aTab;
				if(tabObjects.TryGetValue(i, out aTab)){
					//play tween forward
					aTab.transform.GetComponent<UITweener>().PlayReverse();
				}
			}
		}

		//play selected forwards
		GameObject tabSelected;
		tabObjects.TryGetValue(tabIndex, out tabSelected);
		tabSelected.transform.GetComponent<TabTween>().PlayTab(true);

		//play current backwards
		GameObject tabCurrent;
		tabObjects.TryGetValue(currentTab, out tabCurrent);
		tabCurrent.transform.GetComponent<TabTween>().PlayTab(false);

		//deactive selected's button
		tabSelected.GetComponent<UIButton>().enabled = false;

		//activate current's button
		tabCurrent.GetComponent<UIButton>().enabled = true;

		//update current
		currentTab = tabIndex;
	}

	private void RepositionTab(int index, GameObject tab, float unselectedWidth){
		//set the new position
		float newXPos = (index-1) * unselectedWidth;
		tab.transform.localPosition = new Vector3(newXPos, 0, 0);

		print (index + ": " + newXPos);

		//change the tween start and end point
		float endX = tabWidth - unselectedWidth;
		tab.transform.GetComponent<TweenPosition>().from = new Vector3(newXPos, 0, 0);
		tab.transform.GetComponent<TweenPosition>().to = new Vector3((newXPos+endX), 0, 0);

		//change the hour size for each tab
		if(index == currentTab){
			tab.transform.GetComponent<TabTween>().ReposHours(unselectedWidth, tabWidth, false);
		}else{
			tab.transform.GetComponent<TabTween>().ReposHours(unselectedWidth, tabWidth, true);
		}
	}
}
