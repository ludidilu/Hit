using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace xy3d.tstd.lib.superList{

	public class SuperListCell : MonoBehaviour,IPointerClickHandler {

		protected object data;
		protected bool selected;

		public int index;

		// Use this for initialization
		void Start () {

		}

		public void OnPointerClick (PointerEventData eventData)
		{
			SendMessageUpwards("CellClick",gameObject);
		}  
		
		// Update is called once per frame
		void Update () {
		
		}

		public virtual void SetData(object _data){

			data = _data;
		}

		public virtual void SetSelected(bool _value){

			selected = _value;
		}
	}
}
