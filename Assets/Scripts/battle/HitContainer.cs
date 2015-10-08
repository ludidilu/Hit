using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using xy3d.tstd.lib.csv;
using xy3d.tstd.lib.gameObjectFactory;
using xy3d.tstd.lib.textureFactory;

public class HitContainer : BaseContainer {

	private List<GameObject> hits = new List<GameObject>();

	public void Init(){

		Clear ();
		
		(transform as RectTransform).anchoredPosition = new Vector2 ();

		for (int i = 0; i < heroContainer.csv.hitID.Length; i++) {

			HitCsv csv = StaticData.GetData<HitCsv>(heroContainer.csv.hitID[i]);

			GameObject unit = GameObjectFactory.Instance.GetGameObject("Assets/Prefabs/Hit.prefab", null, true);
			
			Image img = unit.GetComponent<Image>();
			
			unit.transform.SetParent(transform,false);

			img.rectTransform.offsetMax = new Vector2(img.rectTransform.rect.width / img.rectTransform.rect.height * (transform as RectTransform).rect.height,0);
			img.rectTransform.offsetMin = new Vector2(0,-(transform as RectTransform).rect.height);
			
			img.rectTransform.anchoredPosition = new Vector2(heroContainer.csv.hitTime[i] / BattleConstData.MAX_TIME * (transform as RectTransform).rect.width,0);

			if(csv.interrupt){

				img.overrideSprite = TextureFactory.Instance.GetTexture<Sprite> ("Assets/Textures/cangtianyudijian.png", null, true);

			}else{

				img.overrideSprite = TextureFactory.Instance.GetTexture<Sprite> ("Assets/Textures/binghuangyanfengzhang.png", null, true);
			}

			unit.transform.localScale = new Vector3(heroContainer.speed,1,1);
			
			hits.Add(unit);
		}
	}

	public override void SetScale(){

		base.SetScale ();

		for (int i = 0 ; i < hits.Count ; i++) {
				
			hits[i].transform.localScale = new Vector3(heroContainer.speed,1,1);
		}
	}

	public void HitOver(){

		GameObject hit = hits [0];

		hits.RemoveAt (0);

		GameObject.Destroy (hit);
	}

	public override void Clear(){
		
		base.Clear ();

		if (hits.Count > 0) {

			hits.Clear();
		}
	}
}
