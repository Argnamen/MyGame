using System.Collections.Generic;

[System.Serializable]
public class EconomyData
{
    public List<Currency> Currencies;

    public EconomyData()
    {
        Currencies = new List<Currency>();
    }
}

[System.Serializable]
public class Currency
{
    public CurrencyType CurrencyType;
    public int Amount;

    public Currency(CurrencyType currencyType, int amount)
    {
        CurrencyType = currencyType;
        Amount = amount;
    }
}

public enum CurrencyType
{
    Gold
}

