using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public Vector3 moveDestination;
	public string destinationText = "none";
	public float moveSpeed = 10.0f;
	public string currentLocation = "none";
	public bool moveStarted = false;
	public string PlName = "PLAYER";
	
	void Awake () {
		moveDestination = transform.position;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public virtual void TurnUpdate () {
		// afth i methodos ginete override sto userplayer srript ;)
	}
}
