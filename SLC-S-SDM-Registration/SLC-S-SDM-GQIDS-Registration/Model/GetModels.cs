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
	using SLC_S_SDM_GQIDS_Registration.Model;
	using SLC_S_SDM_GQIDS_Registration.Shared;
	using SLDataGateway.API.Querying;

	[GQIMetaData(Name = "Registration.Get Models")]
	public sealed class GetModels : IGQIDataSource
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

		public OnPrepareFetchOutputArgs OnPrepareFetch(OnPrepareFetchInputArgs args)
		{
			var filter = new TRUEFilterElement<ModelRegistration>().ToQuery();

			_pageEnumerator = new GQIPageEnumerator(_sdmRegistrar.Models
				.ReadPaged(filter, 100).SelectMany(page => page.Select(CreateGQIRow)));

			return default;
		}

		public void OnStartUpdates(IGQIUpdater updater)
		{
			_updater = updater;
			_sdmRegistrar.Models.OnCreated += ModelCreated;
			_sdmRegistrar.Models.OnDeleted += ModelDeleted;
			_sdmRegistrar.Models.OnUpdated += ModelUpdated;
		}

		public GQIPage GetNextPage(GetNextPageInputArgs args)
		{
			return _pageEnumerator.GetNextPage(100);
		}

		public void OnStopUpdates()
		{
			_sdmRegistrar.Models.OnCreated -= ModelCreated;
			_sdmRegistrar.Models.OnDeleted -= ModelDeleted;
			_sdmRegistrar.Models.OnUpdated -= ModelUpdated;
		}

		private GQIRow CreateGQIRow(ModelRegistration model)
		{
			return new GQIRow(model.Guid.ToString(), new[]
			{
				new GQICell { Value = model.Guid.ToString() },
				new GQICell { Value = model.Name },
				new GQICell { Value = model.DisplayName },
				new GQICell { Value = model.ApiScriptName },
				new GQICell { Value = model.ApiEndpoint },
				new GQICell { Value = model.VisualizationEndpoint },
				new GQICell { Value = model.VisualizationCreateEndpoint },
				new GQICell { Value = model.VisualizationDeleteEndpoint },
				new GQICell { Value = model.VisualizationUpdateEndpoint },
				new GQICell { Value = model.Solution.Guid.ToString() },
				new GQICell { Value = model.Version },
			});
		}

		private void ModelCreated(object sender, SdmObjectEventArgs<ModelRegistration> e)
		{
			_updater?.AddRow(CreateGQIRow(e.Object));
		}

		private void ModelUpdated(object sender, SdmObjectEventArgs<ModelRegistration> e)
		{
			_updater?.UpdateRow(CreateGQIRow(e.Object));
		}

		private void ModelDeleted(object sender, SdmObjectEventArgs<ModelRegistration> e)
		{
			_updater?.RemoveRow(e.Object.Guid.ToString());
		}
	}
}
