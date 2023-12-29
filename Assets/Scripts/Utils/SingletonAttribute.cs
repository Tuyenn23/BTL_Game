using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class SingletonAttribute : Attribute
{
    public readonly string Name;
    public readonly bool IsDontDestroy;

    public SingletonAttribute(string _name, bool _isDontDestroy)
    {
        Name = _name;
        IsDontDestroy = _isDontDestroy;
    }

    public SingletonAttribute(string _name)
    {
        Name = _name;
        IsDontDestroy = false;
    }
}