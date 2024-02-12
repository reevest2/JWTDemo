using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTDemo;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("/Register")]
    public async Task<IActionResult> Register([FromBody] UserRequestDTO userRequestDTO)
    {
        if (userRequestDTO == null || !ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = new IdentityUser
        {
            UserName = userRequestDTO.Email,
            Email = userRequestDTO.Email,
            PhoneNumber = userRequestDTO.PhoneNumber,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, userRequestDTO.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new RegistrationResponseDTO
            {
                Errors = errors,
                IsSuccessful = false
            });
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "user");

        if (!roleResult.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new RegistrationResponseDTO
            {
                Errors = errors,
                IsSuccessful = false
            });
        }

        return StatusCode(201);
    }

    [HttpPost("/SignIn")]
    public async Task<IActionResult> SignIn([FromBody] AuthenticationDTO authenticationDTO)
    {
        var user = await _userManager.FindByNameAsync(authenticationDTO.UserName);

        if (user == null || !await _userManager.CheckPasswordAsync(user, authenticationDTO.Password))
        {
            return Unauthorized(); // It's better to return Unauthorized instead of Ok(null) for failed auth
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("abcdefghijklmonpqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567891011121314151617181920"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "user"),
                // Add other claims here as needed
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { token = tokenHandler.WriteToken(token) });
    }
}