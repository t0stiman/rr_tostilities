using Game.Messages;
using Game.State;
using Model;
using UI.Builder;
using UI.CompanyWindow;

namespace tostilities.Extensions;

public static class UIPanelBuilder_Extensions
{
	public static bool AddTrainCrewDropdown_2(this UIPanelBuilder builder, Car car)
	{
		if(!car.IsOwnedByPlayer) // todo does this work if you're not the host
		{
			return false;
		}
		
		builder.AddTrainCrewDropdown("Set a train crew to associate a car with a specific job.", car.trainCrewId, StateManager.CheckAuthorizedToSendMessage(new SetCarTrainCrew(car.id, null)), proposedTrainCrewId =>
		{
			string trainCrewId = car.trainCrewId;
			if (proposedTrainCrewId == trainCrewId)
				return;
			StateManager.ApplyLocal(new SetCarTrainCrew(car.id, proposedTrainCrewId));
		});
		return true;
	}
}