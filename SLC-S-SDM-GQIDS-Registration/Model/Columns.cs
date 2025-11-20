namespace SLC_S_SDM_GQIDS_Registration.Model
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
			[new GQIStringColumn("Guid")] = typeof(ModelRegistration).GetProperty(nameof(ModelRegistration.Guid)),
			[new GQIStringColumn("Name")] = typeof(ModelRegistration).GetProperty(nameof(ModelRegistration.Name)),
			[new GQIStringColumn("Display Name")] = typeof(ModelRegistration).GetProperty(nameof(ModelRegistration.DisplayName)),
			[new GQIStringColumn("Default API Script Name")] = typeof(ModelRegistration).GetProperty(nameof(ModelRegistration.ApiScriptName)),
			[new GQIStringColumn("API Endpoint")] = typeof(ModelRegistration).GetProperty(nameof(ModelRegistration.ApiEndpoint)),
			[new GQIStringColumn("Visualization Endpoint")] = typeof(ModelRegistration).GetProperty(nameof(ModelRegistration.VisualizationEndpoint)),
			[new GQIStringColumn("Visualization Create Endpoint")] = typeof(ModelRegistration).GetProperty(nameof(ModelRegistration.VisualizationCreateEndpoint)),
			[new GQIStringColumn("Visualization Delete Endpoint")] = typeof(ModelRegistration).GetProperty(nameof(ModelRegistration.VisualizationDeleteEndpoint)),
			[new GQIStringColumn("Visualization Update Endpoint")] = typeof(ModelRegistration).GetProperty(nameof(ModelRegistration.VisualizationUpdateEndpoint)),
			[new GQIStringColumn("Solution")] = typeof(ModelRegistration).GetProperty(nameof(ModelRegistration.Solution)),
			[new GQIStringColumn("Version")] = typeof(ModelRegistration).GetProperty(nameof(ModelRegistration.Version)),
		};

		internal GQIColumn[] GetColumns() => _columnMap.Keys.ToArray();
	}
}
