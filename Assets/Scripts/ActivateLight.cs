﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLight : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<Light>().enabled = false;
	}
}
