using System;
using System.Collections.Generic;
using DG.Tweening;
using PistonSimulation.Animations;
using PistonSimulation.ConditionManagement;
using PistonSimulation.ReplacingManagement;
using UnityEngine;

namespace PistonSimulation.PistonManagement
{
    public class BasePistonPiece : MonoBehaviour
    {
        public PistonPieceSo pieceSo;
        
        [SerializeField]
        private List<Condition> conditions = new List<Condition>();
        
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
        private string _uniqueId;
        private PlaceToReplace _placeToReplace;
        
        private void Awake()
        {
            _uniqueId = gameObject.GetInstanceID().ToString();
        }

        public void OnPieceReleased(PlaceToReplace target)
        {
            if (AllConditionsAreDone())
            {
                GameManager.Instance.isAnimating = true;

                _placeToReplace = target;
                
                SetIsMounted(true);
                RigidBodyControl(RigidbodyConstraints.FreezeAll);
                ColTriggerControl(true);

                target.IsFull = true;
                var transform1 = target.transform;
                TransformObj.parent = transform1.parent;
                
                baseAnimation.Animate(transform1,
                    () =>
                    {
                        GameManager.Instance.isAnimating = false;
                    });
            }
            else
            {
                GameManager.Instance.onPieceReleased(this);
                RigidBodyControl(RigidbodyConstraints.None);
            }
        }

        private void OnMouseDown()
        {
            if (IsMounted()) // todo fix
            {
                SetIsMounted(false);
                ColTriggerControl(false);
                _placeToReplace.IsFull = false;
                Debug.LogError(_placeToReplace.transform.name);
                TransformObj.parent = null;
            }

            
            GameManager.Instance.onPieceGrabbed?.Invoke(this);
            RbOfObj.constraints = RigidbodyConstraints.FreezeRotation;
            TransformObj.DORotate(holdRotation, .5f);
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

        private void OnMouseUp()
        {
            RigidBodyControl(RigidbodyConstraints.None);
        }

        private bool AllConditionsAreDone()
        {
            foreach (var condition in  conditions)
            {
                if (!condition.IsConditionOkay())
                {
                    return false;
                }
            }
            
            return true;
        }

        private void SetIsMounted(bool isMounted)
        {
            PlayerPrefs.SetInt( _uniqueId, isMounted ? 1 : 0);
        }
        
        private bool IsMounted()
        {
            return PlayerPrefs.GetInt(_uniqueId, 0) == 1;
        }

        private void ColTriggerControl(bool isTrigger)
        {
            Col.isTrigger = isTrigger;
        }

        private void RigidBodyControl(RigidbodyConstraints constraints)
        {
            RbOfObj.constraints = constraints;
        }
    }
}