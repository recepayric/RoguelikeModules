using System.Collections.Generic;
using Runtime.Data.UnityObject;
using UnityEngine;

namespace Runtime.Model
{
    public class StatModel : IStatModel
    {
        private RD_StatConversions _statConversions;
        private RD_StatNames _statNames;
        
        private Dictionary<GameObject, EntityStatsVO> _stats = new Dictionary<GameObject, EntityStatsVO>();
        public Dictionary<GameObject, EntityStatsVO> Stats  => _stats;
        
        
        [PostConstruct]
        public void OnPostConstruct()
        {
            _statConversions = Resources.Load<RD_StatConversions>("Data/StatConversions");
            _statNames = Resources.Load<RD_StatNames>("Data/StatNames");
        }
        
        public bool RegisterStats(GameObject gameObject, EntityStatsVO stats)
        {
            if (!_stats.ContainsKey(gameObject))
            {
                _stats.Add(gameObject, stats);
                return true;
            }
            return false;
        }

        public bool RemoveStats(GameObject gameObject)
        {
            if (_stats.ContainsKey(gameObject))
            {
                _stats.Remove(gameObject);
                return true;
            }
            return false;
        }

        public EntityStatsVO GetStats(GameObject gameObject)
        {
            if (_stats.ContainsKey(gameObject))
                return _stats[gameObject];

            return null;
        }

        public RD_StatConversions StatConversions
        {
            get
            {
                if (_statConversions == null)
                    OnPostConstruct();
                return _statConversions;
            }
        }
        
        public RD_StatNames StatNames
        {
            get
            {
                if (_statNames == null)
                    OnPostConstruct();
                return _statNames;
            }
        }
    }
}