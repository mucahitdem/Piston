using System;
using Core.Control.ControlTypes;
using PistonSimulation.PistonManagement;
using UnityEngine;

namespace PistonSimulation
{
    public class ControlPlayer : ControlSendRayToMousePosition
    {
        [SerializeField]
        private BasePistonPiece selectedPiece;

        [SerializeField]
        private LayerMask defaultLayer;
        
        private void Awake()
        {
            ChangeLayer(defaultLayer.value);
        }

        protected override void OnHitStart()
        {
            base.OnHitStart();
            
            Hit.transform.TryGetComponent(out selectedPiece);
            UpdateLayer();
        }
        
        protected override void OnTapUp()
        {
            base.OnTapUp();
            if (selectedPiece && Hit.transform)
            {
                selectedPiece.OnPieceReleased(Hit.transform);
                ChangeLayer(defaultLayer);
            }
                
        }
        
        private void UpdateLayer()
        {
            if (selectedPiece)
                ChangeLayer(selectedPiece.pieceSo.pistonPieceData.layer);
        }
    }
}