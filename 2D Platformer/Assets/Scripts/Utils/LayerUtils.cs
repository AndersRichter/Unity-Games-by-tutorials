using UnityEngine;

namespace Utils
{
    public static class LayerUtils
    {
        public static bool IsCollisionWithLayer(GameObject gameObject, LayerMask layer)
        {
            // LayerMask is a bit mask of real layer number, so we need to use bit operations
            return layer == (layer | 1 << gameObject.layer);
        }
    }
}