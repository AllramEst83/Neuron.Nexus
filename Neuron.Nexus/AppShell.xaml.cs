using Neuron.Nexus.Pages;

namespace Neuron.Nexus;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        // Register the routes for other pages
        Routing.RegisterRoute("SpeakPage", typeof(SpeakPage));

        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
    }

    private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var exception = (Exception)e.ExceptionObject;
        WriteToFile(exception);
    }

    private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        var exception = e.Exception;
        WriteToFile(exception);
    }

    private void WriteToFile(Exception exception)
    {
        var documents = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var filePath = Path.Combine(documents, "crashlog.txt");

        var logContent = $"[{DateTime.Now}] {exception.ToString()}\n\n";
        File.AppendAllText(filePath, logContent);
    }

}
