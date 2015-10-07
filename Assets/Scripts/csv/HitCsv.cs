using UnityEngine;
using System.Collections;

[System.Serializable]
public class HitCsv : CsvBase {

	public bool interrupt;
	public int damage;
	public int[] buff;
	public float[] buffTime;
}
