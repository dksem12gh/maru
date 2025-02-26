using System;
using System.Collections.Generic;


public class Node<T>
{
    public T Value;
    public Node<T> Next;

    public Node(T value)
    {
        Value = value;
        Next = null;
    }
}

//list-> linkedlist 변경
//마지막 노드의 boxcollider 빼고 모든 노드 오브젝트의 콜라이더를 끔 - 팝업시 뒤 ui는 터치못하게
public class PopupColliderList<T>
{
    private Node<T> head;
    private Node<T> tail;
    private int count;

    public event Action onListChanged;

    public void Add(T item)
    {
        Node<T> newNode = new Node<T>(item);
        if (head == null)
        {
            head = newNode;
            tail = newNode;
        }
        else
        {
            tail.Next = newNode;
            tail = newNode;
        }

        count++;
        RaiseListChangedEvent();
    }

    public void Remove(T item)
    {
        if (item == null) return;

        Node<T> current = head;
        Node<T> previous = null;

        while (current != null)
        {
            if (current.Value.Equals(item))
            {
                if (previous == null)
                {
                    head = current.Next;
                    if (head == null)
                    {
                        tail = null;
                    }
                }
                else
                {
                    previous.Next = current.Next;
                    if (current.Next == null)
                    {
                        tail = previous;
                    }
                }

                count--;
                RaiseListChangedEvent();
                return;
            }

            previous = current;
            current = current.Next;
        }
    }

    public T GetLastItem()
    {
        if (tail != null)
        {
            return tail.Value;
        }
        else
        {
            return default(T); // 리스트가 비어 있으면 T 타입의 기본값을 반환합니다.
        }
    }

    public void Clear()
    {
        head = null;
        tail = null;
        count = 0;
        RaiseListChangedEvent();
    }

    public int Count
    {
        get { return count; }
    }

    public T Get(int index)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException("index");

        Node<T> current = head;
        for (int i = 0; i < index; i++)
        {
            current = current.Next;
        }

        return current.Value;
    }

    public List<T> GetList()
    {
        List<T> list = new List<T>();
        Node<T> current = head;

        while (current != null)
        {
            list.Add(current.Value);
            current = current.Next;
        }

        return list;
    }

    private void RaiseListChangedEvent()
    {
        onListChanged?.Invoke();
    }
}