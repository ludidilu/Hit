using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarContainer : BaseContainer {



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
	}


}
