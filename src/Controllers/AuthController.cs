using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using api.src.Dtos.Auth;
using api.src.Interfaces;
using api.src.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace api.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto){
            {
                try
                {
                    if(!ModelState.IsValid) return BadRequest(ModelState);

                    var appUser = new AppUser
                    {
                        UserName = registerDto.UserName,
                        Email = registerDto.Email,
                    };

                    if(string.IsNullOrEmpty(registerDto.Password))
                    {
                        return BadRequest("Password cannot be null or empty");
                    }

                    var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                    if(createUser.Succeeded)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                        if(roleResult.Succeeded){
                            return Ok(
                                new NewUserDto
                                {
                                    Username = appUser.UserName,
                                    Email = appUser.Email,
                                    Token = _tokenService.CreateToken(appUser)
                                }
                            );
                        }
                        else
                        {
                            return StatusCode(500, roleResult.Errors);
                        }
                    }
                    else
                    {
                        return StatusCode(500, createUser.Errors);
                    }


                }
                catch(Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto){
            {
                try
                {
                    if(!ModelState.IsValid) return BadRequest(ModelState);

                    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);
                    if(user == null) return Unauthorized("Invalid username or password");

                    var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                    if(!result.Succeeded) return Unauthorized("Invalid password or Username");

                    return Ok(
                        new NewUserDto
                        {
                            Username = user.UserName!,
                            Email = user.Email!,
                            Token = _tokenService.CreateToken(user)
                        }
                    );
                }
                catch(Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
        }
        
    }
}