using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using xy3d.tstd.lib.csv;

public class BattleBuff : MonoBehaviour {

	public Text buffText;
	public GameObject img;

	private HeroContainer heroContainer;

	private float buffTime;

	private BuffCsv csv;

	public void Init(HeroContainer _heroContainer,int _id,float _buffTime){

		heroContainer = _heroContainer;

		csv = StaticData.GetData<BuffCsv> (_id);

		buffText.text = csv.buffName;

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

			heroContainer.SetSpeed(0.5f);

			break;
		}
	}

	private void BuffRemove(ref float _deltaTime){

		switch (csv.ID) {
			
		case 1:

			_deltaTime = buffTime * 0.5f + (_deltaTime - buffTime);

			heroContainer.SetSpeed(1);
			
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
