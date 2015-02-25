using UnityEngine;
using System.Collections;

public class PlannerJobStore : Data_General {

	public int id;
	public string name;
	public Color colour;
	
	public override void Create(){
		id = int.Parse(data[0]);
		name = data[1];
		colour = HexToColor(data[2]);
	}

	private Color HexToColor(string hex){
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}
}
