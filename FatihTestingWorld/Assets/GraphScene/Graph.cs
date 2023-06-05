using UnityEngine;
public class Graph : MonoBehaviour {

    [SerializeField]
    private Transform pointPrefab;

    [SerializeField] [Range(10, 200)]
    private int resolution = 10;
    private Transform[] _points;

    [SerializeField]
    FunctionLibrary.FunctionName function;


    private void Awake() {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        _points = new Transform[resolution * resolution];
        for (int i = 0; i < _points.Length; i++) {
            Transform point = _points[i] = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
        }
    }
    // void Update() {
    //     FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
    //     float time = Time.time;
    //     for (int i = 0; i < _points.Length; i++) {
    //         Transform point = _points[i];
    //         Vector3 position = point.localPosition;
    //         position.y = f(position.x, position.z, time);
    //         point.localPosition = position;
    //     }
    // }

    void Update() {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < _points.Length; i++, x++) {
            if (x == resolution) {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            _points[i].localPosition = f(u, v, time);
        }
    }
}