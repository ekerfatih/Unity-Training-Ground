using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;
[CreateAssetMenu]
public class TextureData : UpdateableData {

    private const int textureSize = 512;
    private const TextureFormat textureFormat = TextureFormat.RGB565;
    public Layer[] layers;

    private float savedMinHeight;
    private float savedMaxHeight;

    public void ApplyToMaterial(Material material) {
        material.SetInt("layerCount", layers.Length);
        material.SetColorArray("baseColors", layers.Select(x => x.tint).ToArray());
        material.SetFloatArray("baseStartHeights", layers.Select(x => x.startHeight).ToArray());
        material.SetFloatArray("baseBlends", layers.Select(x => x.blendStrength).ToArray());
        material.SetFloatArray("baseColorStrength", layers.Select(x => x.tintStrength).ToArray());
        material.SetFloatArray("baseTextureScales", layers.Select(x => x.textureScale).ToArray());
        Texture2DArray texturesArray = GenerateTextureArray(layers.Select(x => x.Texture).ToArray());
        material.SetTexture("baseTextures", texturesArray);
        UpdateMeshHeights(material, savedMinHeight, savedMaxHeight);
    }

    Texture2DArray GenerateTextureArray(Texture2D[] textures) {
        Texture2DArray texture2DArray = new Texture2DArray(textureSize, textureSize, textures.Length, textureFormat, true);
        for (int i = 0; i < textures.Length; i++) {
            texture2DArray.SetPixels(textures[i].GetPixels(), i);
            texture2DArray.Apply();
        }
        return texture2DArray;
    }

    public void UpdateMeshHeights(Material material, float minHeight, float maxHeight) {
        savedMinHeight = minHeight;
        savedMaxHeight = maxHeight;
        material.SetFloat("minHeight", minHeight);
        material.SetFloat("maxHeight", maxHeight);
    }
    [System.Serializable]
    public class Layer {
        public Texture2D Texture;
        public Color tint;
        [FormerlySerializedAs("tintStrenght")]
        [Range(0, 1)]
        public float tintStrength;
        [Range(0, 1)]
        public float startHeight;
        [Range(0, 1)]
        public float blendStrength;
        public float textureScale;

    }

}