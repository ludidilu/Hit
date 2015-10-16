using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using xy3d.tstd.lib.superFunction;

public class BattleBuff : MonoBehaviour {

	public Text buffText;
	public Image img;

	private HeroContainer heroContainer;

	[HideInInspector] public float buffTime;

	public BuffCsv csv;

	public void Init(HeroContainer _heroContainer,int _id,float _buffTime){

		heroContainer = _heroContainer;

		csv = StaticData.GetData<BuffCsv> (_id);

		buffText.text = csv.buffName;

		img.color = csv.harm ? Color.red : Color.green;

		buffTime = _buffTime;

		BuffAdd ();
	}

	public void AddTime(float _time){

		buffTime = buffTime + _time;

		RefreshScale ();
	}

	private void RefreshScale(){

		img.transform.localScale = new Vector3 ((heroContainer.transform as RectTransform).rect.width / (transform as RectTransform).rect.width * buffTime / BattleConstData.MAX_TIME, 1, 1);
	}

	private void BuffAdd(){

		if (csv.speedFix != 1) {

			heroContainer.SetSpeed(csv.speedFix);
		}

		if (csv.damageFix != 1) {

			heroContainer.SetDamageFix (csv.damageFix);
		}

		if (csv.silent) {

			heroContainer.SetSilent (true);
		}

		if (csv.blood) {

			heroContainer.SetBlood (true);
		}
	}

	public void BuffRemove(){

		if (csv.speedFix != 1) {
			
			heroContainer.SetSpeed(1 / csv.speedFix);
		}
		
		if (csv.damageFix != 1) {
			
			heroContainer.SetDamageFix (1 / csv.damageFix);
		}
		
		if (csv.silent) {
			
			heroContainer.SetSilent (false);
		}
		
		if (csv.blood) {
			
			heroContainer.SetBlood (false);
		}
	}

	public void PassTime(float _deltaTime){

		buffTime = buffTime - _deltaTime;
		
		RefreshScale();
	}

	// Use this for initialization
	void Start () {
	
		RefreshScale ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
