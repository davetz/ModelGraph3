
using System.Collections.Generic;

namespace ModelGraph.Core
{/*
 */
    public class Level : Item
    {
        internal List<Path> Paths = new List<Path>();
        internal override IdKey IdKey => IdKey.Level;

        #region Constructor  ==================================================
        internal Level(Graph owner)
        {
            Owner = owner;

            owner.Add(this);
        }
        #endregion

        #region Properties/Methods  ===========================================
        internal int Count => Paths.Count;
        internal void Add(Path path) { Paths.Add(path); }
        internal Graph Graph { get { return Owner as Graph; } }
        public override string GetNameId(Root root) => Graph.Levels.IndexOf(this).ToString();
        #endregion
    }
}
