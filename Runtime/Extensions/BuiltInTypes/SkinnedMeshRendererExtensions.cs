using UnityEngine;

namespace AnkleBreaker.Utils.Extensions
{
    public static class SkinnedMeshRendererExtensions
    {
        public static void CopyComponent(this SkinnedMeshRenderer source, SkinnedMeshRenderer compToCopy)
        {
            source.sharedMesh = compToCopy.sharedMesh;
            source.updateWhenOffscreen = compToCopy.updateWhenOffscreen;
            source.materials = compToCopy.sharedMaterials;
            source.tag = compToCopy.tag;
        }
    }
}
