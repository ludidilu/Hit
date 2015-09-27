using System;
using System.Collections.Generic;
using UnityEngine;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleDamageNumGroup
    {

        private const string picStr = "1234567890-";
	
		private const float criticalScale = 2;
		
		public int drawIndex;
		
		public int index;

        public BattleDamageNumUnit[] unitVec;
		
		public float alpha = 0;

        public Vector3 pos = new Vector3();
        public Vector3 nonePos = new Vector3();
        private Vector3 scale = new Vector3(0.5f, 0.5f, 0.5f);
        public Quaternion rotation = Quaternion.Euler(0, 0, 0);
        private Matrix4x4 matrix = new Matrix4x4();

		
		private float groupWidth;
		
		public BattleDamageNumGroup preGroup;
		public BattleDamageNumGroup nextGroup;

        public BattleDamageNum bdm;

        public float nowScale = 1;

		
		public BattleDamageNumGroup(int _drawIndex, int _index)
		{
			drawIndex = _drawIndex;
			index = _index;
            
		}

        public void init(string _str, int _color)
        {
			int i,m,n;
			n = 0;
            unitVec = new BattleDamageNumUnit[_str.Length];
            
			for(i = 0 ; i < _str.Length ; i++){

                unitVec[i] = bdm.getDamageUnit(this);
				
				m = picStr.IndexOf(_str[i]);
				
				unitVec[i].uFix = m * BattleDamageNum.FONT_WIDTH / BattleDamageNum.ASSET_WIDTH;

                unitVec[i].vFix = -_color * BattleDamageNum.FONT_HEIGHT / BattleDamageNum.ASSET_HEIGHT;
				
				n++;
			}
			
			groupWidth = n * BattleDamageNum.FONT_WIDTH;
			
			for(i = 0 ; i < _str.Length ; i++){
				
				unitVec[i].xFix = -groupWidth * 0.5f + BattleDamageNum.FONT_WIDTH * 0.5f + i * BattleDamageNum.FONT_WIDTH;
			}
			
			if(_color == 2){
				
				SetScale(criticalScale);
				
			}else{

                SetScale(1);
			}
		}

        public void SetScale(float s)
        {
            nowScale = scale.x = scale.y = scale.z = s;
        }

        public Vector4 GetPositionsVec()
        {
            Vector4 vec = new Vector4();
            if (alpha > 0)
            {
                vec.x = pos.x;
                vec.y = pos.y;
                vec.z = pos.z;
                vec.w = alpha;
            }
            return vec;
        }

        public Matrix4x4 GetMatrix()
        {

            if (alpha > 0)
            {
                matrix.SetTRS(nonePos, rotation, scale);
            }
            return matrix;
        }

    }
}
