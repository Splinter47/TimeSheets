using UnityEngine;
using System.Collections;

public class PlannerTask : Data_General {

	//reference to the PlannerDay
	public PlannerDay theFeed;

	//downloaded values
	public int id;
	public string taskName;
	public string taskDescription;
	public Color taskColour;

	public bool fee = false;
	public bool sam = false;
	public bool chris = false;
	public bool terence = false;

	//gameobjects
	public GameObject colourObj;
	public GameObject nameObject;

	//list of people working on the task and there functions
	public GameObject[] employeePrefabs;

	public override void Create(){
		
		//set labels
		id = int.Parse(data[0]);
		taskName = data[2];
		taskDescription = data[3];
		taskColour = HexToColor(data[4]);

		//see if names exist
		int i = 5;
		while(i < data.Length){
			FindPerson(data[i]);
			i++;
		}

		SetDisplayedTask();
	}

	private void FindPerson(string person){
		if(person.Equals("Fiona")){ fee = true; }
		else if(person.Equals("Sam")){ sam = true; }
		else if(person.Equals("Terence")){ terence = true; }
		else if(person.Equals("Chris")){ chris = true; }
	}

	public void SetDisplayedTask(){
		//set title and colour
		SetLabel(nameObject, taskName);
		colourObj.GetComponent<UISprite>().color = taskColour;
	}

	private string ColorToHex(Color32 color){
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}
	
	private Color HexToColor(string hex){
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}

	public void UpdateThisTask(){
		theFeed.UploadTask(this);
	}
}
