using System.Reflection;
using UnityEditor.ShaderGraph;
using UnityEngine;

[Title("Custom", "My Custom Node")]
public class MyCustomNode : CodeFunctionNode
{
    public MyCustomNode()
    {
        name = "My Custom Node";
    }

    protected override MethodInfo GetFunctionToConvert()
    {
        return GetType().GetMethod("MyCustomFunction", BindingFlags.Static | BindingFlags.NonPublic);
    }

    static string MyCustomFunction(
        [Slot(0, Binding.None)] DynamicDimensionVector A,
        [Slot(1, Binding.None)] DynamicDimensionVector B,
        [Slot(2, Binding.None)] out DynamicDimensionVector Out)
    {
        return @"{
                    Out = A + B;
                 }";
    }

    static string Min3(
        [Slot(0, Binding.None)] DynamicDimensionVector A,
        [Slot(1, Binding.None)] DynamicDimensionVector B,
        [Slot(2, Binding.None)] DynamicDimensionVector C,
        [Slot(3, Binding.None)] out DynamicDimensionVector Out)
    {
        return @"{
                    Out = min(min(A, B), C);
                 }
                 ";
    }

    static string FlipNormal(
        [Slot(0, Binding.WorldSpaceNormal)] Vector3 Normal,
        [Slot(1, Binding.None)] Boolean Predicate,
        [Slot(2, Binding.None)] out Vector3 Out)
    {
        Out = Vector3.zero;

        return
        @"
        {
            Out = Predicate == 1 ? -1 * Normal : Normal;;
        }
        ";
    }
}
