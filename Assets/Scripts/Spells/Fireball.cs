using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fireball", menuName = "Spell/Fireball")]
public class Fireball : Spell {
    [SerializeField] private SpellStat primaryDamage, secondaryDamage, secondaryDuration;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Animator passivePrefab;
    [SerializeField, Min(0)] private float projectileSpeed = 5f;

    public override HashSet<Vector2Int> GetTargetedTiles(Unit caster, Vector2Int position) =>
        new HashSet<Vector2Int>(caster.environment.Line(caster.Position, position));

    public override IEnumerator PrimaryEffect(Unit caster, Vector2Int position, bool isPrimarySpell, System.Func<HashSet<Unit>, IEnumerator> then) {
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

        Destroy(projectile);

        var target = caster.environment.GetUnit(position);
        if (target == null)
            yield break;

        Damage(target, primaryDamage.GetAmount(isPrimarySpell), caster);
        yield return then(new HashSet<Unit>{target});
    }

    public override IEnumerator SecondaryEffect(Unit caster, HashSet<Unit> targets, bool isSecondarySpell, System.Func<HashSet<Unit>, IEnumerator> then = null) {
        foreach (var target in targets) {
            var animator = Instantiate(passivePrefab, target.WorldPosition, passivePrefab.transform.rotation);
            yield return new WaitForSeconds(1f);

            Destroy(animator.gameObject);

            target.AddTickerEffect(new TickerEffect(
                "En feu !",
                secondaryDuration.GetAmount(isSecondarySpell),
                unit => Damage(unit, secondaryDamage.GetAmount(isSecondarySpell), caster, "En feu !")
            ));
        }

        if (then != null)
            yield return then(targets);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
