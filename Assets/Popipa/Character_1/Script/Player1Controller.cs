using UnityEngine;

public class Player1Controller : MonoBehaviour
{
	public Animator anim;
	public void Jump()
    {
		anim.SetBool("Jump", true) ;
	}

	public void JumpOff()
    {
		anim.SetBool("Jump", false);
	}

	public void Dead()
	{
		anim.SetBool("Dead" , true);
	}

	public void DeadOff()
	{
		anim.SetBool("Dead", false);
	}
	public void Walk()
	{
		anim.SetBool("Walk" , true);
	}

	public void WalkOff()
	{
		anim.SetBool("Walk", false);
	}
	public void Run()
	{
		anim.SetTrigger("Run");
	}
	public void RunOff()
	{
		anim.SetBool("Run", false);
	}
	public void Attack()
	{
		anim.SetTrigger("Attack");
	}
	public void AttackOff()
	{
		anim.SetBool("Attack", false);
	}
}



