// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", 
    Justification = "Sample application demonstrates exception handling patterns", 
    Scope = "module")]

[assembly: SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", 
    Justification = "Sample application prioritizes readability over performance", 
    Scope = "module")]

[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", 
    Justification = "Sample application uses default culture for demonstration", 
    Scope = "module")]

[assembly: SuppressMessage("Design", "CA1034:Nested types should not be visible", 
    Justification = "ViewModel classes are appropriately scoped", 
    Scope = "module")]