using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace PistonSimulation.ReplacingManagement
{
    public class PlaceToReplace : MonoBehaviour
    {
        public Vector3 pistonParentRotation;

        public LayerMask layer;

        [SerializeField]
        private Renderer rend;
        
        private bool _isFull;
        public bool IsFull
        {
            get => _isFull;
            set
            {
                if (value != _isFull)
                {
                    _isFull = value;
                    if(_isFull)
                        ColliderActivateController(false);
                    MeshActivateController(!_isFull);
                }
            }
        }

        private MaterialPropertyBlock _propertyBlock;
        private ReplaceManager _replaceManager;

        private Collider _col;
        
        private void Awake()
        {
            _replaceManager = GetComponentInParent<ReplaceManager>();
            rend = GetComponent<Renderer>();
            _col = GetComponentInChildren<Collider>();
            _propertyBlock = new MaterialPropertyBlock();
        }
        
        public void OnProperItemSelected()
        {
            StartCoroutine(OnProperItemSelect());
        }

        private IEnumerator OnProperItemSelect()
        {
            yield return new WaitUntil(() => GameManager.Instance.isAnimating == false);
            
            if (_replaceManager.transform.eulerAngles != pistonParentRotation)
            {
                _replaceManager.transform.DORotate(pistonParentRotation, 1f).SetEase(Ease.OutQuad);
            }
            
            ColliderActivateController(true);
            MeshActivateController(true);
            ChangeMeshColor(_replaceManager.colorWhenItemGrabbed);
        }
        
        public void OnProperItemReleased()
        {
            ColliderActivateController(false);
            MeshActivateController(false);;
        }

        public void ResetToDefault()
        {
            ColliderActivateController(false);
            MeshActivateController(false);
        }

        private void ColliderActivateController(bool isEnabled)
        {
            _col.enabled = isEnabled;
        }
        
        private void MeshActivateController(bool isEnabled)
        {
            rend.enabled = isEnabled;
        }

        private void ChangeMeshColor(Color newColor)
        {
            rend.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(Defs.SHADER_COLOR_KEY, newColor);
            rend.SetPropertyBlock(_propertyBlock);
        }
    }
}