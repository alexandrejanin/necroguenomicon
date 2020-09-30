using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fireball", menuName = "Spell/Fireball")]
public class Fireball : Spell {
    [SerializeField] private SpellStat primaryDamage, secondaryDamage, secondaryDuration;
    [SerializeField] private GameObject projectilePrefab, passivePrefab;
    [SerializeField, Min(0)] private float projectileSpeed = 5f;

    public override HashSet<Vector2Int> GetValidTargets(Unit caster) {
        return caster.environment.ManhattanRange(caster.Position, Range);
    }

    public override IEnumerator PrimaryEffect(Unit caster, Vector2Int position, bool isPrimarySpell, HashSet<Unit> targets) {
        var projectile = Instantiate(projectilePrefab, caster.WorldPosition, Quaternion.identity);

        var worldTarget = new Vector3(position.x + 0.5f, position.y + 0.5f);

        projectile.transform.right = worldTarget - caster.WorldPosition;

        var progress = 0f;
        var distance = Vector3.Distance(caster.WorldPosition, worldTarget);

        while (progress < 1f) {
            progress += Time.deltaTime * projectileSpeed / distance;
            projectile.transform.position = Vector3.Lerp(caster.WorldPosition, worldTarget, progress);
            yield return new WaitForEndOfFrame();
        }

        Object.Destroy(projectile);

        var target = caster.environment.GetUnit(position);
        if (target == null)
            yield break;

        Damage(target, primaryDamage.GetAmount(isPrimarySpell), caster);
        targets?.Add(target);
    }

    public override IEnumerator SecondaryEffect(Unit caster, HashSet<Unit> targets, bool isSecondarySpell, HashSet<Unit> secondaryTargets) {
        if (targets == null)
            yield break;
        
        foreach (var target in targets) {
            yield return new WaitForEndOfFrame();

            target.AddTickerEffect(new TickerEffect(
                "En feu !",
                secondaryDuration.GetAmount(isSecondarySpell),
                onTick: unit => Damage(unit, secondaryDamage.GetAmount(isSecondarySpell), caster, "En feu !")
            ));
        }
    }
}
