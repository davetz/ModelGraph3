
namespace ModelGraph.Core
{
    public class Model_627_ErrorText : ItemModelOf<Error>
    {
        private int _index;
        internal Model_627_ErrorText(Model_626_ErrorType owner, Error item, int index) : base(owner, item) { _index = index; }
        internal override IdKey IdKey => IdKey.Model_627_ErrorText;

        public override string GetNameId() => Item.GetError(_index);
    }
}
