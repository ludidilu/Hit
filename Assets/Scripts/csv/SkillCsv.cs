using UnityEngine;
using System.Collections;

[System.Serializable]
public class SkillCsv : CsvBase {

	public float[] time;
	public int[] type;
	public float[] hitTime;
	public int[] hitID;

	public float allTime;

	public override void Fix ()
	{
		foreach (float tmpTime in time) {

			allTime = allTime + tmpTime;
		}
	} 
}
