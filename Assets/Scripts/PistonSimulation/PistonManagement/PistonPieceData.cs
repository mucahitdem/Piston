using System;
using System.Collections.Generic;
using PistonSimulation.ConditionManagement;
using UnityEngine;

namespace PistonSimulation.PistonManagement
{
    [Serializable]
    public class PistonPieceData
    {
        public LayerMask layer;
        public List<Condition> conditions = new List<Condition>();
        public bool isMounted;
    }
}