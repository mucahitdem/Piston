using System;
using PistonSimulation.PistonManagement;

namespace PistonSimulation.ConditionManagement
{
    [Serializable]
    public class Condition
    {
        public PistonPieceSo piece;
        public bool isMounted;

        public bool IsConditionOkay()
        {
            return piece.pistonPieceData.isMounted == isMounted;
        }
    }
}