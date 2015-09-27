using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superFunction;

namespace xy3d.tstd.lib.heroFactory{

	[SharedBetweenAnimators]
	//这个组件是绑在AnimatorController的各个state上的
	public class AnimationBehaviour : StateMachineBehaviour
	{

	    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    {
	        
			SuperEvent superEvent = new SuperEvent("poseStart");
	        SuperFunction.Instance.DispatchEvent(animator.gameObject, superEvent);
	    }

	    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    //{

	    //}

	    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    {
//            HeroController hc = animator.gameObject.GetComponent<HeroController>();
//            SuperEvent superEvent = new SuperEvent("poseStop");
//	        SuperFunction.Instance.DispatchEvent(animator.gameObject, superEvent);
	    }

	    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    //{

	    //}

	    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	    //{

	    //}
	}
}