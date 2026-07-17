using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
	[SerializeField] float speed = 2f;
	[SerializeField] Transform player;
	[SerializeField] float attackRange = 2f;
	[SerializeField] float attackInterval = 1.5f;
	[SerializeField] int attackPower = 3;
	[SerializeField] int maxHP = 3;
	int hp;
	bool isAttacking = false;
	bool canMove = true;
	float attackTimer = 0f;

	Rigidbody rbody;
	Animator animator;
	
	private void Awake()
	{
		rbody = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		hp = maxHP;
	}

	private void FixedUpdate()
	{
		if (player == null) { return; }
		Vector3 diff = player.position - transform.position;
		diff.y = 0f;
		float distance = diff.magnitude;
		if (distance > attackRange)
		{
			if (canMove)
			{
				Vector3 dir = diff.normalized;
				rbody.linearVelocity = new Vector3(dir.x * speed, rbody.linearVelocity.y, dir.z * speed);
				transform.forward = dir;
				animator.SetBool("ZWalk", true);
			}
		}
		else
		{
			if (!isAttacking)
			{
				isAttacking = true;
				rbody.linearVelocity = new Vector3(0f,rbody.linearVelocity.y,0f);
				animator.SetBool("ZWalk", false);
				animator.SetTrigger("ZAttack");
			}
		}

	}

	public void SetPlayer(Transform target)
	{
		player = target;
	}

	public void TakeDamage(int damage)
	{
		hp -= damage;
		if (hp <= 0)
		{
			Destroy(gameObject);
		}
		else
		{
			ZombieDamage();
		}
	}

	public void ZombieAttackHit()
	{
		float distance = Vector3.Distance(transform.position, player.position);
		if(distance <= attackRange+0.3f)
		{
			player.GetComponent<PlayerController>().TakeDamage(attackPower);
		}
	}

	public void ZombieAttackEnd()
	{
		isAttacking = false;
		canMove = true;
	}

	public void ZombieDamage()
	{
		animator.SetTrigger("ZDamage");
		canMove = true;
	}
}
