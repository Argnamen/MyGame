using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Projectile : Character
{
    public Character Target;

    public Vector2 Vector;

    private bool isFirstStart = true;

    WorldsMap _worldsMap;

    [Inject]
    public void Constuct(WorldsMap worldsMap)
    {
        _worldsMap = worldsMap;
    }
    public Projectile(int healt, float spead, float size, Weapon weapon, Vector2 startPos, Environment[] environments) : base(healt, spead, size, weapon, startPos, environments)
    {
        /*
        newPos = new Vector2(
                 Target.Position.x < _startPos.x && Target.Position.x > 0 || Target.Position.x > _startPos.x && Target.Position.x < 0 ?
                    _startPos.x - Target.Position.normalized.x * _spead :
                    _startPos.x + Target.Position.normalized.x * _spead,
                 Target.Position.y < _startPos.y && Target.Position.y > 0 || Target.Position.y > _startPos.y && Target.Position.y < 0 ?
                    _startPos.y - Target.Position.normalized.y * _spead :
                    _startPos.y + Target.Position.normalized.y * _spead);
        */
    }

    public virtual Vector2 Move()
    {
        Vector2 newPos = _startPos;

        if (isFirstStart)
        {
            newPos = Vector2.zero;
            isFirstStart = false;

            Vector = (Target.Position - _startPos).normalized * _spead;
        }

        int world = _worldsMap.CorrectWorld;

        // Проверка столкновения с границами мира
        if (_worldsMap.GetWorld(world)[0].x * 2 < Mathf.Abs(newPos.x) || _worldsMap.GetWorld(world)[0].y * 2 < Mathf.Abs(newPos.y))
        {
            // Пересчёт вектора движения
            if (_worldsMap.GetWorld(world)[0].x * 2 < Mathf.Abs(newPos.x))
            {
                Vector = Vector2.Reflect(Vector, Vector2.right); // Отражение по оси X
            }
            else if (_worldsMap.GetWorld(world)[0].y * 2 < Mathf.Abs(newPos.y))
            {
                Vector = Vector2.Reflect(Vector, Vector2.up); // Отражение по оси Y
            }
        }

        foreach (var env in Environments)
        {
            if (env != null && (Vector2.Distance(newPos, env.transform.position) < env.Enemy.Size))
            {
                // Пересчёт вектора движения
                if (Mathf.Abs(env.transform.position.x - newPos.x) <= Mathf.Abs(env.transform.position.y - newPos.y))
                {
                    Vector = Vector2.Reflect(Vector, Vector2.right); // Отражение по оси X
                }
                else
                {
                    Vector = Vector2.Reflect(Vector, Vector2.up); // Отражение по оси Y
                }
            }
        }

        newPos = _startPos + Vector * _spead;

        _startPos = newPos;

        return newPos;
    }


}
