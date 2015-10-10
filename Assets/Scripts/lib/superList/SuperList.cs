using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using System;

namespace xy3d.tstd.lib.superList
{

//	[RequireComponent (typeof(Image))]
//	[RequireComponent (typeof(Mask))]
	[RequireComponent (typeof(RectMask2D))]
	[RequireComponent (typeof(ScrollRect))]
	public class SuperList : MonoBehaviour
	{
		public bool autoStart = true;
		public bool needSelectedIndex = true;

		public float verticalGap;
		public float horizontalGap;

		public GameObject go;

		private GameObject container;
		private GameObject pool;

		private RectTransform rectTransform;
		private ScrollRect scrollRect;

		private float width;
		private float height;
		private float cellWidth;
		private float cellHeight;

		private float cellScale;

		private bool isVertical;

		private float containerWidth;
		private float containerHeight;

		private List<object> data = new List<object> ();

		private int rowNum;//最多同时显示多少行
		private int colNum;//最多同时显示多少列

		private List<GameObject> showPool = new List<GameObject> ();
		private List<GameObject> hidePool = new List<GameObject> ();

		private int showIndex;
		private int selectedIndex = -1;
		private float curPercent = 0;
		
		
		public Action<object> CellClickHandle;
		public Action<int> CellClickIndexHandle;

		public void SetData<T> (List<T> _data)
		{

			data.Clear ();

			foreach (T unit in _data) {

				data.Add (unit);
			}

			if (isVertical) {

				containerHeight = Mathf.Ceil (1.0f * (data.Count - colNum) / colNum) * (cellHeight + verticalGap) + cellHeight;

				if (containerHeight < height) {

					containerHeight = height;
				}

				rectTransform.anchoredPosition = new Vector2 (rectTransform.anchoredPosition.x, 0f);
				rectTransform.offsetMin = new Vector2 (rectTransform.offsetMin.x, -containerHeight);

			} else {

				containerWidth = Mathf.Ceil (1.0f * (data.Count - rowNum) / rowNum) * (cellWidth + horizontalGap) + cellWidth;

				if (containerWidth < width) {
					
					containerWidth = width;
				}

				rectTransform.anchoredPosition = new Vector2 (0f, rectTransform.anchoredPosition.y);
				rectTransform.offsetMax = new Vector2 (containerWidth, rectTransform.offsetMax.y);
			}

			showIndex = 0;

			foreach (GameObject unit in showPool) {

				unit.transform.SetParent (pool.transform, false);

				hidePool.Add (unit);
			}

			showPool.Clear ();

			ResetPos (0);

			curPercent = 0;
		}

		public void SetDataAndKeepLocation<T> (List<T> _data)
		{
			float percent = curPercent;

			SetData<T> (_data);

			SetCurPercent (percent);
		}

		public void Clear ()
		{
			data.Clear ();
			SetData (data);
		}

		private void SetSize ()
		{

			if (isVertical) {

				float tmpCellWidth = (go.transform as RectTransform).rect.width;

				if (width >= tmpCellWidth) {

					colNum = (int)Mathf.Floor ((width - tmpCellWidth) / (tmpCellWidth + horizontalGap)) + 1;

					cellScale = 1;

					cellWidth = tmpCellWidth;
					cellHeight = (go.transform as RectTransform).rect.height;

				} else {

					colNum = 1;

					cellScale = width / tmpCellWidth;

					cellWidth = width;
					cellHeight = (go.transform as RectTransform).rect.height * cellScale;
				}

				int n = (int)(height / (cellHeight + verticalGap));
				
				if (height - n * (cellHeight + verticalGap) < verticalGap) {
					
					rowNum = n + 1;
					
				} else {
					
					rowNum = n + 2;
				}

			} else {

				float tmpCellHeight = (go.transform as RectTransform).rect.height;

				if (height >= tmpCellHeight) {

					rowNum = (int)Mathf.Floor ((height - tmpCellHeight) / (tmpCellHeight + verticalGap)) + 1;

					cellScale = 1;

					cellWidth = (go.transform as RectTransform).rect.width;
					cellHeight = tmpCellHeight;

				} else {

					rowNum = 1;
					
					cellScale = height / tmpCellHeight;
					
					cellWidth = (go.transform as RectTransform).rect.width * cellScale;
					cellHeight = height;
				}

				int n = (int)(width / (cellHeight + verticalGap));
				
				if (width - n * (cellWidth + horizontalGap) < horizontalGap) {
					
					colNum = n + 1;
					
				} else {
					
					colNum = n + 2;
				}
				
//				rowNum = (int)Mathf.Floor ((height - cellHeight) / (cellHeight + verticalGap)) + 1;

			}
		}

