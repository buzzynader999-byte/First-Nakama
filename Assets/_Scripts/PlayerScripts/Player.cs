using UnityEngine;

namespace _Scripts.Entities
{
    public class Player : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Transform gunPlace;
        public Transform GunPlace => gunPlace;

        public void ChangeColor(bool isLocal)
        {
            spriteRenderer.color = isLocal ? Color.gray : Color.green;
        }
    }
}