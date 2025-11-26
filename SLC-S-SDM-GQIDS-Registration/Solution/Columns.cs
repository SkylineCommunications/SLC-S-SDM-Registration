namespace SLC_S_SDM_GQIDS_Registration.Solution
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

		internal IQuery<SolutionRegistration> ApplySorting(FilterElement<SolutionRegistration> filter, IGQISortField sortField)
		{
			return ApplySorting(filter.ToQuery(), sortField);
		}

		internal IQuery<SolutionRegistration> ApplySorting(IQuery<SolutionRegistration> query, IGQISortField sortField)
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

			var orderByElement = SolutionRegistrationExposers.CreateOrderBy(member, sortDirection);
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
