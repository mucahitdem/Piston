using System;
using Core;
using DG.Tweening;
using PistonSimulation.Animations;
using UnityEngine;

namespace PistonSimulation.PistonManagement
{
    public class BasePistonPiece : MonoBehaviour
    {
        public PistonPieceSo pieceSo;
        
        [SerializeField]
        private Vector3 holdRotation;

        [SerializeField]
        private BaseAnimation baseAnimation;
        
        
        private Transform _transformObj;
        private Transform TransformObj
        {
            get
            {
                if (!_transformObj)
                    _transformObj = transform;
                
                return _transformObj;
            }
            set => _transformObj = value;
        }

        
        private Rigidbody _rbOfObj;
        private Rigidbody RbOfObj
        {
            get
            {
                if (!_rbOfObj)
                    _rbOfObj = GetComponent<Rigidbody>();
                
                return _rbOfObj;
            }
            set => _rbOfObj = value;
        }

        private Collider _col;

        private Collider Col
        {
            get
            {
                if (!_col)
                    _col = GetComponentInChildren<Collider>();

                return _col;
            }
        }

        private Camera _camera;
        private Camera Camera
        {
            get
            {
                if (!_camera)
                    _camera = GameManager.Instance.Cam;

                return _camera;
            }
        }


        private float _distToScreen;
        
        public void OnPieceReleased(Transform target)
        {
            if (AllConditionsAreDone())
            {
                RbOfObj.constraints = RigidbodyConstraints.FreezeAll;
                pieceSo.pistonPieceData.isMounted = true;
                Col.enabled = false;
                
                baseAnimation.Animate(target, () => target.gameObject.SetActive(false));

                // transform.DOMove(target.position, .5f);
                // transform.DORotate(target.eulerAngles, .5f)
                //     .OnComplete(() =>
                //     {
                //         target.gameObject.SetActive(false);
                //     });
            }
            else
            {
                RbOfObj.constraints = RigidbodyConstraints.None;
            }
        }

        private void OnMouseDown()
        {
            if (pieceSo.pistonPieceData.isMounted)
            {
                pieceSo.pistonPieceData.isMounted = false;
                Col.enabled = false;
                
                RbOfObj.constraints = RigidbodyConstraints.FreezeRotation;
                TransformObj.DORotate(holdRotation, .5f);
            }
            else
            {
                RbOfObj.constraints = RigidbodyConstraints.FreezeRotation;
                TransformObj.DORotate(holdRotation, .5f);
            }
        }

        private void OnMouseDrag()
        {
            RbOfObj.velocity = Vector3.zero;
            _distToScreen = Camera.WorldToScreenPoint(gameObject.transform.position).z;
            Vector3 pos = Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distToScreen));
            
            transform.position = new Vector3(Mathf.Clamp(pos.x, Defs.XVal.MinVal, Defs.XVal.MaxVal), 
                                                    Mathf.Clamp(pos.y, Defs.YVal.MinVal, Defs.YVal.MaxVal),
                                                            Defs.FIXED_Z_POS);
        }

        private bool AllConditionsAreDone()
        {
            foreach (var condition in  pieceSo.pistonPieceData.conditions)
            {
                if (!condition.IsConditionOkay())
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}