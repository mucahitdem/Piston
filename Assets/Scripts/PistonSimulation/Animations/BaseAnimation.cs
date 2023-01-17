using System;
using DG.Tweening;
using UnityEngine;

namespace PistonSimulation.Animations
{
    public abstract class BaseAnimation : MonoBehaviour
    {
        private Transform _transformObj;

        public Transform TransformObj
        {
            get
            {
                if (!_transformObj)
                    _transformObj = transform;

                return _transformObj;
            }
            set => _transformObj = value;
        }

        public abstract void Animate(Transform target , Action onComplete);
    }
}