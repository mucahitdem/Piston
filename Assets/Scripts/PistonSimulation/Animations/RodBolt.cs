using System;
using DG.Tweening;
using UnityEngine;

namespace PistonSimulation.Animations
{
    public class RodBolt : BaseAnimation
    {
        [SerializeField]
        private float rotateVal;

        [SerializeField]
        private Camera cam;
        
        public override void Animate(Transform target, Action onComplete)
        {
            CameraAnim();
            RodBoltAnim(target, onComplete);
        }

        private void RodBoltAnim(Transform target, Action onComplete)
        {
            TransformObj.DORotate(target.eulerAngles, .5f);
            TransformObj.DOMove(target.position - target.forward * .5f, .5f)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    TransformObj.DOMove(target.position - target.forward * .15f, .5f)
                        .SetEase(Ease.InSine)
                        .OnComplete(() =>
                        {
                            TransformObj.DORotate(Vector3.forward * rotateVal, 2, RotateMode.LocalAxisAdd).SetRelative(true)
                                .SetEase(Ease.Linear);
                            TransformObj.DOMove(target.position, 2f)
                                .SetEase(Ease.InSine)
                                .OnComplete(() => { onComplete?.Invoke(); });
                        });
                });
        }

        private void CameraAnim()
        {
            cam.DOFieldOfView(40, .75f).SetEase(Ease.OutSine)
                .OnComplete(() => { DOVirtual.DelayedCall(2f, () => { cam.DOFieldOfView(85, .75f).SetEase(Ease.OutSine); }); });
        }
    }
}