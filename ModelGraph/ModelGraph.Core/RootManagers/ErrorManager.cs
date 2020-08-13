﻿using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ErrorManager : ChildOfStoreOf<Root, Error>
    {
        #region Constructors  =================================================
        internal ErrorManager(Root root)
        {
            Owner = root;
            SetCapacity(20);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.ErrorManager;
        #endregion

        #region ErrorMethods  =================================================

        private readonly Dictionary<Item, Error> _itemError = new Dictionary<Item, Error>(); //the most serious item error
        private readonly Dictionary<(Item, Item), Error> _itemErrorAux1 = new Dictionary<(Item, Item), Error>(); //the most serious item error
        private readonly Dictionary<(Item, Item, Item), Error> _itemErrorAux2 = new Dictionary<(Item, Item, Item), Error>(); //the most serious item error

        #region RepositoryError  ==============================================
        public void AddRepositorReadError(string text)
        {
            //_itemError[ImportBinaryReader] = new ErrorOne(ErrorStore, this, IdKey.ImportError, text);
        }
        public void AddRepositorWriteError(string text)
        {
            //_itemError[ExportBinaryWriter] = new ErrorOne(ErrorStore, this, IdKey.ExportError, text);
        }
        #endregion

        #region ClearItemErrors  ==============================================
        void ClearItemErrors()
        {
            _itemError.Clear();
        }
        #endregion

        #region AddError  =====================================================
        private ErrorNone AddError(Item item, ErrorNone error)
        {
            _itemError[item] = error;
            item.ErrorDelta++;
            return error;
        }
        private ErrorOne AddError(Item item, ErrorOne error)
        {
            _itemError[item] = error;
            item.ErrorDelta++;
            return error;
        }
        private ErrorMany AddError(Item item, ErrorMany error)
        {
            _itemError[item] = error;
            item.ErrorDelta++;
            return error;
        }

        private ErrorNoneAux AddError(Item item, Item aux1, ErrorNoneAux error)
        {
            _itemErrorAux1[(item, aux1)] = error;
            item.ErrorDelta++;
            return error;
        }
        private ErrorOneAux AddError(Item item, Item aux1, ErrorOneAux error)
        {
            _itemErrorAux1[(item, aux1)] = error;
            item.ErrorDelta++;
            return error;
        }
        private ErrorManyAux AddError(Item item, Item aux1, ErrorManyAux error)
        {
            _itemErrorAux1[(item, aux1)] = error;
            item.ErrorDelta++;
            return error;
        }

        private ErrorNoneAux2 AddError(Item item, Item aux1, Item aux2, ErrorNoneAux2 error)
        {
            _itemErrorAux2[(item, aux1, aux2)] = error;
            item.ErrorDelta++;
            return error;
        }
        private ErrorOneAux2 AddError(Item item, Item aux1, Item aux2, ErrorOneAux2 error)
        {
            _itemErrorAux2[(item, aux1, aux2)] = error;
            item.ErrorDelta++;
            return error;
        }
        private ErrorManyAux2 AddError(Item item, Item aux1, Item aux2, ErrorManyAux2 error)
        {
            _itemErrorAux2[(item, aux1, aux2)] = error;
            item.ErrorDelta++;
            return error;
        }
        #endregion

        #region ClearError  ===================================================.
        internal void ClearError(Item item)
        {
            if (_itemError.TryGetValue(item, out Error error))
                ClearError(item, error);
        }
        internal void ClearError(Item item, Item aux1)
        {
            if (_itemErrorAux1.TryGetValue((item, aux1), out Error error))
                ClearError(item, aux1, error);
        }
        internal void ClearError(Item item, Item aux1, Item aux2)
        {
            if (_itemErrorAux2.TryGetValue((item, aux1, aux2), out Error error))
                ClearError(item, aux1, aux2, error);
        }

        internal void ClearError(Item item, IdKey idKe)
        {
            if (_itemError.TryGetValue(item, out Error error) && error.ErrorId == idKe)
                ClearError(item, error);
        }
        internal void ClearError(Item item, Item aux1, IdKey idKe)
        {
            if (_itemErrorAux1.TryGetValue((item, aux1), out Error error) && error.ErrorId == idKe)
                ClearError(item, aux1, error);
        }
        internal void ClearError(Item item, Item aux1, Item aux2, IdKey idKe)
        {
            if (_itemErrorAux2.TryGetValue((item, aux1, aux2), out Error error) && error.ErrorId == idKe)
                ClearError(item, aux1, aux2, error);
        }

        internal void ClearError(Item item, Error error)
        {
            if (error.IsErrorAux) throw new System.Exception("Corrupt Error Hierarcy");
            Remove(error);
            _itemError.Remove(item);
            item.ErrorDelta++;
        }
        internal void ClearError(Item item, Item aux1, Error error)
        {
            if (!error.IsErrorAux1) throw new System.Exception("Corrupt Error Hierarcy");
            Remove(error);
            _itemErrorAux1.Remove((item, aux1));
            item.ErrorDelta++;
        }
        internal void ClearError(Item item, Item aux1, Item aux2, Error error)
        {
            if (!error.IsErrorAux1) throw new System.Exception("Corrupt Error Hierarcy");
            Remove(error);
            _itemErrorAux2.Remove((item, aux1, aux2));
            item.ErrorDelta++;
        }

        internal void ClearErrors(HashSet<IdKey> traits)
        {
            var removeError = new Dictionary<Item, Error>();
            var removeErrorAux1 = new Dictionary<(Item, Item), Error>();
            var removeErrorAux2 = new Dictionary<(Item, Item, Item), Error>();

            foreach (var e in _itemError)
            {
                if (traits.Contains(e.Value.ErrorId)) removeError.Add(e.Key, e.Value);
            }
            foreach (var e in _itemErrorAux1)
            {
                if (traits.Contains(e.Value.ErrorId)) removeErrorAux1.Add(e.Key, e.Value);
            }
            foreach (var e in _itemErrorAux2)
            {
                if (traits.Contains(e.Value.ErrorId)) removeErrorAux2.Add(e.Key, e.Value);
            }
            foreach (var e in removeError)
            {
                _itemError.Remove(e.Key);
                Remove(e.Value);
                e.Value.Item.ErrorDelta++;
            }
            foreach (var e in removeErrorAux1)
            {
                _itemErrorAux1.Remove(e.Key);
                Remove(e.Value);
                e.Value.Item.ErrorDelta++;
            }
            foreach (var e in removeErrorAux2)
            {
                _itemErrorAux2.Remove(e.Key);
                Remove(e.Value);
                e.Value.Item.ErrorDelta++;
            }
        }
        #endregion

        #region TryAddError  ==================================================
        internal ErrorNone TryAddErrorNone(Item item, IdKey idKe)
        {
            var prevError = TryGetError(item);
            if (prevError != null)
            {
                if (prevError is ErrorNone error && error.ErrorId == idKe)
                    return error; // this error already exists

                //if (prevError.ItemIndex > TraitIndexOf(idKe))
                //    return null; // prevError has hight traitIndex and will not be replace

                ClearError(item, prevError);
            }
            return AddError(item, new ErrorNone(this, item, idKe));
        }
        internal ErrorNoneAux TryAddErrorNone(Item item, Item aux1, IdKey idKe)
        {
            var prevError = TryGetError(item, aux1);
            if (prevError != null)
            {
                if (prevError is ErrorNoneAux error && error.ErrorId == idKe)
                    return error; // this error already exists

                //if (prevError.ItemIndex > TraitIndexOf(idKe))
                //    return null; // prevError has hight traitIndex and will not be replace

                ClearError(item, aux1, prevError);
            }
            return AddError(item, aux1, new ErrorNoneAux(this, item, aux1, idKe));
        }
        internal ErrorNoneAux2 TryAddErrorNone(Item item, Item aux1, Item aux2, IdKey idKe)
        {
            var prevError = TryGetError(item, aux1, aux2);
            if (prevError != null)
            {
                if (prevError is ErrorNoneAux2 error && error.ErrorId == idKe)
                    return error; // this error already exists

                //if (prevError.ItemIndex > TraitIndexOf(idKe))
                //    return null; // prevError has hight traitIndex and will not be replace

                ClearError(item, aux1, aux2, prevError);
            }
            return AddError(item, aux1, aux2, new ErrorNoneAux2(this, item, aux1, aux2, idKe));
        }

        internal ErrorOne TryAddErrorOne(Item item, IdKey idKe, string text = null)
        {
            var prevError = TryGetError(item);
            if (prevError != null)
            {
                if (prevError is ErrorOne error && error.ErrorId == idKe)
                    return error; // this error already exists

                //if (prevError.ItemIndex > TraitIndexOf(idKe))
                //    return null; // prevError has hight traitIndex and will not be replace

                ClearError(item, prevError);
            }
            return AddError(item, new ErrorOne(this, item, idKe, text));
        }
        internal ErrorOneAux TryAddErrorOne(Item item, Item aux1, IdKey idKe, string text = null)
        {
            var prevError = TryGetError(item, aux1);
            if (prevError != null)
            {
                if (prevError is ErrorOneAux error && error.ErrorId == idKe)
                    return error; // this error already exists

                //if (prevError.ItemIndex > TraitIndexOf(idKe))
                //    return null; // prevError has hight traitIndex and will not be replace

                ClearError(item, aux1, prevError);
            }
            return AddError(item, aux1, new ErrorOneAux(this, item, aux1, idKe, text));
        }
        internal ErrorOneAux2 TryAddErrorOne(Item item, Item aux1, Item aux2, IdKey idKe, string text = null)
        {
            var prevError = TryGetError(item, aux1, aux2);
            if (prevError != null)
            {
                if (prevError is ErrorOneAux2 error && error.ErrorId == idKe)
                    return error; // this error already exists

                //if (prevError.ItemIndex > TraitIndexOf(idKe))
                //    return null; // prevError has hight traitIndex and will not be replace

                ClearError(item, aux1, aux2, prevError);
            }
            return AddError(item, aux1, aux2, new ErrorOneAux2(this, item, aux1, aux2, idKe, text));
        }

        internal ErrorMany TryAddErrorMany(Item item, IdKey idKe, string text = null)
        {
            var prevError = TryGetError(item);
            if (prevError != null)
            {
                if (prevError is ErrorMany error && error.ErrorId == idKe)
                    return error; // this error already exists

                //if (prevError.ItemIndex > TraitIndexOf(idKe))
                //    return null; // prevError has hight traitIndex and will not be replace

                ClearError(item, prevError);
            }
            return AddError(item, new ErrorMany(this, item, idKe, text));
        }
        internal ErrorManyAux TryAddErrorMany(Item item, Item aux1, IdKey idKe, string text = null)
        {
            var prevError = TryGetError(item, aux1);
            if (prevError != null)
            {
                if (prevError is ErrorManyAux error && error.ErrorId == idKe)
                    return error; // this error already exists

                //if (prevError.ItemIndex > TraitIndexOf(idKe))
                //    return null; // prevError has hight traitIndex and will not be replace

                ClearError(item, aux1, prevError);
            }
            return AddError(item, aux1, new ErrorManyAux(this, item, aux1, idKe, text));
        }
        internal ErrorManyAux2 TryAddErrorMany(Item item, Item aux1, Item aux2, IdKey idKe, string text = null)
        {
            var prevError = TryGetError(item, aux1, aux2);
            if (prevError != null)
            {
                if (prevError is ErrorManyAux2 error && error.ErrorId == idKe)
                    return error; // this error already exists

                //if (prevError.ItemIndex > TraitIndexOf(idKe))
                //    return null; // prevError has hight traitIndex and will not be replace

                ClearError(item, aux1, aux2, prevError);
            }
            return AddError(item, aux1, aux2, new ErrorManyAux2(this, item, aux1, aux2, idKe, text));
        }
        #endregion

        #region TryGetError  ==================================================
        internal Error TryGetError(Item item)
        {
            return (_itemError.TryGetValue(item, out Error error)) ? error : null;
        }
        internal Error TryGetError(Item item, Item aux1)
        {
            return (_itemErrorAux1.TryGetValue((item, aux1), out Error error)) ? error : null;
        }
        internal Error TryGetError(Item item, Item aux1, Item aux2)
        {
            return (_itemErrorAux2.TryGetValue((item, aux1, aux2), out Error error)) ? error : null;
        }
        #endregion

        #endregion
    }
}
