using UnityEngine;

namespace _Scripts.PlayerScripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] Transform gunPlace;
        public Transform GunPlace => gunPlace;
    }
}