using System.Collections.Generic;

namespace ModelGraph.Core
{
    /// <summary>Continous circular buffer</summary>
    internal class ModelBuffer
    {
        private LineModel[] _buffer;        // continous circular buffer
        private LineModel _target;          // target model
        private LineModel _ending;          // ending model
        private LineModel _starting;        // starting model

        private int _size;                  // actual size of buffer
        private int _uiSize;                // max posible number of visible UI rows 
        private int _count;                 // number of items added to the buffer since Initialize
        private int _overshoot;             // keep adding items, overshooting the target by half the buffer size
        private bool _checkingTarget;       // expecting and checking for the target item to be added
        private bool _foundTarget;          // detected the target being added to the buffer

        internal bool IsEmpty => _count == 0;

        #region Constructor  ==================================================
        internal ModelBuffer(int uiSize)
        {
            SizeCheck(uiSize);
        }
        #endregion

        #region Refresh  ======================================================
        internal void Refresh(LineModel root, int uiSize, LineModel target)
        {
            _count = 0;
            _target = target;
            _foundTarget = false;
            _checkingTarget = (_target != null);
            _starting = root.Count > 0 ? root.Items[0] : null;

            SizeCheck(uiSize);
            if (!root.BufferTraverse(this))
                _ending = (_count > 0) ? _buffer[(_count - 1) % _size] : default;
        }
        internal void Refresh(LineModel root, int uiSize, int offset)
        {
            var (list, indexT, end) = GetNormalized();
            var index = indexT + offset;

            if (index < 0) 
                Refresh(root, uiSize, list[0]);            
            else if (index + _uiSize > end && list[end] != _ending) 
                Refresh(root, uiSize, list[end]);
            else if (index < end)
                _target = list[index];  // set the new target model
        }

        private void SizeCheck(int uiSize)
        {
            _uiSize = uiSize;
            var minSize = 3 * uiSize;
            if (minSize > _size)
            {
                _size = minSize;
                _buffer = new LineModel[minSize];
            }
        }
        #endregion

        #region GetNormalized  ================================================
        private (List<LineModel>, int, int) GetNormalized()
        {
            int start, count, indexT = 0;
            _noralized.Clear();
            if (_count > _size)
            {
                start = _count % _size;
                count = _size;
            }
            else
            {
                start = 0;
                count = _count;
            }

            for (int i = 0, j = start; i < count; i++, j++)
            {
                var m = _buffer[j % _size];
                if (m == _target) indexT = i;
                _noralized.Add(m);
                if (m == _ending) break;
            }
            var end = count - 1;

            return (_noralized, indexT, end);
        }
        private readonly List<LineModel> _noralized = new List<LineModel>(60);
        #endregion

        #region AddItem  ======================================================
        /// <summary>Add item to  buffer, return true: if hit target</summary>
        internal bool AddItem(LineModel item)
        {
            var index = _count++ % _size;
            _buffer[index] = item;

            if (_foundTarget)
            {
                return (_overshoot-- < 0 && _count >= _size);      // end point triggers

            }
            else if (_checkingTarget)
            {
                if (_target.Equals(item))
                {
                    _foundTarget = true;
                    _checkingTarget = false;
                    _overshoot = 2 * _uiSize;
                }
                return false;
            }

            return (_count > _size);    // could not find the target, so abort because the buffer is full
        }
        #endregion

        #region GetList  ======================================================
        internal (List<LineModel>, bool, bool) GetList()
        {
            var (list, uiStart, end) = GetNormalized();

            var uiEnd = uiStart + _uiSize - 1;
            if (uiEnd > end) uiEnd = end;
            var uiCount = uiEnd - uiStart + 1;

            return uiCount == 0 ? (list, true, true) : (list.GetRange(uiStart, uiCount), list[uiEnd] == _ending, list[uiStart] == _starting);
        }
        #endregion
    }
}
