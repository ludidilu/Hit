using UnityEngine;
using System.Collections.Generic;

public class BuffSort : IComparer<BattleBuff> {

	private static BuffSort _Instance;

	public static BuffSort Instance{

		get{

			if (_Instance == null) {

				_Instance = new BuffSort ();
			}

			return _Instance;
		}
	}

	public int Compare (BattleBuff x, BattleBuff y){

		if (x.buffTime < y.buffTime) {

			return -1;

		} else {

			return 1;
		}
	}
}
