using System;
using DG.Tweening;
using UnityEngine;

namespace PistonSimulation.Animations
{
    public class RodBearingRodSide : BaseAnimation
    {
        public override void Animate(Transform target, Action onComplete)
        {
            TransformObj.DORotate(target.eulerAngles, .5f);
            TransformObj.DOMove(target.position + target.forward * .5f, .5f)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    TransformObj.DOMove(target.position, .5f)
                        .SetEase(Ease.InSine)
                        .OnComplete(() =>
                        {
                            onComplete?.Invoke();
                        });
                });       
        }
    }
}