﻿using ModelGraph.Helpers;
using ModelGraph.Core;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace ModelGraph.Controls
{
    public sealed partial class SymbolEditControl
    {

        #region PropertyChanged  ==============================================

        private void EditContactComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditContact == Edit_Contact.Edit)
            {
                AutoFlipGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ContactGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                SelectorCanvas.Visibility = Windows.UI.Xaml.Visibility.Visible;
                PickerCanvas.Visibility = Windows.UI.Xaml.Visibility.Visible;
                PropertyGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                ColorPickerGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                SetIdleOnVoid();
            }
            else if (EditContact == Edit_Contact.Contacts)
            {
                PropertyGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ColorPickerGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                SelectorCanvas.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                PickerCanvas.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                AutoFlipGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ContactGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                SetContactsOnVoid();
            }
            else
            {
                PropertyGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ColorPickerGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                SelectorCanvas.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                PickerCanvas.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ContactGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                AutoFlipGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                SetContactsOnVoid();
            }
            EditorCanvas.Invalidate();
        }
        private void ColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            if (_changesEnabled)
            {
                _shapeColor = ColorPicker.Color;
                SetProperty(ProertyId.ShapeColor);
            }
        }
        private void StrokeWidthSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (_changesEnabled)
            {
                _strokeWidth = StrokeWidthSlider.Value;
                SetProperty(ProertyId.StrokeWidth);
            }
        }
        private void FillStroke_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_changesEnabled)
            {
                SetProperty(ProertyId.FillStroke);
            }
        }
        private void DashStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_changesEnabled)
            {
                SetProperty(ProertyId.DashStyle);
            }
        }
        private void LineJoin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_changesEnabled)
            {
                SetProperty(ProertyId.LineJoin);
            }
        }
        private void StartCap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_changesEnabled)
            {
                SetProperty(ProertyId.StartCap);
            }
        }
        private void EndCap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_changesEnabled)
            {
                SetProperty(ProertyId.EndCap);
            }
        }
        private void DashCap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_changesEnabled)
            {
                SetProperty(ProertyId.DashCap);
            }
        }


        private void Contact_N_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_N, ContactComboBox_N, ContactSizeSlider_N);
            EditorCanvas.Invalidate();
        }
        private void Contact_NE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_NE, ContactComboBox_NE, ContactSizeSlider_NE);
            EditorCanvas.Invalidate();
        }
        private void Contact_NW_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_NW, ContactComboBox_NW, ContactSizeSlider_NW);
            EditorCanvas.Invalidate();
        }
        private void Contact_NEC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_NEC, ContactComboBox_NEC, ContactSizeSlider_NEC);
            EditorCanvas.Invalidate();
        }
        private void Contact_NWC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_NWC, ContactComboBox_NWC, ContactSizeSlider_NWC);
            EditorCanvas.Invalidate();
        }

        private void Contact_E_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_E, ContactComboBox_E, ContactSizeSlider_E);
            EditorCanvas.Invalidate();
        }
        private void Contact_EN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_EN, ContactComboBox_EN, ContactSizeSlider_EN);
            EditorCanvas.Invalidate();
        }
        private void Contact_ES_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_ES, ContactComboBox_ES, ContactSizeSlider_ES);
            EditorCanvas.Invalidate();
        }

        private void Contact_W_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_W, ContactComboBox_W, ContactSizeSlider_W);
            EditorCanvas.Invalidate();
        }
        private void Contact_WN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_WN, ContactComboBox_WN, ContactSizeSlider_WN);
            EditorCanvas.Invalidate();
        }
        private void Contact_WS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_WS, ContactComboBox_WS, ContactSizeSlider_WS);
            EditorCanvas.Invalidate();
        }

        private void Contact_S_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_S, ContactComboBox_S, ContactSizeSlider_S);
            EditorCanvas.Invalidate();
        }
        private void Contact_SE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_SE, ContactComboBox_SE, ContactSizeSlider_SE);
            EditorCanvas.Invalidate();
        }
        private void Contact_SW_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_SW, ContactComboBox_SW, ContactSizeSlider_SW);
            EditorCanvas.Invalidate();
        }
        private void Contact_SEC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_SEC, ContactComboBox_SEC, ContactSizeSlider_SEC);
            EditorCanvas.Invalidate();
        }
        private void Contact_SWC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetContactHighlight(Contact_SWC, ContactComboBox_SWC, ContactSizeSlider_SWC);
            EditorCanvas.Invalidate();
        }

        private void SetContactHighlight(Contact con, ComboBox combo, Slider slide, float size = -1)
        {
            if (size >= 0)
                slide.Value = 100 * size;
            slide.Visibility = (con == Contact.Any) ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
            combo.Background = (con == Contact.None) ? (Brush)Resources["BackgroundNormal"] : (Brush)Resources["BackgroundPink"];
        }

        private void SetNewContactSize(Target targ, double value)
        {
            if (_contactSizeChangeEnabled)
            {
                var d = (float)(value / 100);
                if (Target_Contacts.TryGetValue(targ, out (Contact c, (sbyte x, sbyte y) p, byte s) v))
                {
                    Target_Contacts[targ] = (v.c, v.p, Shape.ToByte(d));
                    EditorCanvas.Invalidate();
                }
            }
        }
        private bool _contactSizeChangeEnabled;

        private void ContactSizeSlider_N_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.N, e.NewValue);
        }
        private void ContactSizeSlider_NE_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.NE, e.NewValue);
        }
        private void ContactSizeSlider_NW_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.NW, e.NewValue);
        }
        private void ContactSizeSlider_NEC_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.NEC, e.NewValue);
        }
        private void ContactSizeSlider_NWC_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.NWC, e.NewValue);
        }

        private void ContactSizeSlider_E_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.E, e.NewValue);
        }
        private void ContactSizeSlider_EN_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.EN, e.NewValue);
        }
        private void ContactSizeSlider_ES_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.ES, e.NewValue);
        }

        private void ContactSizeSlider_W_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.W, e.NewValue);
        }
        private void ContactSizeSlider_WN_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.WN, e.NewValue);
        }
        private void ContactSizeSlider_WS_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.WS, e.NewValue);
        }
        private void ContactSizeSlider_S_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.S, e.NewValue);
        }
        private void ContactSizeSlider_SE_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.SE, e.NewValue);
        }
        private void ContactSizeSlider_SW_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.SW, e.NewValue);
        }
        private void ContactSizeSlider_SEC_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.SEC, e.NewValue);
        }
        private void ContactSizeSlider_SWC_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            SetNewContactSize(Target.SWC, e.NewValue);
        }

        private void TestFlipVert_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            TestAutoFlipStart(FlipState.VertFlip);
        }

        private void TestFlipHorz_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            TestAutoFlipStart(FlipState.VertHorzFlip);
        }

        private void TestFlipVertHorz_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            TestAutoFlipStart(FlipState.VertHorzFlip);
        }

        private void TestRotateLeft_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            TestAutoFlipStart(FlipState.LeftRotate);
        }

        private void TestRotateLeftFlip_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            TestAutoFlipStart(FlipState.LeftHorzFlip);
        }

        private void TestRotateRight_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            TestAutoFlipStart(FlipState.RightRotate);
        }

        private void TestRotateRightFlip_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            TestAutoFlipStart(FlipState.RightHorzFlip);
        }

        private void TestAutoFlipStop_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            TestAutoFlipStop();
        }

        void TestAutoFlipStart(FlipState flip)
        {
        }
        void TestAutoFlipStop()
        {
        }

        private void InitAutoFlipCheckBoxes(AutoFlip flip)
        {
            _autoFlipVert = (flip & AutoFlip.VertFlip) != 0;
            _autoFlipHorz = (flip & AutoFlip.HorzFlip) != 0;
            _autoFlipVertHorz = (flip & AutoFlip.BothFlip) != 0;
            _autoRotateLeft = (flip & AutoFlip.RotateLeft) != 0;
            _autoRotateLeftFlip = (flip & AutoFlip.LeftHorzFlip) != 0;
            _autoRotateRight = (flip & AutoFlip.RotateRight) != 0;
            _autoRotateRightFlip = (flip & AutoFlip.RightHorzFlip) != 0;
        }
        private void ApplyAutoFlip()
        {
            _symbol.AutoFlip =
                (AutoFlipVert ? AutoFlip.VertFlip : AutoFlip.None) |
                (AutoFlipHorz ? AutoFlip.HorzFlip : AutoFlip.None) |
                (AutoFlipVertHorz ? AutoFlip.BothFlip : AutoFlip.None) |
                (AutoRotateLeft ? AutoFlip.RotateLeft : AutoFlip.None) |
                (AutoRotateLeftFlip ? AutoFlip.LeftHorzFlip : AutoFlip.None) |
                (AutoRotateRight ? AutoFlip.RotateRight : AutoFlip.None) |
                (AutoRotateRightFlip ? AutoFlip.RightHorzFlip : AutoFlip.None);
        }


        #endregion

        #region CentralSize  ==================================================
        private void CentralSizeSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            var value = (float)CentralSizeSlider.Value;
            if (Changed(value, _centralSize))
            {
                Shape.ResizeCentral(SelectedShapes, value);
                LockPolyline();
                SetSizeSliders();
                EditorCanvas.Invalidate();
            }
        }
        private void SetCentralSize(float value)
        {
            _centralSize = value;
            CentralSizeSlider.Value = value;
        }
        private float _centralSize;
        #endregion

        #region VerticalSize  =================================================
        private void VerticalSizeSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            var value = (float)VerticalSizeSlider.Value;
            if (Changed(value, _verticalSize))
            {
                Shape.ResizeVertical(SelectedShapes, value);
                LockPolyline();
                SetSizeSliders();
                EditorCanvas.Invalidate();
            }
        }
        private void SetVerticalSize(float value)
        {
            _verticalSize = value;
            VerticalSizeSlider.Value = value;
        }
        private float _verticalSize;
        #endregion

        #region HorizontalSize  ===============================================
        private void HorizontalSizeSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            var value = (float)HorizontalSizeSlider.Value;
            if (Changed(value, _horizontalSize))
            {
                Shape.ResizeHorizontal(SelectedShapes, value);
                LockPolyline();
                SetSizeSliders();
                EditorCanvas.Invalidate();
            }
        }
        private void SetHorizontalSize(float value)
        {
            _horizontalSize = value;
            HorizontalSizeSlider.Value = value;
        }
        private float _horizontalSize;
        #endregion

        #region MajorSize  ====================================================
        private void MajorSizeSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            var value = (float)MajorSizeSlider.Value;
            if (Changed(value, _majorSize))
            {
                Shape.ResizeMajorAxis(SelectedShapes, value);
                SetSizeSliders();
                EditorCanvas.Invalidate();
            }
        }
        private void SetMajorSize(float value)
        {
            _majorSize = value;
            MajorSizeSlider.Value = value;
        }
        private float _majorSize;
        #endregion

        #region MinorAxisSize  ================================================
        private void MinorSizeSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            var value = (float)MinorSizeSlider.Value;
            if (Changed(value, _minorSize))
            {
                Shape.ResizeMinorAxis(SelectedShapes, value);
                SetSizeSliders();
                EditorCanvas.Invalidate();
            }
        }
        private void SetMinorSize(float value)
        {
            _minorSize = value;
            MinorSizeSlider.Value = value;
        }
        private float _minorSize;
        #endregion

        #region PolyDimension  ================================================
        private void DimensionSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            var value = (float)DimensionSlider.Value;
            if (Changed(value, _polyDimension))
            {
                Shape.SetDimension(SelectedShapes, value);
                SetSizeSliders();
                EditorCanvas.Invalidate();
            }
        }
        private void SetDemeinsonSize(float min, float max, float value)
        {
            _polyDimension = value;
            DimensionSlider.Minimum = min;
            DimensionSlider.Maximum = max;
            DimensionSlider.Value = value;
        }
        private float _polyDimension;
        #endregion

        #region TernarySize  ==================================================
        private void TernarySizeSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            var value = (float)TernarySizeSlider.Value;
            if (Changed(value, _ternarySize))
            {
                Shape.ResizeTernaryAxis(SelectedShapes, value);
                SetSizeSliders();
                EditorCanvas.Invalidate();
            }
        }
        private void SetTernarySize(float value)
        {
            _ternarySize = value;
            TernarySizeSlider.Value = value;
        }
        private float _ternarySize;
        #endregion

        #region SetSizeSliders  ===============================================
        private void SetSizeSliders()
        {
            _changesEnabled = false;

            var (locked, min, max, dim, aux, major, minor, cent, vert, horz) = Shape.GetSliders(SelectedShapes);

            if (cent < 0)
                CentralSizeSlider.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            else
            {
                SetCentralSize(cent);
                CentralSizeSlider.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            if (vert < 0)
                VerticalSizeSlider.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            else
            {
                SetVerticalSize(vert);
                VerticalSizeSlider.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            if (horz < 0)
                HorizontalSizeSlider.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            else
            {
                SetHorizontalSize(horz);
                HorizontalSizeSlider.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            if (major < 0)
                MajorSizeSlider.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            else
            {
                SetMajorSize(major);
                MajorSizeSlider.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            if (minor < 0)
                MinorSizeSlider.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            else
            {
                SetMinorSize(minor);
                MinorSizeSlider.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            if (aux < 0)
                TernarySizeSlider.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            else
            {
                SetTernarySize(aux);
                TernarySizeSlider.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            if (dim < 0)
                DimensionSlider.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            else
            {
                SetDemeinsonSize(min, max, dim);
                DimensionSlider.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            CheckPolyLocked(locked);
            _changesEnabled = true;
        }
        private bool Changed(float v1, float v2) => _changesEnabled && v1 != v2;
        bool _changesEnabled;
        #endregion

        #region LeftButtonClick  ==============================================
        private void LockButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (IsPolylineLocked)
                UnlockPolyline();
            else
                LockPolyline();
        }
        private void CheckPolyLocked(bool isLocked)
        {
            if (IsPolylineLocked)
            {
                if (!isLocked) UnlockPolyline();
            }
            else
            {
                if (isLocked) LockPolyline();
            }
        }
        private void UnlockPolyline()
        {
            Shape.LockSliders(SelectedShapes, false);
            LockButton.Content = "\uE785";
            LockButton.Background = (Brush)Resources["BackgroundNormal"];
            ToolTipService.SetToolTip(LockButton, "_02B".GetLocalized());
            DimensionSlider.IsEnabled = true;
            TernarySizeSlider.IsEnabled = true;
            MajorSizeSlider.IsEnabled = true;
            MinorSizeSlider.IsEnabled = true;
            IsPolylineLocked = false;
        }
        private void LockPolyline()
        {
            Shape.LockSliders(SelectedShapes, true);
            LockButton.Content = "\uE72E";
            LockButton.Background = (Brush)Resources["BackgroundPink"];
            ToolTipService.SetToolTip(LockButton, "_02A".GetLocalized());
            DimensionSlider.IsEnabled = false;
            TernarySizeSlider.IsEnabled = false;
            MajorSizeSlider.IsEnabled = false;
            MinorSizeSlider.IsEnabled = false;
            IsPolylineLocked = true;
        }
        private bool IsPolylineLocked = true;
        private void OneManyButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ToggleOneManyButton();
        }
        private void ToggleOneManyButton()
        {
            if (IsSelectOneOrMoreShapeMode)
            {
                OneManyButton.Content = "\uE8C5";
                ToolTipService.SetToolTip(OneManyButton, "_01A".GetLocalized());
                IsSelectOneOrMoreShapeMode = false;
            }
            else
            {
                OneManyButton.Content = "\uE8C4";
                ToolTipService.SetToolTip(OneManyButton, "_01B".GetLocalized());
                IsSelectOneOrMoreShapeMode = true;
            }
        }
        private bool IsSelectOneOrMoreShapeMode = true;

        private void CutButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (SelectedShapes.Count > 0)
            {
                CutCopyShapes.Clear();
                foreach (var shape in SelectedShapes)
                {
                    CutCopyShapes.Add(shape);
                    SymbolShapes.Remove(shape);
                }
                SelectedShapes.Clear();

                EditorCanvas.Invalidate();
            }
        }

        private void CopyButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (SelectedShapes.Count > 0)
            {
                CutCopyShapes.Clear();
                foreach (var shape in SelectedShapes)
                {
                    CutCopyShapes.Add(shape.Clone());
                }
                SelectedShapes.Clear();

                EditorCanvas.Invalidate();
            }
        }

        private void PasteButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (CutCopyShapes.Count > 0)
            {
                SelectedShapes.Clear();
                foreach (var template in CutCopyShapes)
                {
                    var shape = template.Clone();
                    SymbolShapes.Add(shape);
                    SelectedShapes.Add(shape);
                }
                SetShapesAreSelected();
                EditorCanvas.Invalidate();
            }
        }

        private void RecenterButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (SelectedShapes.Count > 0)
            {
                Shape.SetCenter(SelectedShapes, Vector2.Zero);
                EditorCanvas.Invalidate();
            }
        }
        private void RotateLeftButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => RotateLeft();
        private void RotateRightButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => RotateRight();
        private void RotateAngleButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_use30degreeDelta)
            {
                _use30degreeDelta = false;
                RotateAngleButton.Content = "22.5";
            }
            else
            {
                _use30degreeDelta = true;
                RotateAngleButton.Content = "30.0";
            }
        }
        private void FlipVerticalButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => FlipVertical();
        private void FlipHorizontalButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => FlipHorizontal();
        #endregion

        #region RightButtonClick  =============================================
        private void ScratchButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void ApplyButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _symbol.Data = Shape.Serialize(SymbolShapes);
            _symbol.SetTargetContacts(Target_Contacts);
            _symbol.Version += 1;

            ApplyAutoFlip();

            List<LineCommand> buttonCommands = new List<LineCommand>(2);
            //_rootModel.PageButtonComands(buttonCommands);
            //foreach (var cmd in buttonCommands)
            //{
            //    if (cmd.IsSaveCommand) cmd.Execute();
            //    return;
            //}
        }

        private void ReloadButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Shape.Deserialize(_symbol.Data, SymbolShapes);
            EditorCanvas.Invalidate();
        }
        #endregion

        #region Picker_PointerEvents  =========================================
        private int _pickerIndex = -1;
        private void PickerCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PickerShape = null;

            _pickerIndex = GetPickerShapeIndex(e);

            PickerCanvas.Invalidate();
        }

        private void PickerCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var index = GetPickerShapeIndex(e);
            if (index < 0 || (index != _pickerIndex)) return;

            SetPicker(PickerShapes[index]);

            PickerCanvas.Invalidate();
            EditorCanvas.Invalidate();
        }
        private int GetPickerShapeIndex(PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(PickerCanvas).Position;
            int index = (int)(p.Y / PickerCanvas.Width);
            return (index < 0 || index >= PickerShapes.Count) ? -1 : index;
        }
        #endregion

        #region Symbol_PointerEvents  =========================================
        private bool _symbolPointerPressed;
        private void SymbolCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _symbolPointerPressed = true;
        }

        private void SymbolCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (_symbolPointerPressed)
            {
                _symbolPointerPressed = false;

                if (SelectedShapes.Count > 0)
                {
                    SelectedShapes.Clear();
                    SetIdleOnVoid();
                }
                else
                {
                    foreach (var shape in SymbolShapes) { SelectedShapes.Add(shape); }
                    SetShapesAreSelected();
                }


                PickerShape = null;
                PickerCanvas.Invalidate();
            }
        }
        #endregion

        #region Selector_PointerEvents  =======================================
        private int _selectorIndex = -1;
        private bool _selectPointerPressed;
        private bool _ignoreSelectPointerReleased;
        private void SelectorCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _selectPointerPressed = true;
            _selectorIndex = GetSelectorIndex(e);
        }

        private void SelectorCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_selectPointerPressed && _selectorIndex >= 0 && SelectedShapes.Count == 0)
            {
                var index = GetSelectorIndex(e);
                if (index >= 0 && index != _selectorIndex && SelectedShapes.Count == 0)
                {
                    var shape = SymbolShapes[_selectorIndex];
                    SymbolShapes[_selectorIndex] = SymbolShapes[index];
                    SymbolShapes[index] = shape;
                    _selectorIndex = index;
                    _ignoreSelectPointerReleased = true;
                    EditorCanvas.Invalidate();
                }
            }
        }

        private void SelectorCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (_selectPointerPressed && !_ignoreSelectPointerReleased)
            {
                var index = GetSelectorIndex(e);
                if (index < 0 || _selectorIndex < 0)
                    ShapeSelectorMiss();
                else if (index == _selectorIndex)
                    ShapeSelectorHit(index);
            }
            _selectPointerPressed = false;
            _ignoreSelectPointerReleased = false;
        }
        private int GetSelectorIndex(PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(SelectorCanvas).Position;
            int index = (int)(p.Y / SelectorCanvas.Width);
            return (index < 0 || index >= SymbolShapes.Count) ? -1 : index;
        }
        #endregion

        #region Editor_PointerEvents  =========================================
        private bool _editorPointerPressed;
        private void EditorCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _editorPointerPressed = true;
            RawPoint1 = GetRawPoint(e);
            ShapePoint1 = ShapePoint(RawPoint1);

            TryInvokeEventAction(PointerEvent.Begin);
        }
        private void EditorCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            RawPoint2 = GetRawPoint(e);
            ShapePoint2 = ShapePoint(RawPoint2);

            if (_editorPointerPressed)
                TryInvokeEventAction(PointerEvent.Drag);
            else
                TryInvokeEventAction(PointerEvent.Hover);
            RawPoint1 = RawPoint2;
            ShapePoint1 = ShapePoint(RawPoint1);
        }
        private void EditorCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _editorPointerPressed = false;
            RawPoint2 = GetRawPoint(e);
            ShapePoint2 = ShapePoint(RawPoint2);

            TryInvokeEventAction(PointerEvent.End);
        }
        private Vector2 GetRawPoint(PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(EditorCanvas).Position;
            return new Vector2((float)p.X, (float)p.Y);
        }
        private Vector2 ShapePoint(Vector2 rawPoint) => (rawPoint - Center) / EditScale;
        #endregion
    }
}
