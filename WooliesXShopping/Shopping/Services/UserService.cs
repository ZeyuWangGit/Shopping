using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shopping.Models;

namespace Shopping.Services
{
    public class UserService: IUserService
    {
        private readonly Resource _resource;
        public UserService(IOptions<Resource> resourceOptions)
        {
            _resource = resourceOptions.Value;
        }

        public UserInfo GetUserInfo()
        {
            return new UserInfo
            {
                Name = _resource.Name,
                Token = _resource.Token
            };
        }
    }
}
