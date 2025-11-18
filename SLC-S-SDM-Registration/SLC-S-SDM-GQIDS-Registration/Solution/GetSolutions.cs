/*
****************************************************************************
*  Copyright (c) 2025,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

18/11/2025	1.0.0.1		AGE, Skyline	Initial version
****************************************************************************
*/

namespace Solution
{
	using System.Linq;
	using Skyline.DataMiner.Analytics.GenericInterface;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.SDM;
	using Skyline.DataMiner.SDM.Registration;
	using SLC_S_SDM_GQIDS_Registration.Shared;
	using SLC_S_SDM_GQIDS_Registration.Solution;
	using SLDataGateway.API.Querying;

	[GQIMetaData(Name = "Registration.Get Solution")]
	public sealed class GetSolution : IGQIDataSource
		, IGQIOnInit
		, IGQIUpdateable
		, IGQIOnPrepareFetch
	{
		private GQIDMS _dms;
		private Columns _columns;
		private SdmRegistrar _sdmRegistrar;
		private IGQIUpdater _updater;
		private IGQILogger _logger;

		private GQIPageEnumerator _pageEnumerator;

		public OnInitOutputArgs OnInit(OnInitInputArgs args)
		{
			_dms = args.DMS;
			_logger = args.Logger;
			_columns = new Columns();
			_sdmRegistrar = GqiExtensions.GetSdmRegistrar(args);
			return default;
		}

		public GQIColumn[] GetColumns()
		{
			return _columns.GetColumns();
		}

		public void OnStartUpdates(IGQIUpdater updater)
		{
			_updater = updater;
			_sdmRegistrar.Solutions.OnCreated += SolutionCreated;
			_sdmRegistrar.Solutions.OnDeleted += SolutionDeleted;
			_sdmRegistrar.Solutions.OnUpdated += SolutionUpdated;
		}

		public GQIPage GetNextPage(GetNextPageInputArgs args)
		{
			return _pageEnumerator.GetNextPage(100);
		}

		public OnPrepareFetchOutputArgs OnPrepareFetch(OnPrepareFetchInputArgs args)
		{
			var filter = new TRUEFilterElement<SolutionRegistration>().ToQuery();

			_pageEnumerator = new GQIPageEnumerator(_sdmRegistrar.Solutions
				.ReadPaged(filter, 100).SelectMany(page => page.Select(CreateGQIRow)));

			return default;
		}

		public void OnStopUpdates()
		{
			_sdmRegistrar.Solutions.OnCreated -= SolutionCreated;
			_sdmRegistrar.Solutions.OnDeleted -= SolutionDeleted;
			_sdmRegistrar.Solutions.OnUpdated -= SolutionUpdated;
		}

		private GQIRow CreateGQIRow(SolutionRegistration solution)
		{
			return new GQIRow(solution.Guid.ToString(), new[]
			{
				new GQICell { Value = solution.Guid.ToString() },
				new GQICell { Value = solution.ID },
				new GQICell { Value = solution.DisplayName },
				new GQICell { Value = solution.DefaultApiEndpoint },
				new GQICell { Value = solution.DefaultApiScriptName },
				new GQICell { Value = solution.VisualizationEndpoint },
				new GQICell { Value = solution.VisualizationCreateEndpoint },
				new GQICell { Value = solution.VisualizationDeleteEndpoint },
				new GQICell { Value = solution.VisualizationUpdateEndpoint },
				new GQICell { Value = solution.Version },
			});
		}

		private void SolutionCreated(object sender, SdmObjectEventArgs<SolutionRegistration> e)
		{
			_updater?.AddRow(CreateGQIRow(e.Object));
		}

		private void SolutionUpdated(object sender, SdmObjectEventArgs<SolutionRegistration> e)
		{
			_updater?.UpdateRow(CreateGQIRow(e.Object));
		}

		private void SolutionDeleted(object sender, SdmObjectEventArgs<SolutionRegistration> e)
		{
			_updater?.RemoveRow(e.Object.ID);
		}
	}
}
