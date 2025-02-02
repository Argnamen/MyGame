using System.Collections.Generic;
using UnityEngine;

public class EconomyService : IEconomyService
{
    private Dictionary<CurrencyType, int> _currencies = new Dictionary<CurrencyType, int>();

    public void Initialize(EconomyData economyData)
    {
        foreach (var currency in economyData.Currencies)
        {
            _currencies[currency.CurrencyType] = currency.Amount;
        }
        Debug.Log("Economy initialized successfully.");
    }

    public int GetCurrencyAmount(CurrencyType currencyType)
    {
        return _currencies.TryGetValue(currencyType, out var amount) ? amount : 0;
    }

    public void AddCurrency(CurrencyType currencyType, int amount)
    {
        if (_currencies.ContainsKey(currencyType))
        {
            _currencies[currencyType] += amount;
        }
        else
        {
            _currencies[currencyType] = amount;
        }
        Debug.Log($"Added {amount} {currencyType}. New amount: {_currencies[currencyType]}");
    }

    public bool SpendCurrency(CurrencyType currencyType, int amount)
    {
        if (_currencies.TryGetValue(currencyType, out var currentAmount) && currentAmount >= amount)
        {
            _currencies[currencyType] -= amount;
            Debug.Log($"Spent {amount} {currencyType}. Remaining amount: {_currencies[currencyType]}");
            return true;
        }
        Debug.LogWarning($"Not enough {currencyType} to spend.");
        return false;
    }
}


public interface IEconomyService
{
    void Initialize(EconomyData economyData);
    int GetCurrencyAmount(CurrencyType currencyType);
    void AddCurrency(CurrencyType currencyType, int amount);
    bool SpendCurrency(CurrencyType currencyType, int amount);
}

