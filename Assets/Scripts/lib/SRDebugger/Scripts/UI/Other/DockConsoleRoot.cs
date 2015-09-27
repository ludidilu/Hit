using System;
using SRDebugger.Internal;
using SRDebugger.Services;
using SRDebugger.UI.Controls;
using SRF;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRDebugger.UI.Other
{
	public class DockConsoleRoot : SRMonoBehaviourEx
	{

		public const float NonFocusOpacity = 0.65f;

		[RequiredField] public Canvas Canvas;

		[RequiredField] public ConsoleLogControl Console;

		[RequiredField] public ScrollRect ScrollRect;

		[RequiredField] public RectTransform DropdownTransform;

		[RequiredField] public CanvasGroup DropdownCanvasGroup;

		[RequiredField] public CanvasGroup CanvasGroup;

		[RequiredField] public UIBehaviour DragHandle;

		[RequiredField] public Toggle ToggleErrors;
		[RequiredField] public Toggle ToggleWarnings;
		[RequiredField] public Toggle ToggleInfo;

		[RequiredField] public Text TextErrors;
		[RequiredField] public Text TextWarnings;
		[RequiredField] public Text TextInfo;

		[RequiredField] public Image DropdownToggleSprite;

		public const float MinDropdownSize = 50f;

		public const float SmoothPadding = 20f;

		public float MaxDropdownSize { get { return (Canvas.transform as RectTransform).sizeDelta.y*0.8f; } }

		private float _targetSize;
		private float _smooothedSize;

		private bool _dropdownVisible = true;
		private bool _isDirty;

		private int _pointersOver;
		private bool _isDragging;

		private CanvasScaler _canvasScaler;

		protected override void Start()
		{

			base.Start();

			_canvasScaler = Canvas.GetComponent<CanvasScaler>();
			Service.Console.Updated += ConsoleOnUpdated;

			Refresh();
			RefreshAlpha();

		}

		protected override void OnDestroy()
		{

			base.OnDestroy();

			if (Service.Console != null)
				Service.Console.Updated -= ConsoleOnUpdated;

		}

		protected override void Update()
		{

			base.Update();

			if (_isDirty)
				Refresh();
			
		}

		private void Refresh()
		{

			// Update total counts labels
			TextInfo.text = SRDebuggerUtil.GetNumberString(Service.Console.InfoCount, 999, "999+");
			TextWarnings.text = SRDebuggerUtil.GetNumberString(Service.Console.WarningCount, 999, "999+");
			TextErrors.text = SRDebuggerUtil.GetNumberString(Service.Console.ErrorCount, 999, "999+");

			_isDirty = false;

		}

		private void RefreshAlpha()
		{

			if (_isDragging)
				_pointersOver = 0;

			if (_isDragging || _pointersOver > 0) {
				CanvasGroup.alpha = 1.0f;
			} else {
				CanvasGroup.alpha = NonFocusOpacity;
			}

		}

		public void SetDropdownVisibility(bool visible)
		{

			DropdownCanvasGroup.interactable = visible;
			DropdownCanvasGroup.alpha = visible ? 1f : 0f;
			DropdownCanvasGroup.blocksRaycasts = visible;

			DropdownToggleSprite.rectTransform.localRotation = Quaternion.Euler(0, 0, visible ? 0f : 180f);

			_dropdownVisible = visible;

		}

		public void ToggleDropdownVisible()
		{
			SetDropdownVisibility(!_dropdownVisible);
		}

		public void MenuButtonPressed()
		{
			SRDebug.Instance.ShowDebugPanel(DefaultTabs.Console);
		}


		public void ClearButtonPressed()
		{
			Service.Console.Clear();
		}

		public void OnHandleDragStart(BaseEventData d)
		{

			_smooothedSize = _targetSize = DropdownTransform.sizeDelta.y;

			_isDragging = true;
			RefreshAlpha();

		}

		public void OnHandleDragEnd(BaseEventData d)
		{

			var e = d as PointerEventData;

			_isDragging = false;
			RefreshAlpha();
			
			Scroll(e.delta.y);

		}

		public void OnDrag(BaseEventData d)
		{

			var e = d as PointerEventData;
			Scroll(e.delta.y);

		}

		public void OnPointerUp(BaseEventData e)
		{

			_isDragging = false;
			RefreshAlpha();

		}

		void Scroll(float delta)
		{

			if (_canvasScaler != null)
				delta /= _canvasScaler.scaleFactor;

			_targetSize -= delta;
			_smooothedSize = Mathf.Clamp(_targetSize, MinDropdownSize, MaxDropdownSize);

			var p = ScrollRect.verticalNormalizedPosition;

			DropdownTransform.sizeDelta = new Vector2(DropdownTransform.sizeDelta.x, _smooothedSize);

			ScrollRect.verticalNormalizedPosition = p;

		}

		private void ConsoleOnUpdated(IConsoleService console)
		{
			_isDirty = true;
		}

		public void TogglesUpdated()
		{

			Console.ShowErrors = ToggleErrors.isOn;
			Console.ShowWarnings = ToggleWarnings.isOn;
			Console.ShowInfo = ToggleInfo.isOn;

			if (!_dropdownVisible)
				SetDropdownVisibility(true);

		}

		public void PointerEnter(BaseEventData e)
		{
			_pointersOver++;
			RefreshAlpha();
		}

		public void PointerExit(BaseEventData e)
		{
			_pointersOver = Mathf.Max(0, _pointersOver - 1);
			RefreshAlpha();
		}


	}
}
