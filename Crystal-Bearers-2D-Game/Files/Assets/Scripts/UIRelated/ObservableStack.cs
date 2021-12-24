using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* Listens and automatically updates Stack when an event happens */

public delegate void UpdateStackEvent();

public class ObservableStack<T> : Stack<T>
{
    // Events that are raised when Push,Pop, or Clear Stack
    public event UpdateStackEvent OnPush;
    public event UpdateStackEvent OnPop; 
    public event UpdateStackEvent OnClear;

    // instantiate a stack with an amount of items
    public ObservableStack(ObservableStack<T> items) : base(items)
    {

    }

    // empty constructor for stack with no amount of items
    public ObservableStack()
    {

    }


    public new void Push(T item)
    {
        base.Push(item);

        if(OnPush != null)
        {
            OnPush();
        }
    }

    public new T Pop()
    {
        T item = base.Pop();

        if(OnPop != null)
        {
            OnPop();
        }

        return item;
    }

    public new void Clear()
    {
        base.Clear();

        if(OnClear != null)
        {
            OnClear();
        }
    }

}



