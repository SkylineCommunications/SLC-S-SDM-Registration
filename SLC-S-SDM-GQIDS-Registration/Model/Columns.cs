namespace SLC_S_SDM_GQIDS_Registration.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Skyline.DataMiner.Analytics.GenericInterface;
	using Skyline.DataMiner.Analytics.GenericInterface.Operators;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.Ticketing;
	using Skyline.DataMiner.SDM.Registration;
	using SLDataGateway.API.Querying;
	using SLDataGateway.API.Types.Querying;

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

		internal IQuery<ModelRegistration> ApplySorting(FilterElement<ModelRegistration> filter, IGQISortField sortField)
		{
			return ApplySorting(filter.ToQuery(), sortField);
		}

		internal IQuery<ModelRegistration> ApplySorting(IQuery<ModelRegistration> query, IGQISortField sortField)
		{
			if (query is null)
			{
				throw new ArgumentNullException(nameof(query));
			}

			if (sortField is null)
			{
				return query;
			}

			SortOrder sortDirection;
			switch (sortField.Direction)
			{
				case GQISortDirection.Ascending:
					sortDirection = SortOrder.Ascending;
					break;

				case GQISortDirection.Descending:
					sortDirection = SortOrder.Descending;
					break;

				default:
					throw new NotSupportedException($"The sort direction '{sortField.Direction}' is not supported.");
			}

			var member = _columnMap.FirstOrDefault(map => sortField.Column.Equals(map.Key)).Value;
			if (member is null)
			{
				return query;
			}

			var orderByElement = ModelRegistrationExposers.CreateOrderBy(member, sortDirection);
			if (!query.Order.Elements.Any())
			{
				return query.WithOrder(
					OrderBy.Default.SingleConcat(orderByElement));
			}
			else
			{
				return query.WithOrder(
					query.Order.SingleConcat(orderByElement));
			}
		}
	}
}
