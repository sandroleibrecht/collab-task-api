using CollabTask.ConsoleClient.Services.Interfaces;
using CollabTask.ConsoleClient.UI.Interfaces;

namespace CollabTask.ConsoleClient.Services;

public class ViewService : IViewService
{
	public async Task ShowView(IView view)
	{
		await view.Show();
	}
}
