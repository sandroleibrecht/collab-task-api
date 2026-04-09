using CollabTask.ConsoleClient.UI.Interfaces;

namespace CollabTask.ConsoleClient.Services.Interfaces;

public interface IViewService
{
	Task ShowView(IView view);
}
