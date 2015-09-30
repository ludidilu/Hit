using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.gameObjectFactory;

public class MyHeroContainer : HeroContainer {

	public override void AddBuff(int _buffID,float _buffTime){
		
		if (buffDic.ContainsKey (_buffID)) {
			
			BattleBuff buff = buffDic[_buffID];
			
			buff.AddTime(_buffTime);
			
		} else {
			
			GameObject go = GameObjectFactory.Instance.GetGameObject("Assets/Prefabs/Buff.prefab",null,true);
			
			BattleBuff buff = go.GetComponent<BattleBuff> ();
			
			buff.Init (this, _buffID, _buffTime);
			
			go.transform.SetParent(buffRectTransform,false);
			
			(go.transform as RectTransform).anchoredPosition = new Vector2((go.transform as RectTransform).anchoredPosition.x,(1 + buffDic.Count) * (go.transform as RectTransform).rect.height);
			
			buffDic.Add (_buffID, buff);
			
			buffList.Add (go);
		}
	}
	
	public override void RemoveBuff(int _buffID){
		
		GameObject go = buffDic [_buffID].gameObject;
		
		buffDic.Remove (_buffID);
		
		int tmpIndex = buffList.IndexOf (go);
		
		buffList.RemoveAt (tmpIndex);
		
		for (int i = tmpIndex; i < buffList.Count; i++) {
			
			(buffList[i].transform as RectTransform).anchoredPosition = new Vector2((buffList[i].transform as RectTransform).anchoredPosition.x,(buffList[i].transform as RectTransform).anchoredPosition.y - (buffList[i].transform as RectTransform).rect.height);
		}
		
		GameObject.Destroy (go);
	}
}
