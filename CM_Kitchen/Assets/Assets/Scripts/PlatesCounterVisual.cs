using System;
using System.Collections.Generic;
using UnityEngine;
public class PlatesCounterVisual : MonoBehaviour {

    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private PlatesCounter platesCounter;

    private List<GameObject> plateVisualGameObjectList;
    private void Awake() {
        plateVisualGameObjectList = new List<GameObject>();
    }
    private void Start() {
        platesCounter.OnPlateSpawned += PlatesCounterOnOnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounterOnOnPlateRemoved;
    }

    private void PlatesCounterOnOnPlateRemoved(object sender, EventArgs e) {
        GameObject plateGameObject = plateVisualGameObjectList[^1];
            plateVisualGameObjectList.Remove(plateGameObject);
            Destroy(plateGameObject);
    }

    private void PlatesCounterOnOnPlateSpawned(object sender, EventArgs e) {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }

}