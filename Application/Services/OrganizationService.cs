using Application.Dtos;
using Application.Mappers;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private Mapper mapper;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
            mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));
        }

        public async 
        Task
CreateOrganization(OrganizationDto organizationCreateDto)
        {
            var existSlugTenant = _organizationRepository.Where(x => x.SlugTenant == organizationCreateDto.SlugTenant);
            if (existSlugTenant.Any())
            {
                throw new Exception($"Ya existe una organización con el SlugTenant: {organizationCreateDto.SlugTenant}");
            }

            Organization organization = new Organization();
            organization.Name = organizationCreateDto.Name;
            organization.SlugTenant = organizationCreateDto.SlugTenant;

            await _organizationRepository.Create(organization);
        }

        public IEnumerable<OrganizationDto> GetList()
        {
            var orgs = _organizationRepository.GetAll();
            var orgsDto = mapper.Map<List<OrganizationDto>>(orgs);
            return orgsDto;
        }
    }
}
