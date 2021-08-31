using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventCenter
{
    private static Dictionary<EventType, Delegate> m_EventTable = new Dictionary<EventType, Delegate>();

    private static void OnListenerAdding(EventType eventType, Delegate callBack)
    {
        if (!m_EventTable.ContainsKey(eventType))
        {
            m_EventTable.Add(eventType, null);
        }
        //获得key所对应的value
        Delegate d = m_EventTable[eventType];
        //防止新加入的Key没有对应的Value或者同一个key想要添加多个value
        if (d != null && d.GetType() != callBack.GetType())
        {
            throw new Exception(string.Format("尝试添加参数类型不同的委托，当前事件所对应的委托是{0},要添加的委托类型为{1}", d.GetType(), callBack.GetType()));
        }
    }
    private static void OnListnerRemoving(EventType eventType, Delegate callBack)
    {
        if (m_EventTable.ContainsKey(eventType))
        {
            Delegate d = m_EventTable[eventType];
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有监听委托.", eventType));
            }
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试事件{0}移除委托类型{1}与移除的委托事件类型{2}不一致.", eventType, d.GetType(), callBack.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误：没有事件码{0}.", eventType));
        }
    }
    private static void OnListenerRemoved(EventType eventType)
    {
        if (m_EventTable[eventType] == null)
        {
            m_EventTable.Remove(eventType);
        }
    }

//无参添加监听，这里callback理解成action就行了
//另一个脚本中调用eventType的时候会给定EventType.eventType的值
    public static void AddListensener(EventType eventType, CallBack callBack)
    {
        OnListenerAdding(eventType, callBack);
        m_EventTable[eventType] = (CallBack)m_EventTable[eventType] + callBack;
    }
    //无参移除监听
    public static void AddListensener<T>(EventType eventType, CallBack<T> callBack)
    {
        OnListenerAdding(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] + callBack;
    }
    public static void AddListensener<T,X>(EventType eventType, CallBack<T,X> callBack)
    {
        OnListenerAdding(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T,X>)m_EventTable[eventType] + callBack;
    }
    public static void AddListensener<T, X,Y>(EventType eventType, CallBack<T, X,Y> callBack)
    {
        OnListenerAdding(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T, X,Y>)m_EventTable[eventType] + callBack;
    }
    public static void AddListensener<T, X,Y,Z>(EventType eventType, CallBack<T, X,Y,Z> callBack)
    {
        OnListenerAdding(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T, X,Y,Z>)m_EventTable[eventType] + callBack;
    }
    public static void RemoveLinsener(EventType eventType, CallBack callBack)
    {
        OnListnerRemoving(eventType, callBack);
        m_EventTable[eventType] = (CallBack)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    public static void RemoveLinsener<T>(EventType eventType, CallBack<T> callBack)
    {
        OnListnerRemoving(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    public static void RemoveLinsener<T,X>(EventType eventType, CallBack<T,X> callBack)
    {
        OnListnerRemoving(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T,X>)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    public static void RemoveLinsener<T, X,Y>(EventType eventType, CallBack<T, X,Y> callBack)
    {
        OnListnerRemoving(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T, X,Y>)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    public static void RemoveLinsener<T, X, Y,Z>(EventType eventType, CallBack<T, X, Y,Z> callBack)
    {
        OnListnerRemoving(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T, X, Y,Z>)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }


    //广播无参监听时间
    public static void BroadCast(EventType eventType)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack callBack = d as CallBack;
            if (callBack != null)
            {
                callBack();
            }
            else
            {
                throw new Exception(string.Format("广播事件错误:事件{0}对应委托具有不同类型", eventType));
            }
        }
    }
    public static void BroadCast<T>(EventType eventType, T arg)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack<T> callBack = d as CallBack<T>;
            if (callBack != null)
            {
                callBack(arg);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误:事件{0}对应委托具有不同类型", eventType));
            }
        }
    }
    public static void BroadCast<T,X>(EventType eventType, T arg,X arg1)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack<T,X> callBack = d as CallBack<T,X>;
            if (callBack != null)
            {
                callBack(arg,arg1);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误:事件{0}对应委托具有不同类型", eventType));
            }
        }
    }
    public static void BroadCast<T,X,Y>(EventType eventType, T arg,X arg1,Y arg2)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack<T,X,Y> callBack = d as CallBack<T,X,Y>;
            if (callBack != null)
            {
                callBack(arg,arg1,arg2);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误:事件{0}对应委托具有不同类型", eventType));
            }
        }
    }
    public static void BroadCast<T, X, Y,Z>(EventType eventType, T arg, X arg1, Y arg2,Z arg3)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack<T, X, Y,Z> callBack = d as CallBack<T, X, Y,Z>;
            if (callBack != null)
            {
                callBack(arg, arg1, arg2,arg3);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误:事件{0}对应委托具有不同类型", eventType));
            }
        }
    }
}


