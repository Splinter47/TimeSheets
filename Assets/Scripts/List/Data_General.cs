using UnityEngine;
using System.Collections;

public abstract class Data_General : MonoBehaviour {

	protected string[] data;

	public void AddVariables(string bigString){

		//divide into its data components
		string[] attStringSeparators = {"<!>"};
		data = bigString.Split(attStringSeparators, System.StringSplitOptions.None);

		Create();
	}

	abstract public void Create();

	protected string GetLabel(GameObject obj){
		return obj.GetComponent<UILabel>().text;
	}

	protected void SetLabel(GameObject to, string newText){
		UILabel lbl = to.transform.GetComponent<UILabel>();
		lbl.text = newText;
		lbl.MarkAsChanged();
	}
}
