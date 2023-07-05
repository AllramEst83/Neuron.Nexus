﻿using Neuron.Nexus.Pages;
using Neuron.Nexus.Services;
using Neuron.Nexus.ViewModels;

namespace Neuron.Nexus;

public partial class MainPage : ContentPage
{
    // Constructor for the MainPage class.
    public MainPage()
    {
        // Call to InitializeComponent method generated by XAML compiler to create and connect
        // all the defined UI components and event handlers. This should always be the first line in the constructor.
        InitializeComponent();

        // Create a new instance of the NavigationService class, passing the Navigation property of the MainPage
        // (which is inherited from the Page class and represents the navigation logic of a page).
        // NavigationService class is used to abstract away the navigation logic from the ViewModel.
        var navigationService = new NavigationService(Navigation);

        // Create a new instance of the MainPageViewModel, passing in the navigationService instance.
        // This applies the dependency injection principle, keeping the ViewModel decoupled from the concrete implementation
        // of the NavigationService and making the ViewModel easier to test.
        var viewModel = new MainPageViewModel(navigationService);

        // BindingContext is not set in your provided code. Typically, it is set in the next step like this:
        // BindingContext = viewModel; 
        // This assigns the ViewModel instance to the BindingContext of the MainPage,
        // and is necessary for the data bindings defined in the XAML to work correctly.
    }

}

