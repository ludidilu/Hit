using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.csv;

public class BattleAI{

	private NpcCsv csv;

	public void Init(int _id){

		csv = StaticData.GetData<NpcCsv> (_id);
	}

	public int CastSkill(){

		if (Random.Range (0, 120) < 1) {

			return csv.skill [(int)(csv.skill.Length * Random.value)];

		} else {

			return -1;
		}
	}

}
