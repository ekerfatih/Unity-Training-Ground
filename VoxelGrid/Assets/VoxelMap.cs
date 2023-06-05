using System;
using System.Threading.Tasks.Sources;
using UnityEngine;
using UnityEngine.Serialization;

public class VoxelMap : MonoBehaviour {
    public float size = 2f;

    public int voxelResolution = 8; // How many grid inside 1 chunk
    public int chunkResolution = 2; // chunkResolution by chunkResolution 

    public VoxelGrid voxelGridPrefab;

    private VoxelGrid[] _chunks;
    private VoxelStencil[] _stencils = {
        new VoxelStencil(),
        new VoxelStencilCircle()
    };


    private float _chunkSize, _voxelSize, _halfSize;

    private static readonly string[] FillTypeNames = { "Filled", "Empty" };
    private static readonly string[] RadiusNames = { "0", "1", "2", "3", "4", "5" };
    private static string[] _stencilNames = { "Square", "Circle" };
    private int _fillTypeIndex, _radiusIndex, _stencilIndex;
    private void Awake() {
        _halfSize = 0.5f * size;
        _chunkSize = size / chunkResolution;
        _voxelSize = _chunkSize / voxelResolution;

        _chunks = new VoxelGrid[chunkResolution * chunkResolution];
        for (int i = 0, y = 0; y < chunkResolution; y++) {
            for (int x = 0; x < chunkResolution; x++, i++) {
                CreateChunk(i, x, y);
            }
        }
        BoxCollider box = gameObject.AddComponent<BoxCollider>();
        box.size = new Vector3(size, size);
    }


    private void CreateChunk(int i, int x, int y) {
        VoxelGrid chunk = Instantiate(voxelGridPrefab) as VoxelGrid;
        chunk.Initialize(voxelResolution, _chunkSize);
        chunk.transform.parent = transform;
        chunk.transform.localPosition = new Vector3(x * _chunkSize - _halfSize, y * _chunkSize - _halfSize);
        _chunks[i] = chunk;
        if (x > 0) {
            _chunks[i - 1].xNeighbor = chunk;
        }
        if (y > 0) {
            _chunks[i - chunkResolution].yNeighbor = chunk;
            if (x > 0) {
                _chunks[i - chunkResolution - 1].xyNeighbor = chunk;
            }
        }
    }

    private void Update() {
        if (Input.GetMouseButton(0)) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
                if (hit.collider.gameObject == gameObject) {
                    EditVoxels(transform.InverseTransformPoint(hit.point));
                }
            }
        }
    }

    private void EditVoxels(Vector3 point) {
        int centerX = (int)((point.x + _halfSize) / _voxelSize);
        int centerY = (int)((point.y + _halfSize) / _voxelSize);

        int xStart = (centerX - _radiusIndex - 1) / voxelResolution;
        if (xStart < 0) {
            xStart = 0;
        }
        int xEnd = (centerX + _radiusIndex) / voxelResolution;
        if (xEnd >= chunkResolution) {
            xEnd = chunkResolution - 1;
        }
        int yStart = (centerY - _radiusIndex - 1) / voxelResolution;
        if (yStart < 0) {
            yStart = 0;
        }
        int yEnd = (centerY + _radiusIndex) / voxelResolution;
        if (yEnd >= chunkResolution) {
            yEnd = chunkResolution - 1;
        }
        VoxelStencil activeStencil = _stencils[_stencilIndex];
        activeStencil.Initialize(_fillTypeIndex == 0, _radiusIndex);
        int voxelYOffset = yEnd * voxelResolution;
        for (int y = yEnd; y >= yStart; y--) {
            int i = y * chunkResolution + xEnd;
            int voxelXOffset = xEnd * voxelResolution;
            for (int x = xEnd; x >= xStart; x--, i--) {
                activeStencil.SetCenter(centerX - voxelXOffset, centerY - voxelYOffset);
                _chunks[i].Apply(activeStencil);
                voxelXOffset -= voxelResolution;
            }
            voxelYOffset -= voxelResolution;
        }
    }
    private void OnGUI() {
        GUILayout.BeginArea(new Rect(4f, 4f, 150f, 500f));
        GUILayout.Label("Fill Type");
        _fillTypeIndex = GUILayout.SelectionGrid(_fillTypeIndex, FillTypeNames, 2);
        GUILayout.Label("Radius");
        _radiusIndex = GUILayout.SelectionGrid(_radiusIndex, RadiusNames, 6);
        GUILayout.Label("Stencil");
        _stencilIndex = GUILayout.SelectionGrid(_stencilIndex, _stencilNames, 2);
        GUILayout.EndArea();
    }
    private void OnValidate() {
        print(_fillTypeIndex);
    }
}