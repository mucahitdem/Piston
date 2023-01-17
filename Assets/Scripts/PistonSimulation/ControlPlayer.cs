using System;
using Core.Control.ControlTypes;
using PistonSimulation.PistonManagement;
using PistonSimulation.ReplacingManagement;
using UnityEngine;

namespace PistonSimulation
{
    public class ControlPlayer : ControlSendRayToMousePosition
    {
        [SerializeField]
        private BasePistonPiece selectedPiece;

        private PlaceToReplace _tempPlaceToReplace;
        
        
        [SerializeField]
        private LayerMask defaultLayer;
        
        private void Awake()
        {
            ChangeLayer(defaultLayer.value);
        }

        private void OnEnable()
        {
            GameManager.Instance.onPieceGrabbed += OnItemSelected;
        }

        private void OnDisable()
        {
            if(GameManager.Instance)
                GameManager.Instance.onPieceGrabbed -= OnItemSelected;
        }
        

        protected override void OnTapUp()
        {
            base.OnTapUp();
            
            if (selectedPiece)
            {
                if (Hit.transform && Hit.transform.TryGetComponent(out _tempPlaceToReplace))
                {
                    selectedPiece.OnPieceReleased(_tempPlaceToReplace);
                    selectedPiece = null;
                    ChangeLayer(defaultLayer);
                }
                else
                {
                    GameManager.Instance.onPieceReleased?.Invoke(selectedPiece);
                    selectedPiece = null;
                }
            }
        }
        
        private void OnItemSelected(BasePistonPiece piece)
        {
            selectedPiece = piece;
            ChangeLayer(selectedPiece.pieceSo.pistonPieceData.layer);
        }
    }
}