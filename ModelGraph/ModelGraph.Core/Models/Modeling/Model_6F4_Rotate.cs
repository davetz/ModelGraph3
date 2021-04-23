
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6F4_Rotate : ItemModelOf<Selector>
    {
        private readonly Angles _angles;
        internal Model_6F4_Rotate(ItemModel owner, Selector item, Angles angles) : base(owner, item) { _angles = angles; }
        internal override IdKey IdKey => IdKey.Model_6F4_Rotate;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            if ((_angles & Angles.L90) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate90LeftCommand, () => { Item.Rotate(-90); }));
            if ((_angles & Angles.L45) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate45LeftCommand, () => { Item.Rotate(-45); }));
            if ((_angles & Angles.L30) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate30LeftCommand, () => { Item.Rotate(-30); }));
            if ((_angles & Angles.L22) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate22LeftCommand, () => { Item.Rotate(-22.5f); }));
            if ((_angles & Angles.R22) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate22RightCommand, () => { Item.Rotate(22.5f); }));
            if ((_angles & Angles.R30) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate30RightCommand, () => { Item.Rotate(30); }));
            if ((_angles & Angles.R45) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate45RightCommand, () => { Item.Rotate(45); }));
            if ((_angles & Angles.R90) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate90RightCommand, () => { Item.Rotate(90); }));
        }
    }
}
