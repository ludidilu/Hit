using UnityEngine;
using System.Collections;
using System.Timers;

namespace xy3d.tstd.lib.publicTools
{

	public class ActorLightningUnit
	{
	    private GameObject effectGO;
	    private GameObject[] effectList;
	    private GameObject[] targetList;
	    private Vector3 startPos;
	    private int index;
	    private float width = 0.2f;


	    public ActorLightningUnit(GameObject _effectGO, GameObject[] _targetList, Vector3 _startPos)
	    {
	        startPos = _startPos;
	        effectGO = _effectGO;
	        targetList = _targetList;
	        effectList = new GameObject[targetList.Length];
	        for (int i = 0; i < _targetList.Length; i++)
	        {
	            createEffect();
	        }
	    }


	    private void createEffect()
	    {
	        GameObject effect = GameObject.Instantiate(effectGO);
	        effectList[index] = effect;
	        
	        GameObject target = targetList[index];
	        index++;
	        Vector3 endPos= target.transform.position;
	        effect.transform.position = new Vector3((endPos.x + startPos.x) / 2, (endPos.y + startPos.y) / 2, (endPos.z + startPos.z) / 2);
	        //effect.transform.position = startPos;
	        effect.transform.LookAt(endPos);
	        effect.transform.Rotate(new Vector3(0, 90, 0));
	        float distance = Vector3.Distance(startPos, endPos);
//	        Renderer effectRender = effect.transform.GetComponent<Renderer>();
	        float effectScale = distance / width;
	        effect.transform.localScale = new Vector3(effectScale,1,1);
	        startPos = endPos;
	    }

	    public void destroy()
	    {

	    }
	}
}
