using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [System.Serializable]
    public struct ShaderMeshData
    {
        public List<MeshRenderer> primaryMeshList;
        public List<MeshRenderer> secondaryMeshList;
        public List<MeshRenderer> tertiaryMeshList;
        public List<MeshRenderer> quaternaryMeshList;
    }

    public ShaderMeshData shaderMeshData;
    // public GameObject shipVisualPrefab;
    public ShaderData CurrentShader;
    public GameObject attackSource;
    public GameObject exhaustVFXSource;

    public void ApplyNewShader(ShaderData shaderData)
    {
        CurrentShader = shaderData;

        foreach (MeshRenderer meshRenderer in shaderMeshData.primaryMeshList)
        {
            meshRenderer.material = CurrentShader.primary;
        }

        foreach (MeshRenderer meshRenderer in shaderMeshData.secondaryMeshList)
        {
            meshRenderer.material = CurrentShader.secondary;
        }

        foreach (MeshRenderer meshRenderer in shaderMeshData.tertiaryMeshList)
        {
            meshRenderer.material = CurrentShader.tertiary;
        }

        foreach (MeshRenderer meshRenderer in shaderMeshData.quaternaryMeshList)
        {
            meshRenderer.material = CurrentShader.quaternary;
        }
    }

}