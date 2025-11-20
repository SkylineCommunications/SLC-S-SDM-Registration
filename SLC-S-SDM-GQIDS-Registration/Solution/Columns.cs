namespace SLC_S_SDM_GQIDS_Registration.Solution
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Skyline.DataMiner.Analytics.GenericInterface;
	using Skyline.DataMiner.SDM.Registration;

	internal class Columns
	{
		private readonly Dictionary<GQIColumn, MemberInfo> _columnMap = new Dictionary<GQIColumn, MemberInfo>
		{
			[new GQIStringColumn("Guid")] = typeof(SolutionRegistration).GetProperty(nameof(SolutionRegistration.Guid)),
			[new GQIStringColumn("ID")] = typeof(SolutionRegistration).GetProperty(nameof(SolutionRegistration.ID)),
			[new GQIStringColumn("Display Name")] = typeof(SolutionRegistration).GetProperty(nameof(SolutionRegistration.DisplayName)),
			[new GQIStringColumn("Default API Endpoint")] = typeof(SolutionRegistration).GetProperty(nameof(SolutionRegistration.DefaultApiEndpoint)),
			[new GQIStringColumn("Default API Script Name")] = typeof(SolutionRegistration).GetProperty(nameof(SolutionRegistration.DefaultApiScriptName)),
			[new GQIStringColumn("Visualization Endpoint")] = typeof(SolutionRegistration).GetProperty(nameof(SolutionRegistration.VisualizationEndpoint)),
			[new GQIStringColumn("Visualization Create Endpoint")] = typeof(SolutionRegistration).GetProperty(nameof(SolutionRegistration.VisualizationCreateEndpoint)),
			[new GQIStringColumn("Visualization Delete Endpoint")] = typeof(SolutionRegistration).GetProperty(nameof(SolutionRegistration.VisualizationDeleteEndpoint)),
			[new GQIStringColumn("Visualization Update Endpoint")] = typeof(SolutionRegistration).GetProperty(nameof(SolutionRegistration.VisualizationUpdateEndpoint)),
			[new GQIStringColumn("Version")] = typeof(SolutionRegistration).GetProperty(nameof(SolutionRegistration.Version)),
		};

		internal GQIColumn[] GetColumns() => _columnMap.Keys.ToArray();
	}
}
