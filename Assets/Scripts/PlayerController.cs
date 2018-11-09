using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
public float speed = 15.0f;
public float padding = 1.0f;
public GameObject laserPrefab;
public float projectileSpeed;
public float firingRate =0.2f;
public float health = 250f;
public AudioClip fireSound; 
	float xmin;
	float xmax;
	
	void Start() {
		/// Restrict camera using x min and x max with padding to compensate for edges
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint (new Vector3(0,0 , distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint (new Vector3(1,0, distance)); 
		xmin = leftmost.x +padding;
		xmax = rightmost.x -padding; 
	
	}
	void Fire(){
			Vector3 offset = new Vector3(0,1,0); 
			GameObject beam = Instantiate (laserPrefab, transform.position+offset, Quaternion.identity) as GameObject;
			beam.rigidbody2D.velocity = new Vector3(0,projectileSpeed, 0);
			AudioSource.PlayClipAtPoint (fireSound, transform.position); 
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("Fire", 0.000001f, firingRate);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ("Fire");
		}
		/// Left Key, Right Key Movement
		if (Input.GetKey (KeyCode.LeftArrow)) {
		///transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
			transform.position +=Vector3.left *speed *Time.deltaTime; 
			
			
		}else if(Input.GetKey (KeyCode. RightArrow)){
		///transform.position += new Vector3(+speed * Time.deltaTime, 0, 0);
			transform.position+= Vector3.right *speed * Time.deltaTime;
		}
		///Restrict player to play space
		float newX = Mathf.Clamp(transform.position.x, xmin, xmax); 
		
		transform.position = new Vector3(newX, transform.position.y, transform.position.z); 
	
	}
	void OnTriggerEnter2D (Collider2D collider) {
	
		Projectile missile=collider.gameObject.GetComponent <Projectile>();
		if (missile) {
			Debug.Log ("Player Collided with the missile");
			health -= missile.GetDamage();
			missile.Hit();
			if (health <= 0) {
				Die(); 
			}
		}
	}
	void Die(){
		LevelManager man= GameObject.Find ("LevelManager").GetComponent <LevelManager>();
		man.LoadLevel("Win Screen");
		Destroy(gameObject);
	}
}

