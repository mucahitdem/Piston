using UnityEngine;

namespace PistonSimulation.PistonManagement
{
    [CreateAssetMenu(fileName = "Piston Piece", menuName = "Montage/Piston Piece", order = 0)]
    public class PistonPieceSo : ScriptableObject
    {
        public PistonPieceData pistonPieceData;
    }
}