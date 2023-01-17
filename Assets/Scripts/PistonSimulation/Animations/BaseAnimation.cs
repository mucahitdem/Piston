using System;
using DG.Tweening;
using UnityEngine;

namespace PistonSimulation.Animations
{
    public abstract class BaseAnimation : MonoBehaviour
    {
        public abstract void Animate(Transform target , Action onComplete);
    }
}