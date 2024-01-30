using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDealDamage
{
    public void TryDealDamage(int damage, IDamageSource source);
}

public interface IDamageSource
{
    public Transform Transform { get; }
}

public interface IDamageable
{
    public void TryTakeDamage(int damage, IDamageSource source);
}

public interface IHealth
{

}
