using System.Collections.Generic;
using Runtime.Data.UnityObject;
using UnityEngine;

namespace Runtime.Model
{
    public interface IStatModel
    {
        public RD_StatConversions StatConversions { get; }
        public RD_StatNames StatNames { get; }
        public Dictionary<GameObject, EntityStatsVO> Stats { get;  }
        bool RegisterStats(GameObject gameObject, EntityStatsVO stats);
        bool RemoveStats(GameObject gameObject);
        EntityStatsVO GetStats(GameObject gameObject);
    }
}