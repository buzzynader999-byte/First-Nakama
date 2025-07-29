using System;
using UnityEngine;

namespace _Scripts.Entities
{
    public class Player : MonoBehaviour
    {
        [SerializeField] Transform gunPlace;
        public Transform GunPlace => gunPlace;
    }
}