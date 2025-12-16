// ViewModels/AboutViewModel.cs
using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KarpineRfid.App.ViewModels
{
    public partial class AboutViewModel : ObservableObject
    {
        public string Version { get; } = "1.0.0";
        public string Build { get; } = "dev";
        public string SupportEmail { get; } = "support@karpine.example";
    }
}

