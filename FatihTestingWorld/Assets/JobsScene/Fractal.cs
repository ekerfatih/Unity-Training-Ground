using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using quaternion = Unity.Mathematics.quaternion;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fractal : MonoBehaviour {

    static readonly int colorAId = Shader.PropertyToID("_ColorA"), colorBId = Shader.PropertyToID("_ColorB"), matricesId = Shader.PropertyToID("_Matrices"), sequenceNumbersId = Shader.PropertyToID("_SequenceNumbers");

    private static MaterialPropertyBlock _propertyBlock;
    struct FractalPart {
        public float3 worldPosition;
        public quaternion rotation, worldRotation;
        public float spingAngle, maxSagAngle,spinVelocity;
    }

    private Vector4[] sequenceNumbers;
    [SerializeField, Range(3, 8)] int depth = 4;
    [SerializeField] private Mesh mesh, leafmesh;
    [SerializeField] private Material material;
    [SerializeField] private Gradient gradientA, gradientB;
    [SerializeField] private Color leafColorA, leafColorB;
    [SerializeField, Range(0f, 90f)] float maxSagAngleA = 15f, maxSagAngleB = 25f;
    [SerializeField, Range(0f, 90f)] float spinVelocityA = 20f, spinVelocityB = 25f;
    [SerializeField, Range(0f, 90f)] float spinSpeedA = 20f, spinSpeedB = 25f;
    [SerializeField, Range(0f, 1f)] float reverseSpinChance = 0.25f;
    private ComputeBuffer[] matricesBuffers;

    static quaternion[] rotations = {
        quaternion.identity,
        quaternion.RotateZ(-0.5f * PI), quaternion.RotateZ(0.5f * PI),
        quaternion.RotateX(0.5f * PI), quaternion.RotateX(-0.5f * PI)
    };

    NativeArray<FractalPart>[] _parts;
    NativeArray<float3x4>[] _matrices;
    private void OnEnable() {
        _parts = new NativeArray<FractalPart>[depth];
        _matrices = new NativeArray<float3x4>[depth];
        matricesBuffers = new ComputeBuffer[depth];
        sequenceNumbers = new Vector4[depth];
        int stride = 12 * 4; // 4 by 4 matrice has 16 float value each 4 byte
        for (int i = 0, length = 1; i < _parts.Length; i++, length *= 5) {
            _parts[i] = new NativeArray<FractalPart>(length, Allocator.Persistent);
            _matrices[i] = new NativeArray<float3x4>(length, Allocator.Persistent);
            matricesBuffers[i] = new ComputeBuffer(length, stride);
            sequenceNumbers[i] = new Vector4(Random.value, Random.value, Random.value, Random.value);
        }
        //_parts[0] = new FractalPart[1];
        _parts[0][0] = CreatePart(0);
        for (int li = 1; li < _parts.Length; li++) {
            NativeArray<FractalPart> levelParts = _parts[li];
            for (int fpi = 0; fpi < levelParts.Length; fpi += 5) {
                for (int ci = 0; ci < 5; ci++) {
                    levelParts[fpi + ci] = CreatePart(ci);
                }
            }
        }
        _propertyBlock ??= new MaterialPropertyBlock(); // ??= if its null
    }
    void OnDisable() {
        for (int i = 0; i < matricesBuffers.Length; i++) {
            matricesBuffers[i].Release();
            _parts[i].Dispose();
            _matrices[i].Dispose();
        }
        _parts = null;
        _matrices = null;
        matricesBuffers = null;
        sequenceNumbers = null;
    }
    private void OnValidate() {
        if (_parts != null && enabled) {
            OnDisable();
            OnEnable();
        }
    }
    FractalPart CreatePart(int childIndex) => new FractalPart {
        spinVelocity =
            (Random.value < reverseSpinChance ? -1f : 1f) *
            radians(Random.Range(spinSpeedA, spinSpeedB)),
        maxSagAngle = radians(Random.Range(maxSagAngleA, maxSagAngleB)),
        rotation = rotations[childIndex],
    };

    void Update() {
        float deltaTime = Time.deltaTime;
        FractalPart rootPart = _parts[0][0];
        rootPart.spingAngle += rootPart.spinVelocity * deltaTime;
        rootPart.worldRotation = mul(transform.rotation, mul(rootPart.rotation, quaternion.RotateY(rootPart.spingAngle)));
        rootPart.worldPosition = transform.position;
        _parts[0][0] = rootPart;
        float objectScale = transform.lossyScale.x;
        float3x3 r = float3x3(rootPart.worldRotation) * objectScale;
        _matrices[0][0] = float3x4(r.c0, r.c1, r.c2, rootPart.worldPosition);
        // _matrices[0][0] = float4x4.TRS(
        //     rootPart.worldPosition, rootPart.worldRotation, float3(objectScale)
        //     );
        float scale = 1f;
        JobHandle jobHandle = default;
        for (int li = 1; li < _parts.Length; li++) {
            scale *= 0.5f;
            jobHandle = new UpdateFractalLevelJob {
                deltaTime = deltaTime,
                scale = scale,
                parents = _parts[li - 1],
                parts = _parts[li],
                matrices = _matrices[li]
            }.ScheduleParallel(_parts[li].Length, 5, jobHandle);
        }
        jobHandle.Complete();
        var bounds = new Bounds(rootPart.worldPosition, 3f * objectScale * Vector3.one);
        int leafIndex = matricesBuffers.Length - 1;
        for (int i = 0; i < matricesBuffers.Length; i++) {
            ComputeBuffer buffer = matricesBuffers[i];
            buffer.SetData(_matrices[i]);
            Color colorA, colorB;
            Mesh instanceMesh;
            if (i == leafIndex) {
                colorA = leafColorA;
                colorB = leafColorB;
                instanceMesh = leafmesh;
            }
            else {
                float gradientInterpolator = i / (matricesBuffers.Length - 2f);
                colorA = gradientA.Evaluate(gradientInterpolator);
                colorB = gradientB.Evaluate(gradientInterpolator);
                instanceMesh = mesh;
            }
            _propertyBlock.SetColor(colorAId, colorA);
            _propertyBlock.SetColor(colorBId, colorB);
            _propertyBlock.SetBuffer(matricesId, buffer);
            _propertyBlock.SetVector(sequenceNumbersId, sequenceNumbers[i]);
            Graphics.DrawMeshInstancedProcedural(instanceMesh, 0, material, bounds, buffer.count, _propertyBlock);
        }
    }

    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    struct UpdateFractalLevelJob : IJobFor {
        public float spinAngleDelta;
        public float scale;
        public float deltaTime;

        [ReadOnly]
        public NativeArray<FractalPart> parents;
        public NativeArray<FractalPart> parts;
        [WriteOnly]
        public NativeArray<float3x4> matrices;
        public void Execute(int i) {
            FractalPart parent = parents[i / 5];
            FractalPart part = parts[i];
            part.spingAngle += part.spinVelocity * deltaTime;
            float3 upAxis = mul(mul(parent.worldRotation, part.rotation), up());
            float3 sagAxis = cross(up(), upAxis);
            float sagMagnitude = length(sagAxis);
            quaternion baseRotation;
            if (sagMagnitude > 0f) {
                sagAxis /= sagMagnitude;
                quaternion sagRotation = quaternion.AxisAngle(sagAxis, part.maxSagAngle * sagMagnitude);
                baseRotation = mul(sagRotation, parent.worldRotation);
            }
            else {
                baseRotation = parent.worldRotation;
            }
            part.worldRotation = mul(baseRotation, mul(part.rotation, quaternion.RotateY(part.spingAngle)));
            //part.worldPosition = (float3)parent.worldPosition + mul(parent.worldRotation, 1.5f * scale * part.direction);
            part.worldPosition = parent.worldPosition + mul(part.worldRotation, float3(0f, 1.5f * scale, 0f));
            parts[i] = part;
            float3x3 r = float3x3(part.worldRotation) * scale;
            matrices[i] = float3x4(r.c0, r.c1, r.c2, part.worldPosition);
            // matrices[i] = float4x4.TRS(
            //     part.worldPosition, part.worldRotation, float3(scale)
            //     );
        }
    }

    #region First

    /*
    private void Start() {
        name = "Fractal" + depth;
        if (depth <= 1) {
            return;
        }
        Fractal childA = CreateChild(Vector3.up, Quaternion.identity);
        Fractal childB = CreateChild(Vector3.right, Quaternion.Euler(0f, 0f, -90f));
        Fractal childC = CreateChild(Vector3.left, Quaternion.Euler(0f, 0f, 90f));
        Fractal childD = CreateChild(Vector3.forward, Quaternion.Euler(90f, 0f, 0f));
        Fractal childE = CreateChild(Vector3.back, Quaternion.Euler(-90f, 0f, 0f));

        childA.transform.SetParent(transform, false);
        childB.transform.SetParent(transform, false);
        childC.transform.SetParent(transform, false);
        childD.transform.SetParent(transform, false);
        childE.transform.SetParent(transform, false);
    }

    Fractal CreateChild(Vector3 direction, Quaternion rotation) {
        Fractal child = Instantiate(this);
        child.depth = depth - 1;
        child.transform.localPosition = 0.75f * direction;
        child.transform.localRotation = rotation;
        child.transform.localScale = 0.5f * Vector3.one;
        return child;
    }
    void Update () {
        transform.Rotate(0f, 22.5f * Time.deltaTime, 0f);
    }
    

    */

    #endregion
}