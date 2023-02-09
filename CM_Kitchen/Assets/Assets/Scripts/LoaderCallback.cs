using System;
using UnityEngine;
public class LoaderCallback : MonoBehaviour {

    private bool _isfirstUpdate = true;

    private void Update() {
        if (_isfirstUpdate) {
            _isfirstUpdate = false;
            
            Loader.LoaderCallback();
        }
    }

}