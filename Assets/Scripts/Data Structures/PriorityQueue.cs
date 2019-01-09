using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Priority Queue implementation based on Tutorial series https://www.youtube.com/watch?v=-L-WgKMFuhE&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW
/// </summary>
/// <typeparam name="T"></typeparam>
public class PriorityQueue<T> where T : IPriorityQueueItem<T> {

    private T[] items;
    private int currentItemCount;

    public PriorityQueue(int maxPriorityQueueSize)
    {
        items = new T[maxPriorityQueueSize];
    }

    public void Add(T item)
    {
        item.PriorityQueueIndex = currentItemCount;
        items[currentItemCount] = item;

        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].PriorityQueueIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
        SortDown(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.PriorityQueueIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.PriorityQueueIndex * 2 + 1;
            int childIndexRight = item.PriorityQueueIndex * 2 + 2;
            int swapIndex = 0;

            if(childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if(childIndexRight < currentItemCount)
                    if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        swapIndex = childIndexRight;

                if (item.CompareTo(items[swapIndex]) < 0)
                    Swap(item, items[swapIndex]);
                else
                    return;
            }
            else
            {
                return;
            }
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.PriorityQueueIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];

            if(item.CompareTo(parentItem) > 0)
                Swap(item, parentItem);
            else
                break;

            parentIndex = (item.PriorityQueueIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.PriorityQueueIndex] = itemB;
        items[itemB.PriorityQueueIndex] = itemA;
        int itemAIndex = itemA.PriorityQueueIndex;
        itemA.PriorityQueueIndex = itemB.PriorityQueueIndex;
        itemB.PriorityQueueIndex = itemAIndex;
    }
}

public interface IPriorityQueueItem<T> : IComparable<T>
{
    int PriorityQueueIndex
    {
        get;
        set;
    }
}
