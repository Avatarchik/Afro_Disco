﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorTrigger:MonoBehaviour {
	//Trigger for openning the door
	public BoxCollider myTrigger;

	//Pointer to the animator component
	private Animator myAnimator;
	private int OpenHash = Animator.StringToHash("OpenDoor");

	//Whether the door is allowed to open
	public bool EnableOpening = true;

	// Use this for initialization
	void Start() {
		myAnimator = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update() {

	}

	void OnTriggerEnter(Collider other)
    {
        if (!myAnimator) Start();

		if(EnableOpening) {
			myAnimator.SetBool(OpenHash, true);
		}
	}

	void OnTriggerExit(Collider other) {
		if(EnableOpening) {
			myAnimator.SetBool(OpenHash, false);
		}
	}
}
