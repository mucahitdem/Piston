using System;
using UnityEngine;

namespace Core.Control.ControlTypes
{
    public class ControlSendRayToMousePosition : BaseControl
    {
        [SerializeField]
        private Camera cam;
        
        protected RaycastHit Hit;
        
        private Ray _ray;
        private Vector3 _mousePos;
        private LayerMask _layer;
        
        protected override void OnTapDown()
        {
            base.OnTapDown();
            CastToMousePos(OnHitStart);
        }
        protected override void OnTapHold()
        {
            base.OnTapHold();
            CastToMousePos(OnHitAndHold);
        }
        private void CastToMousePos(Action onHit)
        { 
            UpdateMousePos();
            CalculateRay();
            Cast(onHit);
        }
        private void UpdateMousePos()
        {
            _mousePos = Input.mousePosition;
        }
        private void CalculateRay()
        {
            _ray = cam.ScreenPointToRay(_mousePos);
        }
        private void Cast(Action onHit)
        {
            if (Physics.Raycast(_ray, out Hit, Mathf.Infinity, _layer.value))
            {
                onHit?.Invoke();
            }
        }
        protected void ChangeLayer(int newLayer)
        {
            _layer.value = newLayer;
        }
        
        protected virtual void OnHitStart()
        {
        }
        protected virtual void OnHitAndHold()
        {
        }
    }
}