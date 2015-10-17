using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superTween;

public class BattleHpBar : MonoBehaviour {

	public HeroContainer heroContainer;

	public RectTransform bar;
	public RectTransform barFollow;

	public void Init(){

		bar.localScale = new Vector3 (1, 1, 1);
		barFollow.localScale = new Vector3 (1, 1, 1);
	}

	public void SetScale(float _percent){

		bar.localScale = new Vector3 (_percent, 1, 1);

		SuperTween.Instance.To (barFollow.localScale.x, _percent, 1, BarFollowChange, null);
	}

	private void BarFollowChange(float _percent){

		barFollow.localScale = new Vector3 (_percent, 1, 1);
	}

	// Use this for initialization
	void Start () {
	
		bar.offsetMax = new Vector2((heroContainer.transform as RectTransform).rect.width,0);
		barFollow.offsetMax = new Vector2((heroContainer.transform as RectTransform).rect.width,0);
	}
}
