using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFader : MonoBehaviour
{
    [SerializeField] private List<MaterialSwitcher> MaterialSwitchers;

    public IEnumerator FadeOut(List<Renderer> MeshesRenderer, float FadeOutDuration)
    {
        foreach (Renderer mesh in MeshesRenderer)
            ChangeRenderMode(mesh);
        
        float timer = 0f;
        while (timer < FadeOutDuration)
        {
            MeshesRenderer.ForEach(r => r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b,
                Mathf.Lerp(1f, 0f, timer/FadeOutDuration)));
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    private void ChangeRenderMode(Renderer mesh)
    {
        var materialsCopy = mesh.materials;
        for (int index = 0; index < mesh.materials.Length; index++)
        {
            Material material = mesh.materials[index];
            
            // Switch to transparent
            Material transparent = MaterialSwitchers.Find(m => (m.Opaque.name + " (Instance)").Equals(material.name)).Transparent;
            transparent.SetFloat("_Mode", 2);
            transparent.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            transparent.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            transparent.SetInt("_ZWrite", 0);
            transparent.DisableKeyword("_ALPHATEST_ON");
            transparent.EnableKeyword("_ALPHABLEND_ON");
            transparent.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            transparent.renderQueue = 3000;
            
            materialsCopy[index] = transparent;
        }

        mesh.materials = materialsCopy;
    }
    
    [Serializable]
    private struct MaterialSwitcher
    {
        public Material Opaque;
        public Material Transparent;
    }
}
