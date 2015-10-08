using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using xy3d.tstd.lib.gameObjectFactory;

public class BarContainer : BaseContainer {

	public void Init(){
		
		Clear ();
		
		(transform as RectTransform).anchoredPosition = new Vector2 ();

		float allWidth = 0;
		
		for (int i = 0; i < heroContainer.csv.time.Length; i++) {

			GameObject unit = GameObjectFactory.Instance.GetGameObject("Assets/Prefabs/Bar.prefab", null, true);

			Image img = unit.GetComponent<Image>();
			
			unit.transform.SetParent(transform,false);

			float width = heroContainer.csv.time[i] / BattleConstData.MAX_TIME * (transform as RectTransform).rect.width;
			
			img.rectTransform.offsetMax = new Vector2(width,0);
			img.rectTransform.offsetMin = new Vector2(0,-(transform as RectTransform).rect.height);
			
			img.rectTransform.anchoredPosition = new Vector2(allWidth,0);
			
			allWidth = allWidth + width;
			
			img.color = BattleConstData.COLOR_ARR[heroContainer.csv.type[i]];
			
//			img.raycastTarget = false;
		}
	}


}
