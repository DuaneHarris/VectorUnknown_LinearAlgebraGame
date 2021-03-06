﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float Speed = 0.1f;
	public GameObject GameManager;

	private int State = 0;
	private Vector3 StartPosition;
	private Vector3 EndPosition;
	private Vector3 Direction;

	private Queue<Vector3> Route = new Queue<Vector3>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (State == 1) {
			
			if (Direction == Vector3.zero) {
				Direction = Route.Dequeue();
				StartPosition = transform.position;
				EndPosition = transform.position + Direction;
			}

			transform.position += Speed * Vector3.Normalize(Direction);

			if (Vector3.Distance(transform.position,StartPosition) >= Vector3.Distance(EndPosition,StartPosition)) {
				transform.position = EndPosition;
				Direction = Vector3.zero;
				if (Route.Count == 0) {
					State = 0;
					GameManager.GetComponent<Game1> ().NextPuzzle ();
				}
			}
		}
	}

	public void Move (Vector3[] route) {
		for (int i = 0; i < route.Length; i++) {
			Route.Enqueue (route [i]);
		}
		State = 1;
	}

}
