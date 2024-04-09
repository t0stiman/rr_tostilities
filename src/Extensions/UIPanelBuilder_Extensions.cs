using System.Collections.Generic;
using System.Linq;
using Model;
using UI.Builder;
using UI.CompanyWindow;

namespace tostilities.Extensions;

public static class UIPanelBuilder_Extensions
{
	public static bool AddTrainCrewDropdown2(this UIPanelBuilder builder, Car car)
	{
		if(!car.IsOwnedByPlayer) // todo does this work if ur not the host
		{
			return false;
		}

		var trainCrews = BuilderExtensions.PlayersManager.TrainCrews;
		var crewNames = new List<string> { "None" };
		crewNames.AddRange(trainCrews.Select(crew => crew.Name));
		
		var trainCrewId = car.trainCrewId;
		var crewIndex = BuilderExtensions.FindIndex(trainCrews, trainCrewId);
		
		builder.AddField(
			"Train Crew", 
			builder.AddDropdown(
				crewNames, 
				crewIndex + 1, 
				i => BuilderExtensions.TrainCrewDropdownDidChange(car, i)
				)
			)
			.Tooltip("Train Crew", "Set a train crew to associate a car with a specific job.");
		
		return true;
	}
}