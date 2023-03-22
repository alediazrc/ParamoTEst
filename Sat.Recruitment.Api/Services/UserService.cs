using Sat.Recruitment.Api.Model;
using System.Diagnostics;
using System.Net;
using System.Security.Policy;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Sat.Recruitment.Api.Enums;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Sat.Recruitment.Api.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new List<User>();
        
        public Result CreateUser(User newUser)
        {
            var errors = "";

            ValidateErrors(newUser, ref errors);

            if (errors != null && errors != "")
                return new Result()
                {
                    IsSuccess = false,
                    Errors = errors
                };

           
            newUser.Money = Calculate(newUser.UserType, newUser.Money);

            var reader = ReadUsersFromFile();

            //Normalize email
            
            var aux = newUser.Email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

            newUser.Email = string.Join("@", new string[] { aux[0], aux[1] });

            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLineAsync().Result;
                var user = new User
                {
                    Name = line.Split(',')[0].ToString(),
                    Email = line.Split(',')[1].ToString(),
                    Phone = line.Split(',')[2].ToString(),
                    Address = line.Split(',')[3].ToString(),
                    UserType = line.Split(',')[4].ToString(),
                    Money = decimal.Parse(line.Split(',')[5].ToString()),
                };
                _users.Add(user);
            }
            reader.Close();
            try
            {
                var isDuplicated = false;
                foreach (var user in _users)
                {
                    if (user.Email == newUser.Email
                        ||
                        user.Phone == newUser.Phone)
                    {
                        isDuplicated = true;
                    }
                    else if (user.Name == newUser.Name)
                    {
                        if (user.Address == newUser.Address)
                        {
                            isDuplicated = true;
                            throw new Exception("User is duplicated");
                        }

                    }
                }

                if (!isDuplicated)
                {
                    Debug.WriteLine("User Created");

                    return new Result()
                    {
                        IsSuccess = true,
                        Errors = "User Created"
                    };
                }
                else
                {
                    Debug.WriteLine("The user is duplicated");

                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = "The user is duplicated"
                    };
                }
            }
            catch
            {
                Debug.WriteLine("The user is duplicated");
                return new Result()
                {
                    IsSuccess = false,
                    Errors = "The user is duplicated"
                };
            }
        }
        private decimal Calculate(string userType, decimal money) 
        {
            switch (userType)
            {
                case "Normal":
                    if (money > 100)
                    {
                        var percentage = Convert.ToDecimal(0.12);
                        //If new user is normal and has more than USD100
                        var gif = money * percentage;
                        money = money + gif;
                    }
                    else if (money > 10)
                    {
                        var percentage = Convert.ToDecimal(0.8);
                        var gif = money * percentage;
                        money = money + gif;
                    }
                    break;
                case "SuperUser":
                    if (money > 100)
                    {
                        var percentage = Convert.ToDecimal(0.20);
                        var gif = money * percentage;
                        money = money + gif;
                    }
                    break;
                default:
                    if (money > 100)
                    {
                        var gif = money * 2;
                        money = money + gif;
                    }
                    break;
            }

            return money;
        }
        private void ValidateErrors(User newUser, ref string errors)
        {
            if (newUser.Name == null)
                //Validate if Name is null
                errors = "The name is required";
            if (newUser.Email == null)
                //Validate if Email is null
                errors = errors + " The email is required";
            if (newUser.Address == null)
                //Validate if Address is null
                errors = errors + " The address is required";
            if (newUser.Phone == null)
                //Validate if Phone is null
                errors = errors + " The phone is required";
        }
    }
}
