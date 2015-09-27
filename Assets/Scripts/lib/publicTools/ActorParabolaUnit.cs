using UnityEngine;
using System.Collections;
using System;

namespace xy3d.tstd.lib.publicTools
{
	class ActorParabolaUnit
	{
	    private GameObject effectIns;

	    private Vector3 startPos;
	    private Vector3 endPos;

	    private Vector3 lastPos;
	    private Vector3 newPos;

	    private float distance;
//	    private float percent;
	    private Vector3 interval;

	    private Vector3 v;


	    public ActorParabolaUnit(GameObject _effectGO, Vector3 _startPos, Vector3 _endPos, Vector3 _interval)
	    {
	        effectIns = _effectGO;

            effectIns.transform.position = _startPos;

	        startPos = new Vector3(effectIns.transform.position.x, effectIns.transform.position.y, effectIns.transform.position.z);

	        lastPos = new Vector3(startPos.x, startPos.y, startPos.z);

	        endPos = _endPos;

	        distance = Vector3.Distance(startPos, endPos) / 12.0f;
	        interval = _interval;

	        v.x = (endPos.x - startPos.x) / distance + interval.x * distance * 0.5f;
	        v.y = (endPos.y - startPos.y) / distance + interval.y * distance * 0.5f;
	        
	    }

	    public void SetPercent(float value)
	    {
	        newPos.x = startPos.x + (v.x * 2 - interval.x * distance * value) * distance * value * 0.5f;
	        newPos.z = startPos.z + (endPos.z - startPos.z) * value;

	        newPos.y = startPos.y + (v.y * 2 - interval.y * distance * value) * distance * value * 0.5f;
	        effectIns.transform.LookAt(newPos);
	        lastPos = new Vector3(newPos.x, newPos.y, newPos.z);
	        effectIns.transform.position = lastPos;
	        effectIns.transform.Rotate(new Vector3(0, 270, 0));
	        
//	        percent = value;
	    }

	    public void Destroy()
	    {
	        GameObject.Destroy(effectIns);
	        effectIns = null;
	    }


	    //三角形
	   /* public void SetPercent(float value, float f)
	    {
	        vx = (endPos.x - startPos.x);
	        if (value >= 0.5f)
	        {
	            newPos.x = startPos.x + vx * value + gx * (1 - value) / 4;
	        }
	        else
	        {
	            newPos.x = startPos.x + vx * value + gx * value / 4;
	        }


	        newPos.z = startPos.z + (endPos.z - startPos.z) * value;

	        newPos.y = startPos.y + (endPos.y - startPos.y) * value + 2.0f;
	        target.transform.LookAt(newPos);
	        lastPos = new Vector3(newPos.x, newPos.y, newPos.z);
	        target.transform.position = lastPos;
	        target.transform.Rotate(new Vector3(0, 270, 0));

	        percent = value;
	    }*/
	}
}