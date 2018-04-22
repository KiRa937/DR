using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

	public GameObject bullet;

	public float speed = 10;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
		{
			var b = Instantiate(bullet, GetComponent<Transform>().position, GetComponent<Transform>().rotation);

			b.GetComponent<Rigidbody>().velocity = GetComponent<Transform>().forward * speed;

			GetComponent<AudioSource>().Play();
		}
	}
}
