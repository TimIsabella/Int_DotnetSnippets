using Newtonsoft.Json;
using Snippet.Data;
using Snippet.Data.Providers;
using Snippet.Models;
using Snippet.Models.Domain.Parties;
using Snippet.Models.Requests.Parties;
using Snippet.Services.Interfaces.Parties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Snippet.Services.Parties
{
	public class PartiesService : IPartiesService
	{
		IDataProvider _data = null;
		public PartiesService(IDataProvider data) { _data = data; }

		public Paged<Party> GetSearchPaginatedALL(int pageIndex, int pageSize, string query)
		{
			Party model = null;
			Paged<Party> pagedList = null;
			List<Party> list = null;
			int totalCount = 0;
			if(string.IsNullOrEmpty(query)) { query = ""; }

			_data.ExecuteCmd("[dbo].[Parties_Search_All]",
			inputParamMapper: delegate (SqlParameterCollection parameterCollection)
			{
				parameterCollection.AddWithValue("@Index", pageIndex);
				parameterCollection.AddWithValue("@PageSize", pageSize);
				parameterCollection.AddWithValue("@Query", query);
			},
			singleRecordMapper: delegate (IDataReader reader, short set)
			{
				int index = 0;

				model = SelectMapper(reader, ref index);
				if(totalCount == 0) { totalCount = reader.GetSafeInt32(index++); }

				if(list == null) {list = new List<Party>(); }

				list.Add(model);
			});

			if(list != null) { pagedList = new Paged<Party>(list, pageIndex, pageSize, totalCount); }

			return pagedList;
		}

		public Paged<Party> GetSearchPaginatedCoalition(int pageIndex, int pageSize, string query)
		{
			Party model = null;
			Paged<Party> pagedList = null;
			List<Party> list = null;
			int totalCount = 0;
			if(string.IsNullOrEmpty(query)) { query = ""; }

			_data.ExecuteCmd("[dbo].[Parties_Search_Coalition]",
			inputParamMapper: delegate (SqlParameterCollection parameterCollection)
			{
				parameterCollection.AddWithValue("@Index", pageIndex);
				parameterCollection.AddWithValue("@PageSize", pageSize);
				parameterCollection.AddWithValue("@Query", query);
			},
			singleRecordMapper: delegate (IDataReader reader, short set)
			{
				int index = 0;

				model = SelectMapper(reader, ref index);
				if(totalCount == 0) { totalCount = reader.GetSafeInt32(index++); }

				if(list == null)
				{ list = new List<Party>(); }

				list.Add(model);
			});

			if(list != null)
			{ pagedList = new Paged<Party>(list, pageIndex, pageSize, totalCount); }

			return pagedList;
		}

		public Paged<Party> GetSearchPaginatedNonCoalition(int pageIndex, int pageSize, string query)
		{
			Party model = null;
			Paged<Party> pagedList = null;
			List<Party> list = null;
			int totalCount = 0;
			if(string.IsNullOrEmpty(query)) { query = ""; }

			_data.ExecuteCmd("[dbo].[Parties_Search_NonCoalition]",
			inputParamMapper: delegate (SqlParameterCollection parameterCollection)
			{
				parameterCollection.AddWithValue("@Index", pageIndex);
				parameterCollection.AddWithValue("@PageSize", pageSize);
				parameterCollection.AddWithValue("@Query", query);
			},
			singleRecordMapper: delegate (IDataReader reader, short set)
			{
				int index = 0;

				model = SelectMapper(reader, ref index);
				if(totalCount == 0) { totalCount = reader.GetSafeInt32(index++); }

				if(list == null)
				{ list = new List<Party>(); }

				list.Add(model);
			});

			if(list != null)
			{ pagedList = new Paged<Party>(list, pageIndex, pageSize, totalCount); }

			return pagedList;
		}

		public Party GetByIdAll(int id)
		{
			Party model = null;

			_data.ExecuteCmd("[dbo].[Parties_Select_ById_All]",
			delegate (SqlParameterCollection paramsCollection)
			{
				paramsCollection.AddWithValue("@SelectId", id);
			},
			delegate (IDataReader reader, short set)
			{
				int index = 0;
				model = SelectMapper(reader, ref index);
			});

			return model;
		}

		public int AddParty(PartyAddRequest model)
		{
			DataTable paramameterValue = PartyBaseBatchMapper(model.PartyBase);
			int id = 0;

			_data.ExecuteNonQuery("[dbo].[Parties_Insert]",
			inputParamMapper: delegate (SqlParameterCollection parameterCollection)
			{
				ParameterMapper(model, parameterCollection);
				parameterCollection.AddWithValue("@partyCompositeUDT", paramameterValue);

				SqlParameter idOutput = new SqlParameter("@Id", SqlDbType.Int);
				idOutput.Direction = ParameterDirection.Output;
				parameterCollection.Add(idOutput);
			},
			returnParameters: delegate (SqlParameterCollection returnCollection)
			{
				object idReturned = returnCollection["@Id"].Value;
				int.TryParse(idReturned.ToString(), out id);
			});

			return id;
		}

		public void UpdateParty(PartyUpdateRequest model, int Id)
		{
			DataTable paramameterValue = PartyBaseBatchMapper(model.PartyBase);

			_data.ExecuteNonQuery("[dbo].[Parties_Update]",
			inputParamMapper: delegate (SqlParameterCollection parameterCollection)
			{
				parameterCollection.AddWithValue("@Id", model.Id);
				ParameterMapper(model, parameterCollection);
				parameterCollection.AddWithValue("@partyCompositeUDT", paramameterValue);
			},

			returnParameters: null);
		}

	private static Party SelectMapper(IDataReader reader, ref int index)
		{
			Party model = new Party();

			model.Id = reader.GetSafeInt32(index++);
			model.Name = reader.GetSafeString(index++);
			model.Code = reader.GetSafeString(index++);
			model.Logo = reader.GetSafeString(index++);
			model.SiteUrl = reader.GetSafeString(index++);
			model.ColorHEX = reader.GetSafeString(index++);
			model.StatusId = reader.GetSafeInt32(index++);
			model.StatusName = reader.GetSafeString(index++);
			model.RegionTypeId = reader.GetSafeInt32(index++);
			model.RegionName = reader.GetSafeString(index++);
			model.LocationId = reader.GetSafeInt32(index++);
			model.LocationLineOne = reader.GetSafeString(index++);
			model.IsCoalition = reader.GetSafeBool(index++);
			model.RegistrationDate = reader.GetSafeUtcDateTime(index++);

			string MembersJson = reader.GetSafeString(index++);
			if(!string.IsNullOrEmpty(MembersJson))
			{
				model.Members = JsonConvert.DeserializeObject<List<PartyBase>>(MembersJson);
			}

			return model;
		}

		private static void ParameterMapper(PartyAddRequest model, SqlParameterCollection parameterCollection)
		{
			parameterCollection.AddWithValue("@Name", model.Name);
			parameterCollection.AddWithValue("@Code", model.Code);
			parameterCollection.AddWithValue("@Logo", model.Logo);
			parameterCollection.AddWithValue("@SiteUrl", model.SiteUrl);
			parameterCollection.AddWithValue("@ColorHEX", model.ColorHEX);
			parameterCollection.AddWithValue("@StatusId", model.StatusId);
			parameterCollection.AddWithValue("@RegionTypeId", model.RegionTypeId);
			parameterCollection.AddWithValue("@LocationId", model.LocationId);
			parameterCollection.AddWithValue("@IsCoalition", model.IsCoalition);
		}

		private DataTable PartyBaseBatchMapper(List<int> batchListToMap)
		{
			DataTable dataTable = new DataTable();

			dataTable.Columns.Add("PartyId", typeof(Int32));

			foreach(int singleRecord in batchListToMap)
			{
				DataRow dataRow = dataTable.NewRow();

				dataRow.SetField(0, singleRecord);
				dataTable.Rows.Add(dataRow);
			}

			return dataTable;
		}
	}
}
