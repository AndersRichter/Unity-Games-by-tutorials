using UnityEngine;

namespace Utils
{
    public class LayerUtils
    {
        public static bool IsCollisionWithLayer(Collision2D collision, LayerMask layer)
        {
            // LayerMask is a bit mask of real layer number, so we need to use bit operations
            return layer == (layer | 1 << collision.gameObject.layer);
        }
    }
}