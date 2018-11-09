using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemyPrefab; 
	public float width = 10f;
	public float height = 5f; 
	private bool movingRight = true;
	public float speed = 5f;
	private float xmax; 
	private float xmin; 
	public float spawnDelay= 0.5f;

	// Use this for initialization
	void Start () {
		///Boundaries of Camera///
		float distanceToCamera= transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0, distanceToCamera));
		Vector3 rightEdge = Camera.main.ViewportToWorldPoint (new Vector3 (1,0, distanceToCamera));
		xmax= rightEdge.x;
		xmin = leftBoundary.x;
		SpawnUntilFull(); 
		///Instantiates enemies at child Position///
		
	}
	void SpawnEnemies () {
		foreach( Transform child in transform) {
			GameObject enemy = Instantiate (enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child; 
		}
	}
	void SpawnUntilFull (){
		Transform freePosition= NextFreePosition ();
		if (freePosition){
			GameObject enemy = Instantiate (enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition; 
		}
		if (NextFreePosition()) {
		Invoke ("SpawnUntilFull", spawnDelay);
		}
	}
	/// Draws wire cube gizmo/// 
	public void OnDrawGizmos () {
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height)); 
	}
	// Update is called once per frame
	void Update () {
	//Controls speed to either left or right
		if (movingRight) {
			transform.position+= Vector3.right*speed*Time.deltaTime;
		}else{
			transform.position+= Vector3.left* speed*Time.deltaTime;
		}		
		//Moves either left or right once it hits the boundary set
		float rightEdgeOfFormation = transform.position.x+ 0.5f*width;
		float leftEdgeOfFormation = transform.position.x - 0.5f*width;
		if(leftEdgeOfFormation <xmin) {
			movingRight= true;
			}else if (rightEdgeOfFormation >xmax){
				movingRight = false;
		}
			///Check if all enemies dead
			if(AllMembersDead()) {
				Debug.Log ("Empty Formation");
				SpawnUntilFull(); 
				}
		}
		Transform NextFreePosition() {
			foreach(Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount ==0) {
				return childPositionGameObject ; 
			}
		}
		return null; 
		}
			bool AllMembersDead(){
				foreach(Transform childPositionGameObject in transform) {
					if (childPositionGameObject.childCount >0) {
						return false; 
					}
				}
				return true; 
			
			}
	
}