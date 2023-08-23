# Neuron.Nexus

Neuron.Nexus is a .NET MAUI application designed to be an interpreter app using Microsoft Azure Cognitive Service Speech for translation. The application is targeted for both Android and iOS platforms, although the iOS version is not supported at the moment.

## Features

- **Android Support**: Fully functional on Android devices.
- **iOS Support**: Currently not available.
- **Translation Service**: Utilizes MS Azure Cognitive Service Speech for translation.

## Development Setup

To develop locally, you will need to create an `appsettings.DEV.json` file and replace the placeholders with the appropriate values.

### appsettings.DEV.json

\`\`\`json
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
\`\`\`

## Getting Started

1. **Clone the Repository**: Clone the repository from [here](https://github.com/AllramEst83/Neuron.Nexus).
2. **Set Up appsettings.DEV.json**: Follow the instructions above to set up the `appsettings.DEV.json` file.
3. **Build and Run**: Use your preferred development environment to build and run the application.

## Contributing

Feel free to contribute to the project by opening issues, creating pull requests, or suggesting new features.

## License

Please refer to the repository's license file for information on licensing.
