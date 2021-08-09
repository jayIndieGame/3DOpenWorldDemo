using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueChangeEventListener<T>
{
    public delegate void OnValueChangeDelegate(T newVal);
    public event OnValueChangeDelegate OnVariableChange;
    public T m_value;
    public T Value
    {
        get
        {
            return m_value;
        }
        set
        {
            if (m_value.Equals(value)) return;
            OnVariableChange?.Invoke(value);
            m_value = value;
        }
    }

    public void ClearLisener()
    {
        OnVariableChange = null;
    }

}