		void CreateCells ()
		{

			for (int i = 0; i < rowNum * colNum; i++) {
				
				GameObject unit = GameObject.Instantiate (go);
				
				unit.transform.SetParent (pool.transform, false);

				(unit.transform as RectTransform).localScale = new Vector3 (cellScale, cellScale, cellScale);
				
				(unit.transform as RectTransform).pivot = new Vector2 (0, 1);
				
				(unit.transform as RectTransform).anchorMin = new Vector2 (0, 1);
				
				(unit.transform as RectTransform).anchorMax = new Vector2 (0, 1);

				hidePool.Add (unit);
			}
		}

		// Use this for initialization
		void Awake ()
		{
			if (autoStart) {

				Init ();
			}
		}

		public void Init ()
		{

			container = new GameObject ("Container", typeof(RectTransform));
			
			container.transform.SetParent (transform, false);
			
			pool = new GameObject ("Pool", typeof(RectTransform));
			
			pool.transform.SetParent (transform, false);
			
			pool.SetActive (false);
			
			rectTransform = container.transform as RectTransform;
			
			rectTransform.anchorMin = new Vector2 (0, 1);
			rectTransform.anchorMax = new Vector2 (0, 1);

			rectTransform.pivot = new Vector2 (0, 1);
			
			rectTransform.offsetMin = new Vector2 ();
			rectTransform.offsetMax = new Vector2 ();
			
			scrollRect = gameObject.GetComponent<ScrollRect> ();
			scrollRect.content = rectTransform;
			
			width = (transform as RectTransform).rect.width;
			height = (transform as RectTransform).rect.height;
			

			
			scrollRect.onValueChanged.AddListener (new UnityEngine.Events.UnityAction<Vector2> (OnScroll));
			
			isVertical = scrollRect.vertical;
			
			SetSize ();
			
			CreateCells ();
		}

		void OnScroll (Vector2 _v)
		{
			int nowIndex;

			if (isVertical) {

				curPercent = _v.y;

				float y;

				if (curPercent < 0) {
					
					y = 1;
					
				} else if (curPercent > 1) {
					
					y = 0;
					
				} else {
					
					y = 1 - curPercent;
				}

				nowIndex = (int)(y * (containerHeight - height) / (cellHeight + verticalGap)) * colNum;

			} else {

				curPercent = _v.x;

				float x;

				if (curPercent < 0) {
					
					x = 0;
					
				} else if (curPercent > 1) {
					
					x = 1;
					
				} else {
					
					x = curPercent;
				}

				nowIndex = (int)(x * (containerWidth - width) / (cellWidth + horizontalGap)) * rowNum;
			}

			if (nowIndex != showIndex) {

				ResetPos (nowIndex);
			}
		}

