using UnityEngine;
using System.Collections;

public class GreenCardScript : MonoBehaviour {

	bool clicked = false;
	public int GreenCardID;
	public bool isEnabled = true;

	void Start(){

		 
		clicked = GameManager.instance.GreenClicked;
	}

	void Update(){
		if (GameManager.instance.ActiveGreenCardID == this.GreenCardID && GameManager.instance.PlayerTileClicked) {
			 
			MoveGreenToPlayer(GameManager.instance.PlayerTileSelectedGO);
		}
	}


	void OnMouseDown(){
		if (!isEnabled) {
			return;
		}

		// in case area is 4 5 
		GameManager.instance.DisableRedCards (true);
		GameManager.instance.DisableBlueCards (true);

		clicked = GameManager.instance.GreenClicked;
		if (!clicked) {
			iTween.MoveTo (gameObject, iTween.Hash ("y", 13.3, "x", 2, "z", -4.5, "easetype","spring"));
			iTween.RotateTo (gameObject, iTween.Hash ("y",0,"z", 180, "easetype","spring"));

			GameManager.instance.GreenClicked = true;
			GameManager.instance.ActiveGreenCardID = this.GreenCardID;
		}
			
	}

	void MoveGreenToPlayer(GameObject pl){
		if (!isEnabled) {
			return;
		}
		iTween.MoveTo (gameObject, iTween.Hash ("position",pl.transform.position,"easetype","spring"));
		iTween.RotateTo (gameObject, iTween.Hash ("z", -180, "easetype","spring"));

		//do code for giving the card to selected player


		GameManager.instance.PlayerTileSelectedGO = null;
		GameManager.instance.PlayerTileClicked = false;
		GameManager.instance.GreenClicked = false;
		GameManager.instance.ActiveGreenCardID = -1;


	}
}