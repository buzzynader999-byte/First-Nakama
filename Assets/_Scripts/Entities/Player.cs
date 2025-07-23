using UnityEngine;

namespace _Scripts.Entities
{
    public class Player : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        public void ChangeColor(bool isLocal)
        {
            spriteRenderer.color = isLocal ? Color.gray8 : Color.limeGreen;
        }
    }
}