		private void ResetPos (int _nowIndex)
		{
			if (data.Count == 0) {

				return;
			}

			if (isVertical) {

				if (_nowIndex - showIndex == colNum) {

					int row = _nowIndex / colNum + (rowNum - 1);

					for (int i = 0; i < _nowIndex - showIndex; i++) {

						int newIndex = showIndex + rowNum * colNum + i;

						GameObject unit = showPool [0];

						showPool.RemoveAt (0);

						int col = i;

						if (newIndex < data.Count) {

							showPool.Add (unit);

							ShowCell (unit, new Vector2 (col * (cellWidth + horizontalGap), -row * (cellHeight + verticalGap)), newIndex);

						} else {

							hidePool.Add (unit);

							unit.transform.SetParent (pool.transform, false);
						}
					}

				} else if (_nowIndex - showIndex == -colNum) {

					int row = _nowIndex / colNum;
				
					for (int i = 0; i < showIndex - _nowIndex; i++) {
					
						int newIndex = showIndex - colNum + i;

						GameObject unit;

						if (showPool.Count == rowNum * colNum) {

							unit = showPool [showPool.Count - 1];

							showPool.RemoveAt (showPool.Count - 1);

						} else {

							unit = hidePool [0];

							hidePool.RemoveAt (0);

							unit.transform.SetParent (container.transform, false);
						}

						showPool.Insert (0, unit);

						int col = i;

						ShowCell (unit, new Vector2 (col * (cellWidth + horizontalGap), -row * (cellHeight + verticalGap)), newIndex);
					}

				} else {

					List<int> tmpList = new List<int> ();

					for (int i = 0; i < rowNum * colNum; i++) {

						if (_nowIndex + i < data.Count) {

							tmpList.Add (_nowIndex + i);

						} else {

							break;
						}
					}

					GameObject[] newShowPool = new GameObject[tmpList.Count];

					List<GameObject> replacePool = new List<GameObject> ();

					for (int i = 0; i < showPool.Count; i++) {

						GameObject unit = showPool [i];

						int tmpIndex = unit.GetComponent<SuperListCell> ().index;

						if (tmpList.Contains (tmpIndex)) {

							newShowPool [tmpList.IndexOf (tmpIndex)] = unit;

						} else {

							replacePool.Add (unit);
						}
					}

					showPool.Clear ();

					for (int i = 0; i < newShowPool.Length; i++) {

						if (newShowPool [i] == null) {

							GameObject unit;

							if (replacePool.Count > 0) {

								unit = replacePool [0];

								replacePool.RemoveAt (0);

							} else {

								unit = hidePool [0];

								hidePool.RemoveAt (0);

								unit.transform.SetParent (container.transform, false);
							}

							newShowPool [i] = unit;

							int row = tmpList [i] / colNum;
						
							int col = tmpList [i] % colNum;

							ShowCell (unit, new Vector2 (col * (cellWidth + horizontalGap), -row * (cellHeight + verticalGap)), tmpList [i]);
						}

						showPool.Add (newShowPool [i]);
					}

					foreach (GameObject unit in replacePool) {

						unit.transform.SetParent (pool.transform, false);

						hidePool.Add (unit);
					}
				}
			} else {

				if (_nowIndex - showIndex == rowNum) {
					
					int col = _nowIndex / rowNum + (colNum - 1);
					
					for (int i = 0; i < _nowIndex - showIndex; i++) {
						
						int newIndex = showIndex + rowNum * colNum + i;
						
						GameObject unit = showPool [0];
//						Debug.Log (unit.GetComponent<SuperListCell> ().index);
						showPool.RemoveAt (0);
						
						int row = i;
						
						if (newIndex < data.Count) {
							
							showPool.Add (unit);
							
							ShowCell (unit, new Vector2 (col * (cellWidth + horizontalGap), -row * (cellHeight + verticalGap)), newIndex);
							
						} else {
							
							hidePool.Add (unit);
							
							unit.transform.SetParent (pool.transform, false);
						}
					}
					
				} else if (_nowIndex - showIndex == -rowNum) {
					
					int col = _nowIndex / rowNum;
					
					for (int i = 0; i < showIndex - _nowIndex; i++) {
						
						int newIndex = showIndex - rowNum + i;
						
						GameObject unit;
						
						if (showPool.Count == rowNum * colNum) {
							
							unit = showPool [showPool.Count - 1];
							
							showPool.RemoveAt (showPool.Count - 1);
							
						} else {
							
							unit = hidePool [0];
							
							hidePool.RemoveAt (0);
							
							unit.transform.SetParent (container.transform, false);
						}
						
						showPool.Insert (0, unit);
						
						int row = i;
						
						ShowCell (unit, new Vector2 (col * (cellWidth + horizontalGap), -row * (cellHeight + verticalGap)), newIndex);
					}
					
				} else {
					
					List<int> tmpList = new List<int> ();
					
					for (int i = 0; i < rowNum * colNum; i++) {
						
						if (_nowIndex + i < data.Count) {
							
							tmpList.Add (_nowIndex + i);
							
						} else {
							
							break;
						}
					}
					
					GameObject[] newShowPool = new GameObject[tmpList.Count];
					
					List<GameObject> replacePool = new List<GameObject> ();
					
					for (int i = 0; i < showPool.Count; i++) {
						
						GameObject unit = showPool [i];
						
						int tmpIndex = unit.GetComponent<SuperListCell> ().index;
						
						if (tmpList.Contains (tmpIndex)) {
							
							newShowPool [tmpList.IndexOf (tmpIndex)] = unit;
							
						} else {
							
							replacePool.Add (unit);
						}
					}
					
					showPool.Clear ();
					
					for (int i = 0; i < newShowPool.Length; i++) {
						
						if (newShowPool [i] == null) {
							
							GameObject unit;
							
							if (replacePool.Count > 0) {
								
								unit = replacePool [0];
								
								replacePool.RemoveAt (0);
								
							} else {
								
								unit = hidePool [0];
								
								hidePool.RemoveAt (0);
								
								unit.transform.SetParent (container.transform, false);
							}
							
							newShowPool [i] = unit;
							
							int col = tmpList [i] / rowNum;
							
							int row = tmpList [i] % rowNum;
							
							ShowCell (unit, new Vector2 (col * (cellWidth + horizontalGap), -row * (cellHeight + verticalGap)), tmpList [i]);
						}	
						
						showPool.Add (newShowPool [i]);
					}
					
					foreach (GameObject unit in replacePool) {
						
						unit.transform.SetParent (pool.transform, false);
						
						hidePool.Add (unit);
					}
				}
			}
			showIndex = _nowIndex;
		}

