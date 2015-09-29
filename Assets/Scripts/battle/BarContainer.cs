using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarContainer : MonoBehaviour {

	public HeroContainer heroContainer;

	private GameObject[] hits = new GameObject[0];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Init(){
		
		Clear ();
		
		(transform as RectTransform).anchoredPosition = new Vector2 ();

		float allWidth = 0;
		
		for (int i = 0; i < heroContainer.csv.time.Length; i++) {
			
			GameObject unit = new GameObject();
			
			unit.layer = gameObject.layer;
			
			Image img = unit.AddComponent<Image>();
			
			img.transform.SetParent(transform,false);
			
			img.rectTransform.anchorMax = new Vector2(0,1);
			img.rectTransform.anchorMin = new Vector2(0,1);
			
			img.rectTransform.pivot = new Vector2(0,1);
			
			float width = heroContainer.csv.time[i] / BattleConstData.MAX_TIME * (transform as RectTransform).rect.width;
			
			img.rectTransform.offsetMax = new Vector2(width,0);
			img.rectTransform.offsetMin = new Vector2(0,-(transform as RectTransform).rect.height);
			
			img.rectTransform.anchoredPosition = new Vector2(allWidth,0);
			
			allWidth = allWidth + width;
			
			img.color = BattleConstData.COLOR_ARR[heroContainer.csv.type[i]];
			
			img.raycastTarget = false;
		}

		hits = new GameObject[heroContainer.csv.hitID.Length];
		
		for (int i = 0; i < heroContainer.csv.hitID.Length; i++) {
			
			GameObject unit = new GameObject();
			
			unit.layer = gameObject.layer;
			
			Image img = unit.AddComponent<Image>();
			
			img.transform.SetParent(transform,false);
			
			img.rectTransform.anchorMax = new Vector2(0,1);
			img.rectTransform.anchorMin = new Vector2(0,1);
			
			img.rectTransform.pivot = new Vector2(0.5f,1f);
			
			img.rectTransform.offsetMax = new Vector2(4,0);
			img.rectTransform.offsetMin = new Vector2(0,-(transform as RectTransform).rect.height);

			img.rectTransform.anchoredPosition = new Vector2(heroContainer.csv.hitTime[i] / BattleConstData.MAX_TIME * (transform as RectTransform).rect.width ,0);
			
			img.rectTransform.localScale = new Vector3(1 / transform.localScale.x,1,1);
			
			img.color = Color.black;
			
			img.raycastTarget = false;
			
			hits[i] = unit;
		}
	}
	
	public void SetScale(float _scale){

		if (transform.childCount > 0) {

			(transform as RectTransform).anchoredPosition = new Vector2((transform as RectTransform).anchoredPosition.x * _scale / transform.localScale.x,0);
		
			for (int i = 0; i < hits.Length; i++) {

				GameObject hit = hits [i];

				hit.transform.localScale = new Vector3 (1 / _scale, 1, 1);
			}
		}

		transform.localScale = new Vector3 (_scale, 1, 1);
	}
	
	public void Clear(){

		while (transform.childCount > 0) {
			
			GameObject go = transform.GetChild (0).gameObject;
			
			go.transform.SetParent(null);
			
			GameObject.Destroy(go);
		}
	}
}
