using System.Collections;
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
        public PlaceToReplace placeToReplace;
        
        [SerializeField]
        private List<Condition> mountConditions = new List<Condition>();
        
        [SerializeField]
        private List<Condition> dismountConditions = new List<Condition>();

        
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
        
        
        private void Awake()
        {
            _uniqueId = gameObject.GetInstanceID().ToString();
        }

        private void OnEnable()
        {
            GameManager.Instance.onPieceMounted += OnPieceOnMounted;
            GameManager.Instance.onPieceUnmounted += OnPieceOnUnmounted;
        }

        private void OnPieceOnMounted(BasePistonPiece unmountedPiece)
        {
            StartCoroutine(UpdateCollider());
        }
        private void OnPieceOnUnmounted(BasePistonPiece mountedPiece)
        {
            StartCoroutine(UpdateCollider());
        }

        private IEnumerator UpdateCollider()
        {
            yield return new WaitUntil(() => GameManager.Instance.isAnimating == false);           
            ColliderActivateController(DismountConditionsAreDone());
        }
        
        public void OnPieceReleased(PlaceToReplace target)
        {
            placeToReplace = target;
            
            if (MountConditionsAreDone())
            {
                GameManager.Instance.isAnimating = true;
                
                ColTriggerControl(true);
                ColliderActivateController(false);
                SetIsMounted(true);
                RigidBodyControl(RigidbodyConstraints.FreezeAll);
               
                
                GameManager.Instance.onPieceMounted?.Invoke(this);

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
            if (IsMounted())
            {
                if (DismountConditionsAreDone())
                {
                    SetIsMounted(false);
                    ColTriggerControl(false);
                    placeToReplace.IsFull = false;
                    TransformObj.parent = null;
                    
                    GameManager.Instance.onPieceUnmounted?.Invoke(this);
                }
                else
                {
                    return;
                }
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
        
        private bool MountConditionsAreDone()
        {
            foreach (var condition in  mountConditions)
            {
                if (!condition.IsConditionOkay())
                {
                    return false;
                }
            }
            
            return true;
        }
        private bool DismountConditionsAreDone()
        {
            foreach (var condition in dismountConditions)
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
        private void ColliderActivateController(bool isEnabled)
        {
            Col.enabled = isEnabled;
        }
        private void RigidBodyControl(RigidbodyConstraints constraints)
        {
            RbOfObj.constraints = constraints;
        }
    }
}