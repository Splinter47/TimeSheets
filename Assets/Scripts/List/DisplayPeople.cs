using UnityEngine;
using System.Collections;

public class DisplayPeople: MonoBehaviour{

	private Data_General blockData;

	private string imagesURL = "http://www.samdavies.info/social/images/";

	public void Constructor(Data_General block){
		blockData = block;
		create();
	}

	public void create(){
		print ("created");
	}
}
