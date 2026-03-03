using UnityEngine;

namespace AnkleBreaker.Utils.Extensions
{
    public static class RendererExtensions
    {
        public static void DisableAllShadows(this GameObject obj)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
                renderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }
}
