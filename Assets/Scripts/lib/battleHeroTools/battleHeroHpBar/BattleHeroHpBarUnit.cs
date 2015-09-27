using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using xy3d.tstd.lib.superTween;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleHeroHpBarUnit
    {
        public static float ambientNormal = 1;

        private const float hpChangeTime = 1;//血条变化需要时间

        public float alpha = 0;
        public Vector3 pos = new Vector3();
        public Vector3 scale = new Vector3(0.3f, 0.3f, 0.3f);
        public Quaternion rotation = Quaternion.Euler(0, 0, 0);  
        private Matrix4x4 matrix = new Matrix4x4();
        

        public float angerUFix;
        public float angerXFix;

        public float hpUFix;
        public float hpXFix;

//        private float targetHp;
        private float _hp;
        private float maxHp;

        private GameObject go;

        public BattleHeroHpBarUnit()
        {

        }

        public float Hp
        {
            get
            {
                return _hp;
            }
            set
            {
                hpUFix = (value / maxHp - 1) * BattleHeroHpBar.hpBarWidth / BattleHeroHpBar.TEXTURE_WIDTH;
                hpXFix = (value / maxHp - 1) * BattleHeroHpBar.hpBarWidth;

                _hp = value;
            }
        }

        public void SetHp(float _targetHp)
        {
//            targetHp = _targetHp;
            SuperTween.Instance.To(Hp, _targetHp, hpChangeTime, UpdateHp, null);
        }

        private void UpdateHp(float value)
        {
            Hp = value;
        }

        public void Init(float _nowhp, float _maxHp, int _nowAnger, GameObject _go)
        {
            maxHp = _maxHp;
            Hp = _nowhp;
            go = _go;
            updateAnger(_nowAnger);
        }

        public Vector4 GetPositionsVec()
        {
            Vector4 vec = new Vector4();
            if (go != null)
            {
                vec.x = go.transform.position.x ;
                vec.y = go.transform.position.y + 1.8f;
                vec.z = go.transform.position.z;
                vec.w = alpha;
            }
            return vec;
        }

        public Matrix4x4 GetMatrix()
        {
            
            if (go != null)
            {
                matrix.SetTRS(pos, rotation, scale);
            }
            return matrix;
        }

        public Vector4 GetFixVec()
        {
            Vector4 vec = new Vector4();
            if (go != null)
            {
                vec.x = hpUFix;
                vec.y = hpXFix;
                vec.z = angerUFix;
                vec.w = angerXFix;
            }
            return vec;
        }

        public void resetHp(float _nowHp, float _maxHp)
        {
            maxHp = _maxHp;
            Hp = _nowHp;
        }

        public void updateAnger(float value)
        {
            angerUFix = (value / 10 - 1) * BattleHeroHpBar.angerBarWidth / BattleHeroHpBar.TEXTURE_WIDTH;
            angerXFix = (value / 10 - 1) * BattleHeroHpBar.angerBarWidth;

//            angerNum = value;
        }
    }
}