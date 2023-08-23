# Neuron.Nexus

Neuron.Nexus is a .NET MAUI application designed to be an interpreter app using Microsoft Azure Cognitive Service Speech for translation. The application is targeted for both Android and iOS platforms, although the iOS version is not supported at the moment.

## Features

- **Android Support**: Fully functional on Android devices.
- **iOS Support**: Currently not available.
- **Translation Service**: Utilizes MS Azure Cognitive Service Speech for translation.

## Development Environment

This project was developed using the following tools and technologies:

- **Visual Studio 2022**: The primary development environment used for coding, debugging, and testing.
- **.NET 7**: The project is built on the .NET 7 framework, utilizing its features and capabilities to create a robust and efficient application.

Ensure that you have these versions installed if you plan to contribute to or build the project locally.


## Development Setup

To develop locally, you will need to create an `appsettings.DEV.json` file and replace the placeholders with the appropriate values.

### appsettings.DEV.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "ApplicationSettings": {
    "AzureKeys": {
      "AzureSubscriptionKey": "AZURESUBSCRIPTIONKEY",
      "AzureRegion": "AZUREREGION"
    },
    "AppCenterSecret": "APPCENTERSECRET",
    "DeveloperEmail": "DEVELOPEREMAIL"
  }
}
```
## Getting Started

1. **Clone the Repository**: Clone the repository from [here](https://github.com/AllramEst83/Neuron.Nexus).
2. **Set Up appsettings.DEV.json**: Follow the instructions above to set up the `appsettings.DEV.json` file.
3. **Build and Run**: Use your preferred development environment to build and run the application.

## GitHub Build Action

This repository includes an automated build action that performs the following tasks:

- **Builds** the .NET MAUI application into an .aab file suitable for Android distribution.
- **Signs** the .aab file with the appropriate credentials to ensure authenticity.
- **Publishes** the signed .aab file to AppCenter, allowing for streamlined distribution and testing.

The build action is triggered when a commit or merge is made to the master branch, or it can be manually triggered as needed. This automation simplifies the development workflow and ensures that the application is consistently built and ready for deployment. Refer to the repository's build configuration files for specific details on how this process is configured.

## License

Please refer to the repository's license file for information on licensing.

## Contributing

Feel free to contribute to the project by opening issues, creating pull requests, or suggesting new features. Below is a list of Features and Bugs.

## Features and Bugs to be Fixed

### Features
- **Create Theme Selector**: Implement a theme selector to allow users to choose different visual themes.
- **Build Different Components for Tutorial Versions**: Construct various components for different versions of the tutorial, or create a resource for all text.
- **Review MainThread Usage**: Evaluate how the MainThread is being used within the application.
- **Add Attribution for MediaPlayer and Recorder**: Include proper attribution for the media player and recorder components.
- **Animate Button Wiggle and Switch Border Color**: Consider implementing animation for button wiggle and switch border color, possibly within the same message event.

### Bugs
- **Fix Connectivity Alert on Resume**: Ensure that the connectivity alert does not appear when returning to the start page after resuming the app.
- **Event Unsubscription on Sleep**: Determine whether events should also be unsubscribed when the app is put to sleep.
- **Handle App Resource Shutdown**: Manage the shutdown of app resources if audio or translation is working and the user clicks the back button. Consider implementing an ActivityIndicator.

