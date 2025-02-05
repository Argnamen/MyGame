using UnityEngine;

public class Enemy : Character
{
    private Direction _lastDirection;

    public float VisibilityRange;
    public Player PlayerCharacter;

    private Minion[] _minions;
    private int _numberMinion = 0;
    private float _jump;

    private int StepNumber = 100;

    private Vector2 _movePatrolVector;

    private bool _playerFound = false;
    public Character Target { get; private set; }

    public Enemy(int healt, float spead, float visiabilityRange, float size, Weapon weapon, Vector2 startPos, Player player, Minion[] minion, int numberMinion, Environment[] environments, IStaticDataService staticDataService, ItemsInWorld itemsInWorld)
        : base(healt, spead, size, weapon, startPos, environments, staticDataService, itemsInWorld)
    {
        _jump = 1;
        PlayerCharacter = player;
        _minions = minion;
        _numberMinion = numberMinion;
        Target = player.Character;

        VisibilityRange = visiabilityRange;

        DamageEvent.AddListener(PlayerFound);
    }

    public override Vector2 Move(Direction direction)
    {
        _lastDirection = direction;
        return base.Move(direction);
    }

    public Vector2 MoveToPlayer(float distante)
    {
        Vector2 newPos = _startPos;

        if (PlayerCharacter == null || Vector2.Distance(newPos, PlayerCharacter.transform.position) <= distante)
        {
            return _startPos;
        }

        newPos = Vector2.MoveTowards(_startPos, PlayerCharacter.transform.position, _spead);

        if (Vector2.Distance(newPos, PlayerCharacter.transform.position) > VisibilityRange && !_playerFound)
        {
            newPos = PatrolMove(_startPos);
            _startPos = newPos;

            return _startPos;
        }

        PlayerFound();

        _gameMode.BlockStealMode(_playerFound);

        for (int i = 0; i < _minions.Length; i++)
        {
            if (_minions[i] != null && i != _numberMinion)
            {
                if (Vector2.Distance(_startPos, _minions[i].transform.position) <= 1 && Vector2.Distance(_startPos, PlayerCharacter.transform.position) > Vector2.Distance(_minions[i].transform.position, PlayerCharacter.transform.position))
                {
                    newPos = Vector2.MoveTowards(_startPos, AlternativeMove(_startPos, _minions[i].transform.position), _spead);
                }

                if (Vector2.Distance(_startPos, _minions[i].transform.position) <= 0.9f || Vector2.Distance(newPos, _minions[i].transform.position) <= 0.9f)
                {
                    newPos = Vector2.MoveTowards(_startPos, FindOppositePoint(_startPos, PlayerCharacter.transform.position), _spead);
                    break;
                }
            }
        }

        _startPos = newPos;
        return newPos;
    }

    private void PlayerFound()
    {
        _playerFound = true;
    }

    public override void BlockSteals()
    {
        _playerFound = true;
    }

    private Vector2 PatrolMove(Vector2 startPos)
    {
        Vector2 newPos = startPos;

        float mapX = _staticDataService.GetWorld(0)[_staticDataService.CurrentRoom].x * 2;
        float mapY = _staticDataService.GetWorld(0)[_staticDataService.CurrentRoom].y * 2;

        if (StepNumber == 0 || StepNumber == 100)
        {
            StepNumber = 100;

            _movePatrolVector = new Vector2(Random.Range(-mapX, mapX), Random.Range(-mapY, mapY));
        }

        newPos = Vector2.MoveTowards(startPos, startPos + _movePatrolVector, _spead);

        if (Mathf.Abs(newPos.x) >= mapX || Mathf.Abs(newPos.y) >= mapY)
        {
            StepNumber = 0;
            return PatrolMove(startPos);
        }

        StepNumber--;

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
        Vector2 midpoint = (A + B) / 2;
        float sideLength = Vector2.Distance(A, B);
        float angle = Mathf.Deg2Rad * 60;
        Vector2 direction = (B - A).normalized;

        Vector2 rotatedDirection1 = new Vector2(
            direction.x * Mathf.Cos(angle) - direction.y * Mathf.Sin(angle),
            direction.x * Mathf.Sin(angle) + direction.y * Mathf.Cos(angle)
        );

        Vector2 rotatedDirection2 = new Vector2(
            direction.x * Mathf.Cos(-angle) - direction.y * Mathf.Sin(-angle),
            direction.x * Mathf.Sin(-angle) + direction.y * Mathf.Cos(-angle)
        );

        C1 = midpoint + rotatedDirection1 * (sideLength / Mathf.Sqrt(3));
        C2 = midpoint + rotatedDirection2 * (sideLength / Mathf.Sqrt(3));
    }

    private Vector2 FindOppositePoint(Vector2 current, Vector2 target)
    {
        Vector2 delta = target - current;
        return target + delta;
    }

    public Vector2 MoveToTarget(float distance)
    {
        Vector2 direction = (Target.Position - Position).normalized;
        return Position + direction * distance;
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

