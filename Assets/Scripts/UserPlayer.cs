using UnityEngine;
using System.Collections;

public class UserPlayer : Player {


	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override void TurnUpdate ()
	{
		GameManager.instance.movedone = false;
		if (Vector3.Distance(moveDestination, transform.position) > 0.1f) {
			moveStarted=true;

			if(GameManager.instance.fromPlace == null){
				//this is first move
				transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;
			}	

			if(GameManager.instance.fromPlace != null && GameManager.instance.place.transform.name != GameManager.instance.fromPlace.transform.name ){
				//second++ move and only if its not the same
				transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;
			}	


			if (Vector3.Distance(moveDestination, transform.position) <= 0.1f) {
				// almost finished moving

				transform.position = moveDestination;
				GameManager.instance.place.NumberOfPlayersInArea++;
				GameManager.instance.place.takePlace(GameManager.instance.players[GameManager.instance.currentPlayerIndex].gameObject);
				Debug.Log("onoma: "+GameManager.instance.players[GameManager.instance.currentPlayerIndex].gameObject.name);

				if(GameManager.instance.fromPlace != null && GameManager.instance.place.transform.name != GameManager.instance.fromPlace.transform.name ){
					GameManager.instance.fromPlace.NumberOfPlayersInArea--;
					GameManager.instance.fromPlace.leavePlace(GameManager.instance.players[GameManager.instance.currentPlayerIndex].gameObject);
				}
				else{
					Debug.Log("That was the first movement for that player");
				}

				GameManager.instance.waitfordicedelete=false;
				GameManager.instance.DisableCardsForArea(destinationText);


				//GameManager.instance.nextTurn ();



				moveStarted=false;  //move finished

				//wait for end turn.

			}
		}
		
		base.TurnUpdate ();
	}
}
