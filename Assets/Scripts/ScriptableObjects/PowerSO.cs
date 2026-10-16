
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerSO", menuName = "PowerSO", order = 0)]
public class PowerSO : ScriptableObject {
    
    public Sprite sprite; // menu sprite
    public String description; // menu description
    public virtual void PlayerPower(Player player) {} // effects for player
    public virtual void PlayerFirePower(PlayerWeaponSO weaponSO) {} // effects for player main weapon
    public virtual void PlayerAbilityPower(PlayerWeaponSO weaponSO) {} // effects for player special weapon

}
