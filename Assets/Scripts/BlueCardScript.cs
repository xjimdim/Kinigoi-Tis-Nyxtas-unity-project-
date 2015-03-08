using UnityEngine;
using System.Collections;

public class BlueCardScript : MonoBehaviour {

	bool clicked = false;
	public int BlueCardID;
	public bool isEnabled = true;
	public GameObject okButton;
	
	void Start(){
		
		
		clicked = GameManager.instance.BlueClicked;
	}
	
	void Update(){
		if (gameObject.transform.position.x == 2f && gameObject.transform.position.y == 13.3f && gameObject.transform.position.z == -4.5f) {
			okButton.SetActive(true);
		}
		
	}
	
	
	void OnMouseDown(){
		if (!isEnabled) {
			return;
		}

		// in case area is 4 5 
		GameManager.instance.DisableRedCards (true);
		GameManager.instance.DisableGreenCards (true);


		clicked = GameManager.instance.BlueClicked;
		if (!clicked) {
			iTween.MoveTo (gameObject, iTween.Hash ("y", 13.3, "x", 2, "z", -4.5, "easetype","spring"));
			iTween.RotateTo (gameObject, iTween.Hash ("y",0,"z", 180, "easetype","spring"));
			
			GameManager.instance.BlueClicked = true;
			GameManager.instance.ActiveBlueCardID = this.BlueCardID;
		}
		
	}

}
