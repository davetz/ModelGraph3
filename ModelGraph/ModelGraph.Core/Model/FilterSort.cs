using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ModelGraph.Core
{
    /// <summary>Provides filter and sort methods for line models</summary>
    internal class FilterSort
    {
        private static Dictionary<LineModel, FilterSort> _model_filter = new Dictionary<LineModel, FilterSort>();
        private string Filter => _filterText is null ? string.Empty : _filterText;
        private int Count;
        private Usage Usage = Usage.None;
        private Sorting Sorting = Sorting.Unsorted;
        private List<(int I, bool IN, string TX)> Selector;

        private Regex _filterRx;
        private string _filterText;
        private bool _filterChanged;
        private bool _sortChanged;
        private byte _delta;

        private FilterSort() { }

        #region Parms  ========================================================
        //private static readonly List<(int I, bool IN, string TX)> EmptySelector = new List<(int I, bool IN, string TX)>(0);
        internal static (int, Sorting, Usage, string) GetParms(LineModel m) => _model_filter.TryGetValue(m, out FilterSort f) ? (f.Count, f.Sorting, f.Usage, f.Filter) : (m.Count, Sorting.Unsorted, Usage.None, string.Empty);
        internal static bool TryGetSelector(LineModel m, out List<(int I, bool IN, string TX)> selector)
        {
            selector = _model_filter.TryGetValue(m, out FilterSort f) ? f.Selector : null;
            return selector != null;
        }

        internal static bool SetUsage(LineModel model, Usage usage)
        {
            if (_model_filter.TryGetValue(model, out FilterSort f))
            {
                if (f.Usage == usage) return false;
                f.SetUsage(usage);
                if (f.HasDefaultParms)
                {
                    ReleaseFilter(model);
                    return true;
                }
            }
            else
            {
                if (usage == Usage.None) return false;
                f = AllocateFilter(model);
                f.SetUsage(usage);
            }
            f.Refresh(model);
            return true;
        }

        internal static bool SetSorting(LineModel model, Sorting sorting)
        {
            if (_model_filter.TryGetValue(model, out FilterSort f))
            {
                if (f.Sorting == sorting) return false;
                f.SetSorting(sorting);
                if (f.HasDefaultParms)
                {
                    ReleaseFilter(model);
                    return true;
                }
            }
            else
            {
                if (sorting == Sorting.Unsorted) return false;
                f = AllocateFilter(model);
                f.SetSorting(sorting);
            }
            f.Refresh(model);
            return true;
        }

        internal static bool SetText(LineModel model, string text)
        {
            if (_model_filter.TryGetValue(model, out FilterSort f))
            {
                if (f.HasSameText(text)) return false;
                f.SetNewText(text);
                if (f.HasDefaultParms)
                {
                    ReleaseFilter(model);
                    return true;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(text)) return false;
                f = AllocateFilter(model);
                f.SetNewText(text);
            }
            f.Refresh(model);
            return true;
        }

        #region HelperMethods  ================================================
        private void SetNewText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                _filterText = null;
                _filterRx = null;
            }
            else
            {
                _filterText = text;
                _filterRx = text.Contains("*") ?
                    new Regex(text, RegexOptions.Compiled | RegexOptions.IgnoreCase) :
                    new Regex($".*{text}.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            _filterChanged = true;
        }
        private void SetUsage(Usage usage)
        {
            _filterChanged |= Usage != usage;
            Usage = usage;
        }
        private void SetSorting(Sorting sorting)
        {
            _sortChanged = Sorting != sorting;
            Sorting = sorting;
        }

        private bool HasDefaultParms => Usage == Usage.None && Sorting == Sorting.Unsorted && _filterText == null;

        private bool HasSameText(string a)
        {
            var b = _filterText;
            return N(a) ? N(b) : (!N(b) && E(a, b));

            bool N(string v) => string.IsNullOrWhiteSpace(v); //is NULL or blank
            bool E(string p, string q) => (string.Compare(p, q) == 0); //are EQUAL
        }
        private static FilterSort AllocateFilter(LineModel model)
        {
            model.HasFilterSortAllocation = true;
            var f = new FilterSort();
            _model_filter.Add(model, f);
            return f;
        }
        internal static void ReleaseFilter(LineModel model)
        {
            model.HasFilterSortAllocation = false;
            _model_filter.Remove(model);
        }
        #endregion
        #endregion

        #region Refresh  ======================================================
        internal void Refresh(LineModel model)
        {
            var root = model.DataRoot;
            if (_delta != model.ChildDelta || Selector is null || Selector.Count != model.Count)
            {
                #region need to build to new selector
                _delta = model.ChildDelta;
                _filterChanged = true;
                Selector = new List<(int I, bool IN, string ID)>(model.Count);
                for (int i = 0; i < model.Count; i++)
                {
                    Selector.Add((i, true, model.Items[i].GetFilterSortId(root)));
                }
                #endregion
            }

            if (_filterChanged)
            {
                #region need to revalidate the selector
                Count = 0;
                if (_filterText is null && Usage != Usage.None)
                {
                    for (int i = 0; i < Selector.Count; i++)
                    {
                        var (I, IN, TX) = Selector[i];
                        var child = model.Items[I];
                        var used = child.IsItemUsed;
                        var tIN = used ? (Usage == Usage.IsUsed) : (Usage == Usage.IsNotUsed);
                        if (tIN) Count++;
                        if (tIN != IN) Selector[i] = (I, tIN, TX);
                    }
                }
                else if (!(_filterText is null) && Usage == Usage.None)
                {
                    for (int i = 0; i < Selector.Count; i++)
                    {
                        var (I, IN, TX) = Selector[i];
                        var tIN = _filterRx.IsMatch(TX);
                        if (tIN) Count++;
                        if (tIN != IN) Selector[i] = (I, tIN, TX);
                    }
                }
                else if (!(_filterText is null) && Usage != Usage.None)
                {
                    for (int i = 0; i < Selector.Count; i++)
                    {
                        var (I, IN, TX) = Selector[i];
                        var child = model.Items[I];
                        var used = child.IsItemUsed;
                        var tIN = used ? (Usage == Usage.IsUsed) : (Usage == Usage.IsNotUsed);
                        tIN |= _filterRx.IsMatch(TX);
                        if (tIN) Count++;
                        if (tIN != IN) Selector[i] = (I, tIN, TX);
                    }
                }
                else
                {
                    for (int i = 0; i < Selector.Count; i++)
                    {
                        var (I, IN, TX) = Selector[i];
                        Selector[i] = (I, true, TX);
                    }
                    Count = Selector.Count;
                }
                #endregion
            }

            if (_sortChanged)
            {
                #region need to resort the selector
                _sortChanged = false;
                if (Sorting == Sorting.Unsorted)
                {
                    Selector.Sort(RestoreSelectorOrder);
                }
                else
                {
                    Selector.Sort(AlphaSortSelector);
                    if (Sorting == Sorting.Descending) Selector.Reverse();
                }
                #endregion
            }
        }
        private static int AlphaSortSelector((int, bool, string) a, (int, bool, string) b) => a.Item3.CompareTo(b.Item3);
        private static int RestoreSelectorOrder((int, bool, string) a, (int, bool, string) b) => a.Item1.CompareTo(b.Item1);
        #endregion

    }
}
