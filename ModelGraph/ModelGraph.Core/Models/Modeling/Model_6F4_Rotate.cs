
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class Model_6F4_Rotate : ItemModelOf<DrawModel>
    {
        private readonly Angles _angles;
        internal Model_6F4_Rotate(ItemModel owner, DrawModel item, Angles angles) : base(owner, item) { _angles = angles; }
        internal override IdKey IdKey => IdKey.Model_6F4_Rotate;
        public override string GetNameId() => Root.GetNameId(IdKey);
        public override void GetButtonCommands(Root root, List<ItemCommand> list)
        {
            list.Clear();
            if ((_angles & Angles.L90) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate90LeftCommand, () => { Item.RotateLeft(Item.ToAngle(90)); }));
            if ((_angles & Angles.L45) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate45LeftCommand, () => { Item.RotateLeft(Item.ToAngle(45)); }));
            if ((_angles & Angles.L30) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate30LeftCommand, () => { Item.RotateLeft(Item.ToAngle(30)); }));
            if ((_angles & Angles.L22) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate22LeftCommand, () => { Item.RotateLeft(Item.ToAngle(22.5f)); }));
            if ((_angles & Angles.R22) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate22RightCommand, () => { Item.RotateRight(Item.ToAngle(22.5f)); }));
            if ((_angles & Angles.R30) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate30RightCommand, () => { Item.RotateRight(Item.ToAngle(30)); }));
            if ((_angles & Angles.R45) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate45RightCommand, () => { Item.RotateRight(Item.ToAngle(45)); }));
            if ((_angles & Angles.R90) != 0)
                list.Add(new DrawCommand(this, IdKey.Rotate90RightCommand, () => { Item.RotateRight(Item.ToAngle(90)); }));
        }
    }
}
