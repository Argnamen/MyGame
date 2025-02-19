using Unity.VisualScripting;
using UnityEngine;

public class UIFactory : IUIFactory
{
    private BattleUIModel _battleUIModel;
    private InventoryUIModel _inventoryUIModel;
    private IHeroFactory _heroFactory;
    public UIFactory(BattleUIModel battleUIModelFactory, InventoryUIModel inventoryUIModel, IHeroFactory heroFactory)
    {
        _battleUIModel = battleUIModelFactory;
        _inventoryUIModel = inventoryUIModel;

        _heroFactory = heroFactory;
    }
    public void CreateUI()
    {
        PlayerCharacter playerCharacter = _heroFactory.GetHero().Character;

        _inventoryUIModel.Close();
        playerCharacter.UpdateInventory.AddListener(_inventoryUIModel.AddItem);
        _inventoryUIModel.AddWeapon(playerCharacter.Weapon);
        _inventoryUIModel.AddTool(playerCharacter.Tool);

        _battleUIModel.UpdateHealth(playerCharacter.HP);
        playerCharacter.DamageEvent.AddListener(() => _battleUIModel.UpdateHealth(playerCharacter.HP));
        _battleUIModel.BindInventory(_inventoryUIModel);
    }

    public void CreateGameOverUI()
    {
        Debug.Log("Game over UI created successfully.");
    }
}


public interface IUIFactory
{
    void CreateUI();
    void CreateGameOverUI();
}


