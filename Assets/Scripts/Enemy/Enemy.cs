using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : Character
{
    private Direction _lastDirection;

    public Player PlayerCharacter;
    private Minion[] _minions;

    private int _numberMinion = 0;

    private float _jump;
    public Enemy(int healt, float spead, float size, Weapon weapon, Vector2 startPos, Player player, Minion[] minion, int numberMinion, Environment[] environments) : base(healt, spead, size, weapon, startPos, environments)
    {
        _jump = 1;
        PlayerCharacter = player;
        _minions = minion;
        _numberMinion = numberMinion;
    }

    public override Vector2 Move(Direction direction)
    {
        _lastDirection = direction;
        return base.Move(direction);
    }

    public Vector2 MoveToPlayer(float distante)
    {
        Vector2 newPos = _startPos;

        int c = 0;

        if (PlayerCharacter == null || Vector2.Distance(newPos, PlayerCharacter.transform.position) <= distante)
        {
            return _startPos;
        }

        newPos = Vector2.MoveTowards(_startPos, PlayerCharacter.transform.position, 0.1f);

        for(int i = 0; i < _minions.Length; i++)
        {
            if(_minions[i] != null 
                && i != _numberMinion)
            {
                if (Vector2.Distance(_startPos, _minions[i].transform.position) <= 1 
                    && Vector2.Distance(_startPos, PlayerCharacter.transform.position) > Vector2.Distance(_minions[i].transform.position, PlayerCharacter.transform.position))
                {
                    c++;

                    newPos = Vector2.MoveTowards(_startPos, AlternativeMove(_startPos, _minions[i].transform.position), 0.1f);
                }

                if(Vector2.Distance(_startPos, _minions[i].transform.position) <= 0.9f || Vector2.Distance(newPos, _minions[i].transform.position) <= 0.9f)
                {
                    newPos = Vector2.MoveTowards(_startPos, FindOppositePoint(_startPos, PlayerCharacter.transform.position), 0.1f);
                    break;
                }
            }
        }

        /*
        if (c > 1 && c < 8)
        {
            newPos = Vector2.MoveTowards(_startPos, FindOppositePoint(_startPos, _player.transform.position), 0.1f);
        }
        else if (c >= 8)
        {
            newPos = Vector2.MoveTowards(_startPos, AlternativeMove(_startPos, _player.transform.position), 0.1f); ;
        }
        */

        _startPos = newPos;

        return newPos;
    }

    private Vector2 AlternativeMove(Vector2 target, Vector2 target2)
    {
        Vector2 newPos = target;

        Vector2 C1, C2;

        FindEquilateralTriangleVertex(target, target2, out C1, out C2);

        newPos = Vector2.Distance(C1, PlayerCharacter.transform.position) <= Vector2.Distance(C2, PlayerCharacter.transform.position) ? C1 : C2;

        return newPos;
    }

    private void FindEquilateralTriangleVertex(Vector2 A, Vector2 B, out Vector2 C1, out Vector2 C2)
    {
        // Находим середину отрезка AB
        Vector2 midpoint = (A + B) / 2;

        // Находим длину стороны треугольника
        float sideLength = Vector2.Distance(A, B);

        // Угол поворота на 60 градусов в радианах
        float angle = Mathf.Deg2Rad * 60;

        // Вектор направления от A к B
        Vector2 direction = (B - A).normalized;

        // Поворот направления на 60 градусов для первой вершины
        Vector2 rotatedDirection1 = new Vector2(
            direction.x * Mathf.Cos(angle) - direction.y * Mathf.Sin(angle),
            direction.x * Mathf.Sin(angle) + direction.y * Mathf.Cos(angle)
        );

        // Поворот направления на -60 градусов для второй вершины
        Vector2 rotatedDirection2 = new Vector2(
            direction.x * Mathf.Cos(-angle) - direction.y * Mathf.Sin(-angle),
            direction.x * Mathf.Sin(-angle) + direction.y * Mathf.Cos(-angle)
        );

        // Вычисляем координаты третьих вершин
        C1 = midpoint + rotatedDirection1 * (sideLength / Mathf.Sqrt(3));
        C2 = midpoint + rotatedDirection2 * (sideLength / Mathf.Sqrt(3));
    }

    private Vector2 FindOppositePoint(Vector2 current, Vector2 target)
    {
        // Вычисляем разницу координат
        Vector2 delta = target - current;

        // Определяем координаты противоположной точки
        return target + delta;
    }

    public Vector2 JumpToDamage()
    {
        Vector2 newPos = _startPos;

        switch (_lastDirection)
        {
            case Direction.Left:
                newPos = Move(Direction.Right);
                break;
            case Direction.Right:
                newPos = Move(Direction.Left);
                break;
            case Direction.Up:
                newPos = Move(Direction.Down);
                break;
            case Direction.Down:
                newPos = Move(Direction.Up);
                break;
            default:
                newPos = Move(Direction.None);
                break;
        }

        _startPos = newPos;

        return newPos;
    }
}
