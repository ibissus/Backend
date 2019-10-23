using KompaniaPchor.ORM_Models;
using KompaniaPchor.Repositories;
using KompaniaPchor.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.Services
{
    public class RequestService : IRequestService
    {
        private readonly GenericRepo<Prosba> _requestRepo;
        private readonly ICompanyService _companyService;
        private readonly IPlatoonService _platoonService;

        public RequestService(GenericRepo<Prosba> requestRepo, ICompanyService companyService, IPlatoonService platoonService)
        {
            _requestRepo = requestRepo;
            _companyService = companyService;
            _platoonService = platoonService;
        }
        public async Task AcceptRequest(int requestId, bool accepted)
        {
            var request = await _requestRepo.Get().Where(r => r.IdProsby == requestId).SingleOrDefaultAsync();

            switch (request.TypProsby)
            {
                case TypProsby.JC: await _companyService.AcceptRequest(request, accepted);
                    break;
                case TypProsby.JP: await _platoonService.AcceptRequest(request, accepted);
                    break;
                case TypProsby.PA: await _platoonService.AcceptRequest(request, accepted);
                    break;
                case TypProsby.PC: await _platoonService.AssignNewPlatoonCommander(request, accepted);
                    break;
            }

            return;
        }
    }
}
