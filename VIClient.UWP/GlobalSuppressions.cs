// <copyright file="GlobalSuppressions.cs" company="Microsoft Corp">
// Copyright (c) Microsoft Corp. All rights reserved.
// </copyright>

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Allowed as a final fallback after capturing more specific exceptions", Scope = "member", Target = "~M:VideoIndexerClient.MainPage.BtnConnect_Click(System.Object,Windows.UI.Xaml.RoutedEventArgs)")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Allowed as a final fallback after capturing more specific exceptions", Scope = "member", Target = "~M:VideoIndexerClient.MainPage.BtnGetVideos_Click(System.Object,Windows.UI.Xaml.RoutedEventArgs)")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Allowed as a final fallback after capturing more specific exceptions", Scope = "member", Target = "~M:VideoIndexerClient.MainPage.DisplayVideoData(VideoIndexerLibrary.Video)")]
