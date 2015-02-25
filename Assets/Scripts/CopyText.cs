using UnityEngine;
using System.Collections;

public class CopyText : MonoBehaviour {

	public UILabel labelToCopy;
	private UILabel thisLabel;

	// Use this for initialization
	void Start () {
		//find own label
		thisLabel = transform.GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {
		thisLabel.text = labelToCopy.text;
		thisLabel.MarkAsChanged();
	}
}
