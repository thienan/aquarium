﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFlocker : MonoBehaviour {

	public FlockingParameters parameters;
	public bool passParametersToFlock;

	float minScale = 0;
	float maxScale = 5000;
	[Range(0, 5000)] // NB: need to match above
	public float fishScale = 1;

	[Range(10, 50)] //NB: What's a good value here?
	public float minimumPopulation = 2;

	[Range(100, 10000)] //NB: What's a good value here?
	public float oldAge = 10;

	void Start () {
		//generateSeekPosition();
		LoadPreferences();
	}

	void Update () {
		if (passParametersToFlock) {
			foreach (Transform child in transform) {
				FlockingFish f = child.gameObject.GetComponent (typeof(FlockingFish)) as FlockingFish;
				f.parameters = parameters;
			}
		}
		if(Random.Range(0, 1000) < 5) {
			//generateSeekPosition();
		}
		foreach (Transform child in transform) {
			child.localScale = new Vector3(fishScale, fishScale, fishScale);
		}
		if (fishScale < maxScale && Input.GetKey("up")) fishScale += 1f;
		if (fishScale > minScale && Input.GetKey("down")) fishScale -= 1f;

		List<GameObject> allFish = getAllFish();
		int numFishes = allFish.Count;
		int realFishCount = 0; //I probably don't need this variable as I can get the count for the aliveFish list.
		List<GameObject> aliveFish = new List<GameObject>();

		foreach(GameObject fish in allFish){
			if(fish.GetComponent<FlockingFish>().dying == false){
				aliveFish.Add(fish.gameObject);
				realFishCount++;
			}
		}

		if(realFishCount > minimumPopulation){
			//Let's find the oldest fish in the alive fishes list.
			GameObject oldestFish = null;
			float oldestAge = 0;

			foreach(GameObject fish in aliveFish){
				if (fish.GetComponent<FlockingFish>().age > oldAge){
					if(fish.GetComponent<FlockingFish>().age > oldestAge){
						oldestFish = fish;
						oldestAge = fish.GetComponent<FlockingFish>().age;
					}
				}
			}

			if(oldestFish != null){
				oldestFish.GetComponent<FlockingFish>().dying = true;
				Debug.Log("One fish set for dying.");
			}
	
		}
	}

	public List<GameObject> getAllFish(){
		List<GameObject> allFish = new List<GameObject>();
		foreach(Transform child in transform){
			allFish.Add(child.gameObject);
		}
		return allFish;
	}

	void generateSeekPosition(){
		// TODO implement
		throw new System.NotImplementedException ();
	}

	void OnValidate() {
		parameters.OnValidate ();
	}

	void SavePreferences() {
		PlayerPrefs.SetFloat("fishScale", fishScale);
	}

	void LoadPreferences() {
		fishScale = PlayerPrefs.GetFloat("fishScale", fishScale);
	}

	void OnApplicationQuit() {
		SavePreferences();
	}

}
