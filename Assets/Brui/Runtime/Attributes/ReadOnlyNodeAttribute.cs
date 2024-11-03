using System;
using UnityEngine;

namespace Brui.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyNodeAttribute : PropertyAttribute
    {
    }
}