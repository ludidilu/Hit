using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using xy3d.tstd.lib.superFunction;

public class BattleBuff : MonoBehaviour {

	public Text buffText;
	public Image img;

	private HeroContainer heroContainer;

	private float buffTime;

	private BuffCsv csv;

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

		switch (csv.ID) {

		case 1:

			heroContainer.SetSpeed(BattleConstData.SLOW_VALUE);

			break;

		case 2:

			heroContainer.SetSilent(true);

			break;

		case 3:

			heroContainer.SetBlood(true);

			break;

		case 4:
			
			heroContainer.SetDamageFix(BattleConstData.CRACK_VALUE);
			
			break;

		case 5:

			heroContainer.SetDamageFix(BattleConstData.PROTECT_VALUE);
			
			break;
		}
	}

	private void BuffRemove(ref float _deltaTime){

		switch (csv.ID) {
			
		case 1:

			_deltaTime = buffTime / BattleConstData.SLOW_VALUE + (_deltaTime - buffTime);

			heroContainer.SetSpeed(1 / BattleConstData.SLOW_VALUE);
			
			break;

		case 2:
			
			heroContainer.SetSilent(false);
			
			break;

		case 3:
			
			heroContainer.SetBlood(false);
			
			break;

		case 4:
			
			heroContainer.SetDamageFix(1 / BattleConstData.CRACK_VALUE);
			
			break;

		case 5:
			
			heroContainer.SetDamageFix(1 / BattleConstData.PROTECT_VALUE);
			
			break;
		}
	}

	public bool PassTime(ref float _deltaTime){

		if(buffTime <= _deltaTime){
			
			BuffRemove(ref _deltaTime);
			
			return true;
			
		}else{
			
			buffTime = buffTime - _deltaTime;
			
			RefreshScale();

			return false;
		}
	}

	// Use this for initialization
	void Start () {
	
		RefreshScale ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
