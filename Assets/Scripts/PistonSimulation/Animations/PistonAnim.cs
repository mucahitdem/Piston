using System;
using DG.Tweening;
using UnityEngine;

namespace PistonSimulation.Animations
{
    public class PistonAnim : BaseAnimation
    {
        public override void Animate(Transform target , Action onComplete)
        {
            transform.DORotate(target.eulerAngles, .5f);
            transform.DOMove(target.position + target.right * .5f, .5f)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    transform.DOMove(target.position, .5f)
                        .SetEase(Ease.InSine)
                        .OnComplete(() =>
                        {
                            onComplete?.Invoke();
                        });
                });

        }
    }
}