using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
	
	public Vector2 gridPosition = Vector2.zero;
	public int perioxi; 
	public int NumberOfPlayersInArea = 0;

	public GameObject[] positions; 
	public bool[] taken = new bool[]{false,false,false,false,false};
	public GameObject[] PlayersInArea;



	// Use this for initialization
	void Start () {
		PlayersInArea = new GameObject[5];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector3 getEmptyPlace(){


		Vector3 emptyPos = new Vector3 (-1, -1, -1);
		for (int i=0; i<positions.Length; i++) {
			if(!taken[i]){
				emptyPos=positions[i].transform.position;
				Debug.Log("got position "+i);
				break;
			}

		}

		if (emptyPos == new Vector3 (-1, -1, -1) ) {
			Debug.Log("Error, players probably more than 5 [see Tile script] perioxi: "+perioxi);		
		}

		return emptyPos;
	}

	public void takePlace(GameObject playergo){
		//check if player exists *Same place as before
		for (int i = 0; i<positions.Length; i++) {
			if(PlayersInArea[i] == playergo){
				return;
			}
		}


		for (int i = 0; i<positions.Length; i++) {
			if(!taken[i]){
				taken[i]=true;
				PlayersInArea[i] = playergo;
				break;
			}
		}
	}

	public void leavePlace(GameObject playergo){
		for (int i = 0; i<positions.Length; i++) {
			if(PlayersInArea[i]==playergo){
				taken[i]=false;
				PlayersInArea[i] = null;
				break;
			}
		}
	}
	

	
}
