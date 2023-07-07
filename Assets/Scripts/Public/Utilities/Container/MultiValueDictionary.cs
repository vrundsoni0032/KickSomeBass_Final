using System.Collections.Generic;

public class MultiValueDictionary<TKey, TValues> : Dictionary<TKey, List<TValues>>
{
    public bool AddValue(TKey key, TValues value)
    {
        if(this.ContainsKey(key))
        {
            if(!this[key].Contains(value))
            {
                this[key].Add(value);
                return true;
            }
        }
        else
        {
            this.Add(key, new List<TValues>() {value});
            return true;
        }

        return false;
    }

    public bool AddMultipleValues(TKey key, params TValues[] values)
    {
        bool bAnyNewAddition = false;

        if (this.ContainsKey(key))
        {
            foreach (TValues value in values)
            {
                if (!this[key].Contains(value))
                {
                    this[key].Add(value);
                    bAnyNewAddition = true;
                }
            }
        }
        else
        {
            this.Add(key, new List<TValues>(values));
            bAnyNewAddition = true;
        }

        return bAnyNewAddition;
    }

    public void AddMultipleKeys(TValues value, params TKey[] keys)
    {
        foreach (TKey key in keys)
        {
            AddValue(key, value);
        }
    }

    public bool CheckKeyExist(TKey key)
    {
        return this.ContainsKey(key);
    }

    public bool CheckValueExist(TKey key, TValues value)
    {
        if (this.ContainsKey(key))
        {
            return this[key].Contains(value);
        }
        return false;
    }


    public bool RemoveKey(TKey key)
    {
        if (this.ContainsKey(key)) { return this.Remove(key); }
        return false;
    }

    public bool RemoveValue(TKey key, TValues value)
    {
        if (this.ContainsKey(key))
        {
            if (this[key].Contains(value))
            {
                return this[key].Remove(value);
            }
        }

        return false;
    }

    public void RemoveValueForAllKeys(TValues value)
    {
        foreach (TKey key in this.Keys)
        {
            RemoveValue(key, value);
        }
    }
}