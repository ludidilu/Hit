using System;
using UnityEngine;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleDamageNumUnit
    {
        public float uFix = 0;
		public float vFix = 0;
		public float xFix = 0;
		
		public float groupIndex = 10;

        private Vector4 vec = new Vector4();
		
		public BattleDamageNumUnit nextUnit;

        private int state;

        public int State
        {
            get { return state; }
            set { state = value; }
        }

        private bool isChange = true;

        public bool IsChange
        {
            get { return isChange; }
            set { isChange = value; }
        }
		
		public BattleDamageNumUnit()
		{
			
		}
		
		public void SetGroup(int _groupIndex){
			
			groupIndex = _groupIndex;
		}

        public Vector4 GetFix()
        {
            vec.x = xFix;
            vec.y = uFix;
            vec.z = vFix;
            vec.w = groupIndex;
            return vec;
        }
    }
}
