using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Snippet.Models;
using Snippet.Models.Domain.Parties;
using Snippet.Models.Requests.Parties;
using Snippet.Services.Interfaces.Parties;
using Snippet.Web.Controllers;
using Snippet.Web.Models.Responses;
using System;

namespace Snippet.Web.Api.Controllers.Parties
{
	[Route("api/parties")]
	[ApiController]
	public class PartiesApiController : BaseApiController
	{
		private IPartiesService _service = null;

		public PartiesApiController(IPartiesService service, ILogger<PartiesApiController> logger) : base(logger)
		{
			_service = service;
		}

		[HttpGet("search/all")]
		public ActionResult<ItemsResponse<Paged<Party>>> GetSearchPaginatedALL(int pageIndex, int pageSize, string query)
		{
			ActionResult result = null;
			try
			{
				Paged<Party> paged = _service.GetSearchPaginatedALL(pageIndex, pageSize, query);

				if(paged == null)
				{
					result = NotFound404(new ErrorResponse("Record not found"));
				}
				else
				{
					ItemResponse<Paged<Party>> response = new ItemResponse<Paged<Party>>();
					response.Item = paged;
					result = Ok200(response);
				}
			}
			catch(Exception ex)
			{
				Logger.LogError(ex.ToString());
				result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
			}

			return result;
		}

		[HttpGet("search/coalition")]
		public ActionResult<ItemsResponse<Paged<Party>>> GetSearchPaginatedCoalition(int pageIndex, int pageSize, string query)
		{
			ActionResult result = null;
			try
			{
				Paged<Party> paged = _service.GetSearchPaginatedCoalition(pageIndex, pageSize, query);

				if(paged == null)
				{
					result = NotFound404(new ErrorResponse("Record not found"));
				}
				else
				{
					ItemResponse<Paged<Party>> response = new ItemResponse<Paged<Party>>();
					response.Item = paged;
					result = Ok200(response);
				}
			}
			catch(Exception ex)
			{
				Logger.LogError(ex.ToString());
				result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
			}

			return result;
		}

		[HttpGet("search/noncoalition")]
		public ActionResult<ItemsResponse<Paged<Party>>> GetSearchPaginatedNonCoalition(int pageIndex, int pageSize, string query)
		{
			ActionResult result = null;
			try
			{
				Paged<Party> paged = _service.GetSearchPaginatedNonCoalition(pageIndex, pageSize, query);

				if(paged == null)
				{
					result = NotFound404(new ErrorResponse("Record not found"));
				}
				else
				{
					ItemResponse<Paged<Party>> response = new ItemResponse<Paged<Party>>();
					response.Item = paged;
					result = Ok200(response);
				}
			}
			catch(Exception ex)
			{
				Logger.LogError(ex.ToString());
				result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
			}

			return result;
		}

		[HttpGet("{id:int}")]
		public ActionResult<ItemsResponse<Party>> GetByIdAll(int id)
		{
			int iCode = 200;
			BaseResponse response = null;

			try
			{
				Party party = _service.GetByIdAll(id);

				if(party == null)
				{
					iCode = 404;
					response = new ErrorResponse("Record not found");
				}
				else
				{
					response = new ItemResponse<Party> { Item = party };
				}
			}

			catch(Exception ex)
			{
				iCode = 500;
				base.Logger.LogError(ex.ToString());
				response = new ErrorResponse($"ArgumentException Error 500: {ex.Message}");
			}

			return StatusCode(iCode, response);
		}

		[HttpPost]
		public ActionResult<ItemResponse<int>> AddParty(PartyAddRequest model)
		{
			ObjectResult result = null;

			try
			{
				int id = _service.AddParty(model);
				ItemResponse<int> response = new ItemResponse<int>() { Item = id };

				result = Created201(response);
			}
			catch(Exception ex)
			{
				Logger.LogError(ex.ToString());
				ErrorResponse response = new ErrorResponse(ex.Message);

				result = StatusCode(500, response);
			}

			return result;
		}

		[HttpPut("{id:int}")]
		public ActionResult<SuccessResponse> UpdateParty(PartyUpdateRequest model, int Id)
		{
			int code = 200;
			BaseResponse response = null;

			try
			{
				_service.UpdateParty(model, Id);
				response = new SuccessResponse();
			}
			catch(Exception ex)
			{
				code = 500;
				response = new ErrorResponse(ex.Message);
			}

			return StatusCode(code, response);
		}
	}
}
