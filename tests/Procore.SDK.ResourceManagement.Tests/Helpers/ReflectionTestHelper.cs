using System;
using System.Reflection;

namespace Procore.SDK.ResourceManagement.Tests.Helpers;

/// <summary>
/// Helper class for reflection-based operations in tests.
/// Provides utilities for accessing private fields and methods for testing purposes.
/// </summary>
public static class ReflectionTestHelper
{
    /// <summary>
    /// Sets a private field value on an object instance.
    /// </summary>
    /// <param name="target">The target object instance.</param>
    /// <param name="fieldName">The name of the private field.</param>
    /// <param name="value">The value to set.</param>
    public static void SetPrivateField(object target, string fieldName, object value)
    {
        if (target == null) throw new ArgumentNullException(nameof(target));
        if (string.IsNullOrEmpty(fieldName)) throw new ArgumentException("Field name cannot be null or empty", nameof(fieldName));

        var field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (field == null)
        {
            throw new InvalidOperationException($"Field '{fieldName}' not found on type '{target.GetType().Name}'");
        }

        field.SetValue(target, value);
    }

    /// <summary>
    /// Gets a private field value from an object instance.
    /// </summary>
    /// <typeparam name="T">The type of the field value.</typeparam>
    /// <param name="target">The target object instance.</param>
    /// <param name="fieldName">The name of the private field.</param>
    /// <returns>The field value.</returns>
    public static T GetPrivateField<T>(object target, string fieldName)
    {
        if (target == null) throw new ArgumentNullException(nameof(target));
        if (string.IsNullOrEmpty(fieldName)) throw new ArgumentException("Field name cannot be null or empty", nameof(fieldName));

        var field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (field == null)
        {
            throw new InvalidOperationException($"Field '{fieldName}' not found on type '{target.GetType().Name}'");
        }

        return (T)field.GetValue(target);
    }

    /// <summary>
    /// Invokes a private method on an object instance.
    /// </summary>
    /// <param name="target">The target object instance.</param>
    /// <param name="methodName">The name of the private method.</param>
    /// <param name="parameters">The method parameters.</param>
    /// <returns>The method return value.</returns>
    public static object InvokePrivateMethod(object target, string methodName, params object[] parameters)
    {
        if (target == null) throw new ArgumentNullException(nameof(target));
        if (string.IsNullOrEmpty(methodName)) throw new ArgumentException("Method name cannot be null or empty", nameof(methodName));

        var parameterTypes = parameters != null ? Array.ConvertAll(parameters, p => p?.GetType()) : Type.EmptyTypes;
        var method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance, null, parameterTypes, null);
        
        if (method == null)
        {
            throw new InvalidOperationException($"Method '{methodName}' not found on type '{target.GetType().Name}'");
        }

        return method.Invoke(target, parameters);
    }

    /// <summary>
    /// Invokes a private generic method on an object instance.
    /// </summary>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    /// <param name="target">The target object instance.</param>
    /// <param name="methodName">The name of the private method.</param>
    /// <param name="parameters">The method parameters.</param>
    /// <returns>The method return value.</returns>
    public static object InvokePrivateGenericMethod<T>(object target, string methodName, params object[] parameters)
    {
        if (target == null) throw new ArgumentNullException(nameof(target));
        if (string.IsNullOrEmpty(methodName)) throw new ArgumentException("Method name cannot be null or empty", nameof(methodName));

        var parameterTypes = parameters != null ? Array.ConvertAll(parameters, p => p?.GetType()) : Type.EmptyTypes;
        var method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (method == null)
        {
            throw new InvalidOperationException($"Method '{methodName}' not found on type '{target.GetType().Name}'");
        }

        var genericMethod = method.MakeGenericMethod(typeof(T));
        return genericMethod.Invoke(target, parameters);
    }
}