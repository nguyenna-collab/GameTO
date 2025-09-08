using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SlidingPuzzle : MonoBehaviour
{
    [SerializeField, Min(2)] private int _size;
    [SerializeField, Min(1)] private float _tileSize;
    [SerializeField] private Button _tilePrefab;
    [SerializeField] private Sprite[] _tileSprites = new Sprite[0];

    public Action OnPuzzleSolved;

    private Button[,] _grid;
    private Tile _emptyTile;

    private Vector2 _topLeftPosition;
    private bool _isShuffling;

    private void Start()
    {
        Init();
        Shuffle();
    }

    [Button]
    public void Init()
    {
        var rect = GetComponent<RectTransform>();
        _grid = new Button[_size, _size];
        _topLeftPosition = new Vector2(rect.rect.width / 2 - _tileSize / 2, -rect.rect.height / 2 + _tileSize / 2);
        int value = 1;
        for (int i = _size - 1; i >= 0; i--)
        {
            for (int j = _size - 1; j >= 0; j--)
            {
                _grid[i, j] = CreateTile(i, j, value);
                value++;
            }
        }
        _grid[0, 0].GetComponent<Tile>().Value = 0; // Empty tile
        _emptyTile = _grid[0, 0].GetComponent<Tile>();
    }

    public bool TryMove(Tile target)
    {
        if (target == null || _emptyTile == null || target.Equals(_emptyTile)) return false;

        if (IsAdjacent(target.Pos, _emptyTile.Pos))
        {
            // World Position
            var emptyTileWorldPos = _emptyTile.GetComponent<RectTransform>().position;
            var targetWorldPos = target.GetComponent<RectTransform>().position;
            var tempWorldPos = targetWorldPos;
            _emptyTile.GetComponent<RectTransform>().position = tempWorldPos;
            target.GetComponent<RectTransform>().position = emptyTileWorldPos;

            // Update Grid
            var temp = _grid[_emptyTile.Pos.x, _emptyTile.Pos.y];
            _grid[_emptyTile.Pos.x, _emptyTile.Pos.y] = _grid[target.Pos.x, target.Pos.y];
            _grid[target.Pos.x, target.Pos.y] = temp;

            Vector2Int tempPos = _emptyTile.Pos;
            _emptyTile.Pos = target.Pos;
            target.Pos = tempPos;
            if (IsSolved())
            {
                Debug.Log("Puzzle Solved!");
            }
            return true;
        }
        return false;
    }

    private Button CreateTile(int x, int y, int value)
    {
        var pos = _topLeftPosition + new Vector2(x * _tileSize, -y * _tileSize);
        Button tile = Instantiate(_tilePrefab);
        if (x == 0 && y == 0) tile.gameObject.name = "EmptyTile";
        tile.onClick.AddListener(() => TryMove(tile.GetComponent<Tile>()));
        tile.GetComponent<RectTransform>().localPosition = pos;
        tile.transform.SetParent(transform, false);
        tile.GetComponent<Tile>().Value = value;
        tile.GetComponent<Tile>().Pos = new Vector2Int(x, y);
        tile.GetComponent<Image>().sprite = value > 0 && value <= _tileSprites.Length ? _tileSprites[value - 1] : null;
        return tile;
    }

    [Button]
    public void Shuffle()
    {
        _isShuffling = true;
        int shuffleCount = 0;
        while (!IsSolved() && shuffleCount < 100)
        {
            var randomTile = _grid[UnityEngine.Random.Range(0, _size), UnityEngine.Random.Range(0, _size)];
            TryMove(randomTile.GetComponent<Tile>());
            shuffleCount++;
        }
        _isShuffling = false;
    }

    private bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y)) == 1;
    }

    bool IsSolved()
    {
        if (_isShuffling) return false;
        int value = 1;
        for (int y = 0; y < _size; y++)
        {
            for (int x = 0; x < _size; x++)
            {
                if (x == 0 && y == 0 && _grid[x, y].gameObject.name == "EmptyTile")
                {
                    OnPuzzleSolved?.Invoke();
                    return true;
                }
                if (_grid[x, y].GetComponent<Tile>().Value != value) return false;
                value++;
            }
        }
        Debug.Log("Puzzle is solved!");
        OnPuzzleSolved?.Invoke();
        return true;
    }
}