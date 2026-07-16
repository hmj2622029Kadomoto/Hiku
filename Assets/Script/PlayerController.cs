using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float moveSpeed = 5f;
	[SerializeField] float attackRange = 2f;
	[SerializeField] float attackRadius = 1f;
	[SerializeField] int attackPower = 1;
	Rigidbody rbody;
	Animator animator;
	bool canMove = true;
	bool isDead = false;
	bool isInvincible = false;
	int hp = 10;

	enum PlayerState { Idle,Move,Attack,Damage,Dead}
	private void Start()
	{
		rbody = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
	}
	private void FixedUpdate()
	{
		if(!canMove)
		{
			rbody.linearVelocity = Vector3.zero;
			return;
		}
		Vector3 move = Vector3.zero;
		if (Keyboard.current.wKey.isPressed)
			move += Vector3.forward;
		if (Keyboard.current.sKey.isPressed)
			move += Vector3.back;
		if (Keyboard.current.aKey.isPressed)
			move += Vector3.left;
		if (Keyboard.current.dKey.isPressed)
			move += Vector3.right;
		move.Normalize();
		rbody.linearVelocity = new Vector3(move.x * moveSpeed,rbody.linearVelocity.y,move.z*moveSpeed);
		if(move != Vector3.zero)
		{
			transform.forward = move;
		}
		animator.SetBool("Run", move != Vector3.zero);
	}
	private void Update()
	{
		if(Mouse.current.leftButton.wasPressedThisFrame && canMove)
		{
			canMove = false;
			animator.SetTrigger("AttackR");
		}
		if(Mouse.current.rightButton.wasPressedThisFrame && canMove)
		{
			canMove = false;
			animator.SetTrigger("Attack2H");
		}
	}
	public void Damage()
	{
		animator.SetTrigger("Damage");
	}
	public void Death()
	{
		animator.SetTrigger("Death");
	}
	public void AttackEnd()
	{
		canMove = true;
	}
	public void AttackHit()
	{
		Vector3 center = transform.position+transform.forward * attackRange;
		Collider[] hitEnemies = Physics.OverlapSphere(center, attackRadius);

		foreach (Collider hit in hitEnemies)
		{
			ZombieController zombie = hit.GetComponent<ZombieController>();
			if (zombie != null)
			{
				zombie.TakeDamage(attackPower);
			}
		}
	}
	public void TakeDamage(int damage)
	{
		if (isDead || isInvincible)
			return;
		hp -= damage;
		canMove = true;
		animator.SetTrigger("Damage");
		if(hp<=0)
		{
			isDead = true;
			animator.SetTrigger("Death");
		}
	}
}
