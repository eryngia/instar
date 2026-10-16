
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWeaponSO", menuName = "EnemyWeaponSO", order = 0)]
public class EnemyWeaponSO : WeaponSO {

    public new EnemyProjectileSO projectileSO {
        get => (EnemyProjectileSO) base.projectileSO;
        set => base.projectileSO = value;
    }

}