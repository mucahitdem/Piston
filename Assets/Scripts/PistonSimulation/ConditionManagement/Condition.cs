using System;
using PistonSimulation.PistonManagement;
using PistonSimulation.ReplacingManagement;

namespace PistonSimulation.ConditionManagement
{
    [Serializable]
    public class Condition
    {
        public PlaceToReplace place;
        public bool isFull;

        public bool IsConditionOkay()
        {
            return place.IsFull == isFull;
        }
    }
}