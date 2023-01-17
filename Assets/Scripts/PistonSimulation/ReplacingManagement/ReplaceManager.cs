using System;
using System.Collections.Generic;
using PistonSimulation.PistonManagement;
using UnityEngine;

namespace PistonSimulation.ReplacingManagement
{
    public class ReplaceManager : MonoBehaviour
    {
        public Action onReplacedOwRemovedItem;
        public Action<bool> onAllPiecesReplaced;
        
        [SerializeField]
        private List<PlaceToReplace> placeToReplaces = new List<PlaceToReplace>();

        public Color colorWhenAboutToReplace; // green because it help user that it is correct place
        public Color colorWhenItemGrabbed; // grey preferly to declare postiion

        private PlaceToReplace _tempPlace;
        
        private void OnEnable()
        {
            GameManager.Instance.onPieceGrabbed += OnItemSelected;
            GameManager.Instance.onPieceReleased += OnItemReleased;
            onReplacedOwRemovedItem += CheckIfGameEnded;
        }

        private void OnDisable()
        {
            if (!GameManager.Instance)
            {
                GameManager.Instance.onPieceGrabbed -= OnItemSelected;
                GameManager.Instance.onPieceReleased -= OnItemReleased;
            }

            onReplacedOwRemovedItem -= CheckIfGameEnded;
        }

        private void OnItemSelected(BasePistonPiece pistonPiece)
        {
            if (pistonPiece.placeToReplace)
            {
                pistonPiece.placeToReplace.OnProperItemSelected();
                return;
            }
            
            foreach (var place in placeToReplaces)
            {
                if (!place.IsFull && place.layer == pistonPiece.pieceSo.pistonPieceData.layer)
                {
                    _tempPlace = place;
                    _tempPlace.OnProperItemSelected();
                    break;
                }
            }   
        }
        
        private void OnItemReleased(BasePistonPiece pistonPiece)
        {
            if(_tempPlace)
                _tempPlace.OnProperItemReleased();
            else if(pistonPiece.placeToReplace)
                pistonPiece.placeToReplace.OnProperItemReleased();
            _tempPlace = null;
        }

        private void CheckIfGameEnded()
        {
            foreach (var place in placeToReplaces)
            {
                if (!place.IsFull)
                {
                    onAllPiecesReplaced?.Invoke(false);
                    return;
                }
            }  
            
            onAllPiecesReplaced?.Invoke(true);
        }
    }
}