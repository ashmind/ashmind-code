using System;
using System.Collections;
using System.Diagnostics;
using System.Resources;

// namespace skipped to simplify reusing

internal static class Argument
{
    private static readonly ResourceManager resourceManager = new ResourceManager(typeof(Argument));

    [DebuggerHidden]
    public static void VerifyNotNull(string name, object value)
    {
        if (value == null)
            throw new ArgumentNullException(name);
    }

    [DebuggerHidden]
    public static void VerifyNotEmpty(string name, string value)
    {
        if (value.Length == 0)
            throw new ArgumentException(Argument.CannotBeEmpty, name);
    }

    [DebuggerHidden]
    public static void VerifyNotEmpty(string name, Array array)
    {
        if (array.Length == 0)
            throw new ArgumentException(Argument.CannotBeEmpty, name);
    }

    [DebuggerHidden]
    public static void VerifyNotEmpty(string name, ICollection collection)
    {
        if (collection.Count == 0)
            throw new ArgumentException(Argument.CannotBeEmpty, name);
    }

    [DebuggerHidden]
    public static void VerifyNotNullOrEmpty(string name, string value)
    {
        Argument.VerifyNotNull(name, value);
        Argument.VerifyNotEmpty(name, value);
    }

    [DebuggerHidden]
    public static void VerifyNotNullOrEmpty(string name, Array array)
    {
        Argument.VerifyNotNull(name, array);
        Argument.VerifyNotEmpty(name, array);
    }

    [DebuggerHidden]
    public static void VerifyNotNullOrEmpty(string name, ICollection collection)
    {
        Argument.VerifyNotNull(name, collection);
        Argument.VerifyNotEmpty(name, collection);
    }

    [DebuggerHidden]
    public static T CastAndVerify<T>(string name, object value) {
        if (!(value is T))
            throw new ArgumentException(Argument.FormatWrongType(value, typeof(T)), name);

        return (T)value;
    }

    [DebuggerHidden]
    public static void CheckBuffer(string name, Array buffer, int offset, int count)
    {
        Argument.VerifyNotNullOrEmpty(name, buffer);

        if(count <= 0)
            throw new ArgumentOutOfRangeException(Argument.MustBePositive);

        if(offset < 0)
            throw new ArgumentOutOfRangeException(Argument.MustBePositive);

        if(buffer.Length - offset < count)
            throw new ArgumentException(Argument.InsufficientBuffer);
    }

    private static string CannotBeEmpty
    {
        get { return Argument.GetString("Argument.CannotBeEmpty"); }
    }

    private static string MustBePositive
    {
        get { return Argument.GetString("Argument.MustBePositive"); }
    }

    private static string InsufficientBuffer
    {
        get { return Argument.GetString("Argument.InsufficientBuffer"); }
    }

    public static string FormatWrongType(object value, Type type)
    {
        string template = Argument.GetString("Argument.WrongType");

        return String.Format(template, value, type);
    }

    public static string FormatWrongKeyType(object value, Type type)
    {
        string template = Argument.GetString("Argument.WrongKeyType");

        return String.Format(template, value, type);
    }

    private static string GetString(string key)
    {
        return resourceManager.GetString(key);
    }
}