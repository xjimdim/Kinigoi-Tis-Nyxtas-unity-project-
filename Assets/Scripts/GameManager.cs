using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	 
	public int NumberOfPlayers;
	public GameObject PlayerPrefab; 
	public GameObject FSD;
	public GameObject SSD; 

	// FLAGS
	public bool movedone = false;
	public bool bothsleeping = false;
	public bool bothcreated = false;
	public bool bothdeleted = true;
	public bool waitfordicedelete = true;
	public bool waitForPlayerToRoll = true;

	public bool GreenClicked = false;
	public int ActiveGreenCardID = -1;
	public GameObject PlayerTileSelectedGO; 
	public bool PlayerTileClicked = false;

	public bool RedClicked = false;
	public int ActiveRedCardID = -1;

	public bool BlueClicked = false;
	public int ActiveBlueCardID = -1;

	//FOR CARDS GENERATOR
	public List <GameObject> StartGreenCards = new List<GameObject>();
	List <GameObject> GreenCards = new List<GameObject>();
	public List <GameObject> StartRedCards = new List<GameObject>();
	List <GameObject> RedCards = new List<GameObject>();
	public List <GameObject> StartBlueCards = new List<GameObject>();
	List <GameObject> BlueCards = new List<GameObject>();
	 
	public List <Player> players = new List<Player>();
	public int currentPlayerIndex = 0;

	public Tile place = null;
	public Tile fromPlace = null;



	// private vars

	GameObject placeobj2 = null;
	
	void Awake() {
		instance = this;

		//DUMMIES FOR PLAYER NAMES
		for (int i = 0; i<5; i++){
			PlayerInfo.PlayerNames.Add ("Player "+(i+1));
		}


		//this needs reverse to get the correct order if dummies are not used 
		//PlayerInfo.PlayerNames.Reverse (); 



		NumberOfPlayers = PlayerInfo.PlayerNames.Count;
		generatePlayers();
		generateCards();
		
		
		DisableBlueCards (true);
		DisableRedCards (true);
		DisableGreenCards (true);
	}
	

	
	// Update is called once per frame
	void Update () {
		players [currentPlayerIndex].TurnUpdate (); //calls userplayer script

		makeMove ();

		GameObject fs = GameObject.Find ("FourSidedDie(Clone)");
		GameObject ss = GameObject.Find ("SixSidedDie(Clone)");


		//checking dice status and updating flags
		if (fs != null && ss != null) {
			bothcreated = true;
			if (fs.rigidbody.IsSleeping () && ss.rigidbody.IsSleeping () && waitfordicedelete) {

				bothsleeping = true;

			}
			else{
				bothsleeping = false;
			}
		}
		else {
			bothcreated = false;
		}


	}

	public void EndTurnClicked(){
		nextTurn ();
		GameObject RollButtonGO = GameObject.Find ("RollButton");
		RollButtonGO.GetComponent<Button> ().interactable = true; 
	}
	
	public void nextTurn() {
		if (currentPlayerIndex + 1 < players.Count) {
			currentPlayerIndex++;
		} else {
			currentPlayerIndex = 0;
		}
	}

	public void ChangeCurrentPlayerText(string txt){
		GameObject CurrPlayerTextGO = GameObject.Find ("CurrentPlayerText");
		CurrPlayerTextGO.GetComponent<Text> ().text = txt; 
	}

	public void makeMove(){ //called onUpdate
		ChangeCurrentPlayerText(players [currentPlayerIndex].PlName + " Plays");

		if (!bothcreated && !waitForPlayerToRoll){ // afto simenei pws o paiktis molis ekane click gia na riksei zaria
			RollDice ();
			waitForPlayerToRoll = true;

		}				
		if (bothsleeping && waitfordicedelete) {  // afto simenei pws ta zaria exoune rixtei 

			//evresi topothesias
			GameObject dieTextGameObject = GameObject.Find ("DieText");
			string dtext = dieTextGameObject.GetComponent<TextMesh> ().text;
			 
			if (dtext == "4" || dtext == "5")
				dtext = "45";
			if (dtext == "2" || dtext == "3")
				dtext = "23";
			if (dtext == "7" ){
				dtext = "10";
			}
			if (dtext == "1"){
				RollDice();    //se periptosi sfalmatos, not optimal vevea 
			}




			GameObject placeobj = GameObject.Find (dtext);

			string cloc = players[currentPlayerIndex].currentLocation;

			if(cloc != "none"){  // if this is not the first move of the player find FROM location
				placeobj2 = GameObject.Find (cloc);
			}
			if (placeobj != null) {
				place = placeobj.GetComponent<Tile> ();


				if (placeobj2 != null) {
					fromPlace = placeobj2.GetComponent<Tile> ();

				}

				moveCurrentPlayer (place, dtext);

			}




			StartCoroutine (DestroyDice (5));
						
		}

	}

	public void moveCurrentPlayer(Tile destTile, string dt) { //called onUpdate


		players [currentPlayerIndex].moveDestination = destTile.getEmptyPlace() + 0.5f * Vector3.up;

		if (!players[currentPlayerIndex].moveStarted) {
			players [currentPlayerIndex].currentLocation = players [currentPlayerIndex].destinationText;  //the from destination;
		}

		players [currentPlayerIndex].destinationText = dt;  // for the deactivation of the cards when someone is in the destination tile 
	}



	public void DisableCardsForArea(string area){
		if (area == "23") {
			DisableBlueCards(true);
			DisableRedCards(true);
			DisableGreenCards(false);
		}
		if (area == "8") {
			DisableGreenCards(true);
			DisableBlueCards(true);
			DisableRedCards(false);
		}
		if (area == "6") {
			DisableGreenCards(true);
			DisableRedCards(true);
			DisableBlueCards(false);
		}
		if (area == "10" || area == "9") {
			DisableGreenCards(true);
			DisableRedCards(true);
			DisableBlueCards(true);
		}
		if (area == "45") {
			DisableBlueCards(false);
			DisableRedCards(false);
			DisableGreenCards(false);
		}


	}

	public void DisableGreenCards(bool ac){
		if(ac){
			foreach (GameObject gcardz in GreenCards) {
				gcardz.GetComponent<GreenCardScript>().isEnabled = false;	
				gcardz.transform.renderer.material.color= Color.gray;
			}
		}
		else{
			foreach (GameObject gcardz in GreenCards) {
				gcardz.GetComponent<GreenCardScript>().isEnabled = true;	
				gcardz.transform.renderer.material.color= Color.green;
			}
		}
	}

	public void DisableRedCards(bool ac){
		if(ac){
			foreach (GameObject rcardz in RedCards) {
				rcardz.GetComponent<RedCardScript>().isEnabled = false;
				rcardz.transform.renderer.material.color= Color.gray;
			}
		}
		else{
			foreach (GameObject rcardz in RedCards) {
				rcardz.GetComponent<RedCardScript>().isEnabled = true;
				rcardz.transform.renderer.material.color= Color.red;
			}
		}
	}

	public void DisableBlueCards(bool ac){
		if(ac){
			foreach (GameObject bcardz in BlueCards) {
				bcardz.GetComponent<BlueCardScript>().isEnabled = false;
				bcardz.transform.renderer.material.color= Color.gray;
			}
		}
		else{
			foreach (GameObject bcardz in BlueCards) {
				bcardz.GetComponent<BlueCardScript>().isEnabled = true;
				bcardz.transform.renderer.material.color= Color.blue;
			}
		}
	}

	


	public void RollDice(){
		
		Vector3 position = new Vector3(-3.24f, 10.5f, 0.42f);
		Vector3 rotation = new Vector3(Random.Range(0f, 360f),Random.Range(0f, 360f),Random.Range(0f, 360f));
		GameObject fsided = ((GameObject)Instantiate (FSD, position, Quaternion.Euler (rotation)));
		fsided.rigidbody.AddTorque(Random.Range(150f,300f),Random.Range(10f,32f),Random.Range(160f,280f));
		
		
		Vector3 position2 = new Vector3(0f, 10.5f, 0.42f);
		Vector3 rotation2 = new Vector3(Random.Range(0f, 360f),Random.Range(0f, 360f),Random.Range(0f, 360f));
		GameObject ssided = ((GameObject)Instantiate (SSD, position2, Quaternion.Euler (rotation2)));
		ssided.rigidbody.AddTorque(Random.Range(150f,300f),Random.Range(10f,32f),Random.Range(160f,280f));
		
		
	}

	
	void generatePlayers() {
		UserPlayer firstplayer;
		UserPlayer player;

		Color[] PlayerColor = new Color[] {Color.gray, Color.blue, Color.yellow, Color.red, Color.black};

		firstplayer = ((GameObject)Instantiate (PlayerPrefab, new Vector3 (-6.87f, 0.5f, 2.67f), Quaternion.Euler (new Vector3 (0, 0, 0)))).GetComponent<UserPlayer> ();
		firstplayer.transform.renderer.material.color = PlayerColor [0];
		firstplayer.PlName = PlayerInfo.PlayerNames [0];
		players.Add (firstplayer);

		for (int i = 1; i<NumberOfPlayers; i++) {
			float temp = players[i-1].transform.position.x + 1.2f;
			player = ((GameObject)Instantiate (PlayerPrefab, new Vector3 (temp, 0.5f, 2.67f), Quaternion.Euler (new Vector3 (0, 0, 0)))).GetComponent<UserPlayer> ();
			player.transform.renderer.material.color = PlayerColor[i];
			player.PlName = PlayerInfo.PlayerNames [i];
			players.Add (player);
		
		}
	}

	void generateCards(){

		//GENERATING GREEN CARDS
		GameObject firstgreencard;
		firstgreencard = ((GameObject)Instantiate(StartGreenCards[0], new Vector3(-17f,0.04f,4f), Quaternion.Euler(new Vector3(0,90,0))));
		firstgreencard.GetComponent<GreenCardScript> ().GreenCardID = 0;
		GreenCards.Add(firstgreencard);

		GameObject GCard;
		for(int i=1; i<StartGreenCards.Count; i++){
			float temp = GreenCards[i-1].transform.position.y +0.045f;
			GCard = ((GameObject)Instantiate(StartGreenCards[i], new Vector3(-17f,temp,4f), Quaternion.Euler(new Vector3(0,90,0))));
			GCard.GetComponent<GreenCardScript> ().GreenCardID = i;
			GreenCards.Add(GCard);
		}

		//GENERATING RED CARDS
		GameObject firstredcard;
		firstredcard = ((GameObject)Instantiate(StartRedCards[0], new Vector3(-17f,0.04f,-1.5f), Quaternion.Euler(new Vector3(0,90,0))));
		firstredcard.GetComponent<RedCardScript> ().RedCardID = 0;
		RedCards.Add(firstredcard);
		
		GameObject RCard;
		for(int i=1; i<StartRedCards.Count; i++){
			float temp2 = RedCards[i-1].transform.position.y +0.045f;
			RCard = ((GameObject)Instantiate(StartRedCards[i], new Vector3(-17f,temp2,-1.5f), Quaternion.Euler(new Vector3(0,90,0))));
			RCard.GetComponent<RedCardScript> ().RedCardID = i;
			RedCards.Add(RCard);
		}

		//GENERATING BLUE CARDS
		GameObject firstbluecard;
		firstbluecard = ((GameObject)Instantiate(StartBlueCards[0], new Vector3(-17f,0.04f,-7.1f), Quaternion.Euler(new Vector3(0,90,0))));
		firstbluecard.GetComponent<BlueCardScript> ().BlueCardID = 0;
		BlueCards.Add(firstbluecard);
		
		GameObject BCard;
		for(int i=1; i<StartBlueCards.Count; i++){
			float temp3 = BlueCards[i-1].transform.position.y +0.045f;
			BCard = ((GameObject)Instantiate(StartBlueCards[i], new Vector3(-17f,temp3,-7.1f), Quaternion.Euler(new Vector3(0,90,0))));
			BCard.GetComponent<BlueCardScript> ().BlueCardID = i;
			BlueCards.Add(BCard);
		}




	}





	public IEnumerator DestroyDice(float duration)		
	{
		GameObject fs = GameObject.Find ("FourSidedDie(Clone)");
		GameObject ss = GameObject.Find ("SixSidedDie(Clone)");

						yield return new WaitForSeconds(duration);

						Destroy (ss);
						Destroy (fs);
						waitfordicedelete = true;

						yield break;

				
	}
}
