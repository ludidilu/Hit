using UnityEngine;
using System.Collections;

public class BaseContainer : MonoBehaviour {
	
	public HeroContainer heroContainer;

	// Use this for initialization
	void Start () {
		
		(transform as RectTransform).offsetMax = new Vector2((heroContainer.transform as RectTransform).rect.width,0);
		(transform as RectTransform).offsetMin = new Vector2(0,-(heroContainer.transform as RectTransform).rect.height);
	}

	public void Move(float _deltaTime){
		
		(transform as RectTransform).anchoredPosition = new Vector2((transform as RectTransform).anchoredPosition.x - _deltaTime / BattleConstData.MAX_TIME * (transform as RectTransform).rect.width,(transform as RectTransform).anchoredPosition.y);
	}

	public virtual void SetScale(){
		
		if (transform.childCount > 0) {
			
			(transform as RectTransform).anchoredPosition = new Vector2((transform as RectTransform).anchoredPosition.x / heroContainer.speed / transform.localScale.x,(transform as RectTransform).anchoredPosition.y);
		}
		
		transform.localScale = new Vector3 (1 / heroContainer.speed, 1, 1);
	}

	public virtual void Clear(){
		
		while (transform.childCount > 0) {
			
			GameObject go = transform.GetChild (0).gameObject;
			
			go.transform.SetParent(null);
			
			GameObject.Destroy(go);
		}
	}
}
