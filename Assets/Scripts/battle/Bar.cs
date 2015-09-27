using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.csv;
using UnityEngine.UI;

public class Bar : MonoBehaviour {

	public static float MAX_TIME = 6;
	private static Color[] COLOR_ARR = new Color[]{Color.green,Color.yellow,Color.red};

	public GameObject bar;

	[HideInInspector]public SkillCsv csv;

	private GameObject[] hits;

	// Use this for initialization
	void Start () {
	
	}

	public void Init(int _skillID){

		Clear ();

		(bar.transform as RectTransform).anchoredPosition = new Vector2 ();

		csv = StaticData.GetData<SkillCsv> (_skillID);

		float pos = 0;

		for (int i = 0; i < csv.time.Length; i++) {

			GameObject unit = new GameObject();

			Image img = unit.AddComponent<Image>();

			img.transform.SetParent(bar.transform,false);

			img.rectTransform.anchorMax = new Vector2(0,1);
			img.rectTransform.anchorMin = new Vector2(0,1);

			img.rectTransform.pivot = new Vector2(0,1);

			float width = csv.time[i] / MAX_TIME * (transform as RectTransform).rect.width;

			img.rectTransform.offsetMax = new Vector2(width,0);
			img.rectTransform.offsetMin = new Vector2(0,-(transform as RectTransform).rect.height);

			img.rectTransform.anchoredPosition = new Vector2(pos,0);

			pos = pos + width;

			img.color = COLOR_ARR[csv.type[i]];

			img.raycastTarget = false;
		}

		hits = new GameObject[csv.hitID.Length];

		for (int i = 0; i < csv.hitID.Length; i++) {

			GameObject unit = new GameObject();
			
			Image img = unit.AddComponent<Image>();
			
			img.transform.SetParent(bar.transform,false);
			
			img.rectTransform.anchorMax = new Vector2(0,1);
			img.rectTransform.anchorMin = new Vector2(0,1);
			
			img.rectTransform.pivot = new Vector2(0.5f,1f);

			img.rectTransform.offsetMax = new Vector2(4,0);
			img.rectTransform.offsetMin = new Vector2(0,-(transform as RectTransform).rect.height);

//			img.rectTransform.anchoredPosition = new Vector2(csv.hitTime[i] / (bar.transform as RectTransform).localScale.x / MAX_TIME * (transform as RectTransform).rect.width ,0);

			img.rectTransform.anchoredPosition = new Vector2(csv.hitTime[i] / MAX_TIME * (transform as RectTransform).rect.width ,0);

			img.rectTransform.localScale = new Vector3(1 / bar.transform.localScale.x,1,1);

			img.color = Color.black;
			
			img.raycastTarget = false;

			hits[i] = unit;
		}
	}

	public void SetScale(float _scale){

		bar.transform.localScale = new Vector3 (1 / _scale, 1, 1);

		for(int i = 0 ; i < hits.Length ; i++){

			GameObject hit = hits[i];

//			(hit.transform as RectTransform).anchoredPosition = new Vector2(csv.hitTime[i] / (bar.transform as RectTransform).localScale.x / MAX_TIME * (transform as RectTransform).rect.width ,0);

			(hit.transform as RectTransform).anchoredPosition = new Vector2(csv.hitTime[i] / MAX_TIME * (transform as RectTransform).rect.width ,0);

			hit.transform.localScale = new Vector3(1 / bar.transform.localScale.x,1,1);
		}
	}

	public void Clear(){

		csv = null;

		while (bar.transform.childCount > 0) {

			GameObject go = bar.transform.GetChild (0).gameObject;

			go.transform.SetParent(null);

			GameObject.Destroy(go);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
