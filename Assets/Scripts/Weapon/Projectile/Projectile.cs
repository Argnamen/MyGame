using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Projectile : Character
{
    public Character Target;
    public Vector2 Vector;
    private bool isFirstStart = true;
    private WorldsMap _worldsMap;

    [Inject]
    public void Construct(WorldsMap worldsMap)
    {
        _worldsMap = worldsMap;
    }

    public Projectile(int healt, float spead, float size, Weapon weapon, Vector2 startPos, Environment[] environments, IStaticDataService staticDataService, ItemsInWorld itemsInWorld)
        : base(healt, spead, size, weapon, startPos, environments, staticDataService, itemsInWorld)
    {
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

        // �������� ������������ � ��������� ����
        if (_worldsMap.GetWorld(world)[0].x * 2 < Mathf.Abs(newPos.x) || _worldsMap.GetWorld(world)[0].y * 2 < Mathf.Abs(newPos.y))
        {
            // �������� ������� ��������
            if (_worldsMap.GetWorld(world)[0].x * 2 < Mathf.Abs(newPos.x))
            {
                Vector = Vector2.Reflect(Vector, Vector2.right); // ��������� �� ��� X
            }
            else if (_worldsMap.GetWorld(world)[0].y * 2 < Mathf.Abs(newPos.y))
            {
                Vector = Vector2.Reflect(Vector, Vector2.up); // ��������� �� ��� Y
            }

            UpdateHP(-10);
        }

        foreach (var env in Environments)
        {
            if (env != null && (Vector2.Distance(newPos, env.transform.position) < env.Character.Size))
            {
                // �������� ������� ��������
                if (Mathf.Abs(env.transform.position.x - newPos.x) <= Mathf.Abs(env.transform.position.y - newPos.y))
                {
                    Vector = Vector2.Reflect(Vector, Vector2.right); // ��������� �� ��� X
                }
                else
                {
                    Vector = Vector2.Reflect(Vector, Vector2.up); // ��������� �� ��� Y
                }

                UpdateHP(-10);
            }
        }

        newPos = _startPos + Vector * _spead;
        _startPos = newPos;

        return newPos;
    }
}
