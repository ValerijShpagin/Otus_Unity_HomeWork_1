using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour
{
    Character character;
    Character targetch;

    void Start()
    {
        character = GetComponentInParent<Character>();
        targetch = character.target.GetComponent<Character>();
    }

    void ShootEnd()
    {
        character.SetState(Character.State.Idle);
        DeadCharacter();
    }

    void AttackEnd()
    {
        character.SetState(Character.State.RunningFromEnemy);
    }

    void DeadCharacter()
    {
        targetch.KillCharacter();
    }

    void HandEnd()
    {
        character.SetState(Character.State.Idle);
        DeadCharacter();
    }
}