		void ShowCell (GameObject _cell, Vector2 _pos, int _index)
		{
			_cell.GetComponent<RectTransform> ().anchoredPosition = _pos;
			
			_cell.GetComponent<SuperListCell> ().index = _index;

			_cell.GetComponent<SuperListCell> ().SetSelected (_index == selectedIndex);
			
			_cell.GetComponent<SuperListCell> ().SetData (data [_index]);
		}
	

		void CellClick (GameObject obj)
		{
			SuperListCell cell = obj.GetComponent<SuperListCell> ();

			SetSelectedIndex (cell.index);
		}

		public void SetSelectedIndex (int _index)
		{
			if (selectedIndex == _index) {

				return;
			}

			if (needSelectedIndex) {

				foreach (GameObject unit in showPool) {

					SuperListCell cell = unit.GetComponent<SuperListCell> ();
						
					if (cell.index == selectedIndex) {

						cell.SetSelected (false);

					} else if (cell.index == _index) {

						cell.SetSelected (true);
					}
				}

				selectedIndex = _index;
			}

			if (selectedIndex != -1 || !needSelectedIndex) {

				if (CellClickHandle != null) {
					
					CellClickHandle (data [_index]);
				}

				if (CellClickIndexHandle != null) {

					CellClickIndexHandle (_index);
				}
			}
		}

		public int GetSelectedIndex ()
		{
			
			return selectedIndex;
		}

		public void DisplayIndex (int _index)
		{
			float finalPos;

			if (isVertical) {

				if (containerHeight > height) {

					float pos = (_index / colNum) * (cellHeight + verticalGap);

					finalPos = 1 - pos / (containerHeight - height);

					if (finalPos < 0) {

						finalPos = 0;

					} else if (finalPos > 1) {

						finalPos = 1;
					}

				} else {

					finalPos = 1;
				}

				scrollRect.verticalNormalizedPosition = finalPos;

			} else {

				if (containerWidth > width) {

					float pos = (_index / rowNum) * (cellWidth + horizontalGap);

					finalPos = pos / (containerWidth - width);

					if (finalPos < 0) {
						
						finalPos = 0;

					} else if (finalPos > 1) {

						finalPos = 1;
					}

				} else {

					finalPos = 0;
				}

				scrollRect.horizontalNormalizedPosition = finalPos;
			}

			curPercent = finalPos;
		}

		public void UpdateItemAt (int _index, object _data)
		{

			data [_index] = _data;

			foreach (GameObject unit in showPool) {
				
				SuperListCell cell = unit.GetComponent<SuperListCell> ();
				
				if (cell.index == _index) {
					
					cell.SetData (_data);

					break;
				}
			}
		}

		private void SetCurPercent (float _percent)
		{
			if (isVertical) {
				
				scrollRect.verticalNormalizedPosition = _percent;
				
			} else {
				
				scrollRect.horizontalNormalizedPosition = _percent;
			}
		}
	}
}
