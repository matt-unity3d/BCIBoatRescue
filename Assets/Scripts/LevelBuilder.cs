using System;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private Vector2 _gridSize;
    
    [Header("Level Blocks")]
    [SerializeField] private LevelBlock _playerPrefab;
    [SerializeField] private LevelBlock _rewardPrefab;
    [SerializeField] private LevelBlock _punishmentPrefab;
    [SerializeField] private LevelBlock _clearAreaPrefab;
    [SerializeField] private LevelBlock _barrierPrefab;

    private void Awake()
    {
        GameManager.Instance.RegisterBuilder(this);
    }

    private void OnDestroy()
    {
        GameManager.Instance.UnregisterBuilder(this);
    }

    public Level BuildLevel(LevelBuilderInstructions buildInstructions)
    {
        var builtLevel = new Level
        {
            LevelSize = buildInstructions.LevelSize,
        };

        foreach (var blockInstruction in buildInstructions._LevelBlocks)
        {
            var levelBlock = InstantiateBlockType(blockInstruction.Type);
            levelBlock.Column = (int) blockInstruction.Position.x;
            levelBlock.Row = (int) blockInstruction.Position.y;
            
            levelBlock.transform.position = new Vector3(levelBlock.Column * _gridSize.x, 0 , levelBlock.Row * _gridSize.y);

            builtLevel.AddBlock(levelBlock);
        }

        return builtLevel;
    }

    public void DestroyLevel(Level level)
    {
        if (level == null)
        {
            return;
        }

        foreach (var levelBlock in level.LevelBlocks)
        {
            Destroy(levelBlock.gameObject);
        }

        foreach (var player in level.Players)
        {
            Destroy(player.gameObject);
        }

        level.ResetLevelData();
    }

    private LevelBlock InstantiateBlockType(BlockType blockType)
    {
        GameObject blockGO;
        switch (blockType)
        {
            case BlockType.Player:
                blockGO = Instantiate(_playerPrefab.gameObject, transform);
                break;
            case BlockType.Barrier:
                blockGO = Instantiate(_barrierPrefab.gameObject, transform);
                break;
            case BlockType.Reward:
                blockGO = Instantiate(_rewardPrefab.gameObject, transform);
                break;
            case BlockType.Punishment:
                blockGO = Instantiate(_punishmentPrefab.gameObject, transform);
                break;
            case BlockType.ClearArea:
                blockGO = Instantiate(_clearAreaPrefab.gameObject, transform);
                break;
            default:
                return null;
        }

        return blockGO != null ? blockGO.GetComponent<LevelBlock>() : null;
    }
}

[Serializable]
public class LevelBuilderInstructions
{
    [Serializable]
    public class LevelBlock
    {
        public Vector2 Position;
        public BlockType Type;
    }
    
    public Vector2 LevelSize = Vector2.one;
    public LevelBlock[] _LevelBlocks;
}