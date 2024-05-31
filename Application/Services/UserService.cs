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
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private Mapper mapper;

        public UserService(IUserRepository userRepository, IOrganizationRepository organizationRepository)
        {
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));
        }

        public async Task<User> GetUserByEmailAndPassword(UserLoginDto userLoginDto)
        {
            return await _userRepository.Single(u => u.Email == userLoginDto.Email && u.Password == userLoginDto.Password);
        }

        public IEnumerable<OrganizationDto> GetOrganizationsByUser(int userId)
        {
            var orgs = _userRepository.GetOrganizationsByUser(userId);
            return mapper.Map<List<OrganizationDto>>(orgs);
        }

        public async Task CreateUser(UserCreateDto userCreateDto)
        {

            var userByEmail = _userRepository.Where(x => x.Email == userCreateDto.Email);

            if(userByEmail.Any())
            {
                throw new Exception($"Ya existe un usuario con el email: {userCreateDto.Email}");
            }
            var org = await _organizationRepository.Single(x => x.Id == userCreateDto.OrganizationId);
            if (org == null)
            {
                throw new Exception($"No existe una organizacion con id: {userCreateDto.OrganizationId}");
            }

            User user = new User();
            user.Email = userCreateDto.Email;
            user.Password = userCreateDto.Password;
            user.Name = userCreateDto.Name;
            
            await _userRepository.Create(user);

            _userRepository.AssingUserToOrg(user.Id, userCreateDto.OrganizationId);

        }

        public void AssingUserToOrg(int userId, int organizationId)
        {
            _userRepository.AssingUserToOrg(userId, organizationId);
        }


    }
}
