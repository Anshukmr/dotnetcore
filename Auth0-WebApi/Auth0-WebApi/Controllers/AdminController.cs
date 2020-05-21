using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MentalVerse.Web.Api.DAL;
using MentalVerse.Web.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using MentalVerse.Web.Api.Interfaces;
using MentalVerse.Web.Api.Models.Request;
using AutoMapper;

namespace MentalVerse.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository<User> _userRepository;
        private readonly ICompanyRepository<Company> _companyRepository;
        private readonly IMapper _mapper;

        public AdminController(IUserRepository<User> userRepository, ICompanyRepository<Company> companyRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        // POST: api/Admin
        [HttpPost]
        [Route("user/{companyId}")]
        public ActionResult<User> AddUser(int companyId, UserInput user)
        {
            var companyInfo = _companyRepository.FindByCompanyID(companyId);

            if (companyInfo == null)
                return NotFound("Company Not Found.");

            if (_userRepository.FindByEmail(user.Email) != null) //UserExists
            {
                return Conflict();
            }

             var userDto = _mapper.Map<User>(user);
            userDto.CompanyID = companyId;
            _userRepository.Add(userDto);
            return CreatedAtAction("GetUser", companyId, user);
        }

        [HttpGet]
        [Route("user/{companyId}")]
        public ActionResult<IEnumerable<User>> GetUsers(int companyId)
        {
            return _userRepository.GetUser().ToList();
        }
    }

}
