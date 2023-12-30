﻿using System.Text.Json.Serialization;
using Common.Utils;

namespace Common.Devices;

[Serializable]
public class VariableRegistryItem : ICloneable
{
    public object? Value { get; set; }

    public object? Default { get; private set; }

    public object? Max { get; set; }
    public object? Min { get; set; }
    public string Title { get; set; } = "";
    public string Remark { get; set; } = "";
    public VariableFlags Flags { get; set; } = VariableFlags.None;

    public VariableRegistryItem()
    {
    }

    public VariableRegistryItem(object? defaultValue, object? max = null, object? min = null, string title = "", string remark = "",
        VariableFlags flags = VariableFlags.None)
    {
        Value = defaultValue;
        Default = defaultValue;

        if (Value != null && max != null && Value.GetType() == max.GetType())
            Max = max;

        if (Value != null && min != null && Value.GetType() == min.GetType())
            Min = min;

        Title = title;
        Remark = remark;
        Flags = flags;
    }

    public VariableRegistryItem(object? value, object? defaultValue, object? max = null, object? min = null, string title = "",
        string remark = "", VariableFlags flags = VariableFlags.None)
    {
        Value = value ?? defaultValue;
        Default = defaultValue;

        if (Value != null && max != null && Value.GetType() == max.GetType())
            Max = max;

        if (Value != null && min != null && Value.GetType() == min.GetType())
            Min = min;

        Title = title;
        Remark = remark;
        Flags = flags;
    }

    public void SetVariable(object? newValue)
    {
        if (Value != null && newValue != null && Value.GetType() == newValue.GetType())
        {
            Value = newValue;
        }
    }

    internal void Merge(VariableRegistryItem variableRegistryItem)
    {
        Default = variableRegistryItem.Default;
        Title = variableRegistryItem.Title;
        Remark = variableRegistryItem.Remark;
        Min = variableRegistryItem.Min;
        Max = variableRegistryItem.Max;

        Flags = variableRegistryItem.Flags;

        if (variableRegistryItem.Default == null) return;

        var defaultType = variableRegistryItem.Default.GetType();
        var typ = Value.GetType();

        if (defaultType != typ && typ == typeof(long) && defaultType.IsEnum)
            Value = Enum.ToObject(defaultType, Value);
        else if (defaultType != typ && Value is long && TypeUtils.IsNumericType(defaultType))
            Value = Convert.ChangeType(Value, defaultType);
        else if (Value == null && defaultType != typ)
            Value = variableRegistryItem.Default;
    }

    public object Clone()
    {
        return new VariableRegistryItem(Value?.TryClone(), Default?.TryClone(), Max, Min, Title, Remark);
    }
}

public enum VariableFlags
{
    None = 0,
    UseHex = 1
}

public class VariableRegistry : ICloneable //Might want to implement something like IEnumerable here
{
    [JsonIgnore]
    public int Count => Variables.Count;

    public IDictionary<string, VariableRegistryItem> Variables { get; set; } = new Dictionary<string, VariableRegistryItem>();

    public void Combine(VariableRegistry otherRegistry, bool removeMissing = false)
    {
        //Below doesn't work for added variables
        var vars = new Dictionary<string, VariableRegistryItem>();

        foreach (var variable in otherRegistry.Variables)
        {
            if (removeMissing)
            {
                var local = Variables.TryGetValue(variable.Key, out var outVar) ? outVar : null;
                if (local != null)
                    local.Merge(variable.Value);
                else
                    local = variable.Value;

                vars.Add(variable.Key, local);
            }
            else
                Register(variable.Key, variable.Value);
        }

        if (removeMissing)
            Variables = vars;
    }

    public IEnumerable<string> GetRegisteredVariableKeys()
    {
        return Variables.Keys.ToArray();
    }

    public void Register(string name, object defaultValue, string title = "", object? max = null, object? min = null, string remark = "",
        VariableFlags flags = VariableFlags.None)
    {
        if (!Variables.ContainsKey(name))
            Variables.Add(name, new VariableRegistryItem(defaultValue, max, min, title, remark, flags));
    }

    public void Register(string name, VariableRegistryItem varItem)
    {
        if (!Variables.TryAdd(name, varItem))
            Variables[name].Merge(varItem);
    }

    public void SetVariable(string name, object? variable)
    {
        if (!Variables.ContainsKey(name)) return;

        Variables[name].SetVariable(variable);
    }

    public void ResetVariable(string name)
    {
        if (Variables.ContainsKey(name))
        {
            Variables[name].Value = Variables[name].Default;
        }
    }

    public string GetString(string name)
    {
        if (Variables.TryGetValue(name, out var value) && value.Value is string strVal)
            return strVal;
        
        return string.Empty;
    }

    public T GetVariable<T>(string name) where T : new()
    {
        if (Variables.TryGetValue(name, out var value) && value.Value is T tVal)
            return tVal;

        return default(T) ?? new T();
    }

    public bool GetVariableMax<T>(string name, out T? value)
    {
        if (Variables.TryGetValue(name, out var outVal) && outVal is { Max: not null, Value: T })
        {
            value = (T)outVal.Max;
            return true;
        }

        value = Activator.CreateInstance<T>();
        return false;
    }

    public bool GetVariableMin<T>(string name, out T value)
    {
        if (Variables.TryGetValue(name, out var outVal) && outVal is { Min: not null, Value: T })
        {
            value = (T)outVal.Min;
            return true;
        }

        value = Activator.CreateInstance<T>();
        return false;
    }

    public Type GetVariableType(string name)
    {
        if (Variables.TryGetValue(name, out var value) && value.Value != null)
            return value.Value.GetType();

        return typeof(object);
    }

    public string GetTitle(string name)
    {
        return Variables.TryGetValue(name, out var variable) ? variable.Title : string.Empty;
    }

    public string GetRemark(string name)
    {
        return Variables.TryGetValue(name, out var variable) ? variable.Remark : string.Empty;
    }

    public VariableFlags GetFlags(string name)
    {
        return Variables.TryGetValue(name, out var variable) ? variable.Flags : VariableFlags.None;
    }

    public object Clone()
    {
        var clone = new VariableRegistry();
        foreach (var registryItem in Variables)
        {
            clone.Variables[registryItem.Key] = (VariableRegistryItem)registryItem.Value.Clone();
        }
        return clone;
    }
}