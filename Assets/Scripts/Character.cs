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
        BeginDead,
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
    public Transform target;
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
        if (IsDeadCharacter())
        {
            return;
        }
        state = newState;
    }

    [ContextMenu("Attack")]
    void AttackEnemy()
    {
        if (IsDeadCharacter())
        {
           return;
        }

        Character targetCharacter = target.GetComponent<Character>();
        if (targetCharacter.IsDeadCharacter())
        {
            return;
        }
        switch (weapon)
        {
            case Weapon.Hand:
                state = State.RunningToEnemy;
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
                    switch (weapon)
                    {
                        case Weapon.Bat:
                            state = State.BeginAttack;
                            break;
                        case  Weapon.Hand:
                            state = State.BeginHandAtack;
                            break;
                    }
                    
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
            
            case  State.BeginDead:
                animator.SetBool("Dead", true);
                state = State.Dead;
                break;
            case  State.Dead:
                break;
        }
    }

    public bool IsDeadCharacter()
    {
        if (state == State.Dead)
            return true;
        if(state == State.BeginDead)
            return true;
        return false;
    }

    public void KillCharacter()
    {
        if (IsDeadCharacter())
        {
            return;
        }

        state = State.BeginDead;
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
