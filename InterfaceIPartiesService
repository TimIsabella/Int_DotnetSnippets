using Snippet.Models;
using Snippet.Models.Domain.Parties;
using Snippet.Models.Requests.Parties;

namespace Snippet.Services.Interfaces.Parties
{
	public interface IPartiesService
	{
		Paged<Party> GetSearchPaginatedALL(int pageIndex, int pageSize, string query);
		Paged<Party> GetSearchPaginatedCoalition(int pageIndex, int pageSize, string query);
		Paged<Party> GetSearchPaginatedNonCoalition(int pageIndex, int pageSize, string query);
		Party GetByIdAll(int id);
		int AddParty(PartyAddRequest model);
		void UpdateParty(PartyUpdateRequest model, int Id);
	}
}
