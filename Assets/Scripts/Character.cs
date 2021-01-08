using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum State
    {
        Idle,
        RunningToEnemy,
        RunningFromEnemy,
        BeginAttack,
        Attack,
        BeginShoot,
        Shoot,
        Dead,
        BeginHandAtack,
        HandAtack,
    }

    public enum Weapon
    {
        Pistol,
        Bat,
        Hand,
    }

    Animator animator;
    State state;

    public Weapon weapon;
    public Character target;
    public float runSpeed;
    public float distanceFromEnemy;
    Vector3 originalPosition;
    Quaternion originalRotation;
    bool policmenDead;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        state = State.Idle;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void SetState(State newState)
    {
        state = newState;
    }

    [ContextMenu("Attack")]
    void AttackEnemy()
    {
        if (policmenDead) 
        {
            Debug.LogWarning("Policmen is dead");
        }
        if (state == State.Dead)
        {
            Debug.LogWarning("Policmen cannot attack");
            return;
        }
        switch (weapon)
        {
            case Weapon.Hand:
                state = State.BeginHandAtack;
                break;
            case Weapon.Bat:
                state = State.RunningToEnemy;
                break;
            case Weapon.Pistol:
                state = State.BeginShoot;
                break;
        }
    }

    void FixedUpdate()
    {
        switch (state) {
            case State.Idle:
                transform.rotation = originalRotation;
                animator.SetFloat("Speed", 0.0f);
                break;

            case State.RunningToEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(target.transform.position, distanceFromEnemy))
                    state = State.BeginAttack;
                break;

            case State.RunningFromEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(originalPosition, 0.0f))
                    state = State.Idle;
                break;

            case State.BeginAttack:
                animator.SetTrigger("MeleeAttack"); 
                    state = State.Attack;
                break;

            case State.Attack:
                break;

            case State.BeginShoot:
                animator.SetTrigger("Shoot");
                state = State.Shoot;
                break;

            case State.Shoot:
                break;
            
            case State.BeginHandAtack:
                animator.SetTrigger("Hand");
                state = State.HandAtack;
                break;
            
            case State.HandAtack:
                break;
            
            case  State.Dead:
                animator.SetBool("Dead", true);
                break;
        }
    }

    public void DeadPolicmen()
    {
        if (target.state != State.Dead)
        {
            target.SetState(State.Dead);
            policmenDead = true;
        }
    }

    bool RunTowards(Vector3 targetPosition, float distanceFromTarget)
        {
            Vector3 distance = targetPosition - transform.position;
            if (distance.magnitude < 0.00001f) {
                transform.position = targetPosition;
                return true;
            }
    
            Vector3 direction = distance.normalized;
            transform.rotation = Quaternion.LookRotation(direction);
    
            targetPosition -= direction * distanceFromTarget;
            distance = (targetPosition - transform.position);
    
            Vector3 step = direction * runSpeed;
            if (step.magnitude < distance.magnitude) {
                transform.position += step;
                return false;
            }
    
            transform.position = targetPosition;
            return true;
        }
}
