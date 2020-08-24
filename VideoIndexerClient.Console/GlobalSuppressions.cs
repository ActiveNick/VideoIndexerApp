// <copyright file="GlobalSuppressions.cs" company="Microsoft Corp">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Allowed as a final fallback after capturing more specific exceptions", Scope = "member", Target = "~M:VideoIndexerClient.DemoConsole.Program.ConnectToAccount(System.String,System.String,System.String)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Allowed as a final fallback after capturing more specific exceptions", Scope = "member", Target = "~M:VideoIndexerClient.DemoConsole.Program.SaveThumbnailsFromVideoShots(System.String)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Allowed in sample console apps", Scope = "member", Target = "~M:VideoIndexerClient.DemoConsole.Program.Main(System.String[])")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Allowed in sample console apps", Scope = "member", Target = "~M:VideoIndexerClient.DemoConsole.Program.ConnectToAccount(System.String,System.String,System.String)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "blob container names specifically do not allow uppercase characters", Scope = "member", Target = "~M:VideoIndexerClient.DemoConsole.Program.SaveThumbnailsFromVideoShots(System.String)~System.Threading.Tasks.Task")]
