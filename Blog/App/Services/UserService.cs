﻿using Blog.App.Database.Entities;
using Blog.App.Repositories;
using Blog.App.Services.Sessions;
using JWT.Algorithms;
using JWT.Builder;

namespace Blog.App.Services;

public class UserService
{
    private readonly UserRepository UserRepository;
    private readonly IdentityService IdentityService;

    private readonly string JwtSecret;

    public UserService(UserRepository userRepository, ConfigService configService, IdentityService identityService)
    {
        UserRepository = userRepository;
        IdentityService = identityService;

        JwtSecret = configService.Config.JwtSecret;
    }

    public async Task<(string, User)> Register(string email, string password, string fullName, string username)
    {
        var emailTaken = UserRepository.Get().FirstOrDefault(x => x.Email == email) != null;
        var usernameTaken = UserRepository.Get().FirstOrDefault(x => x.Username == username) != null;

        if (emailTaken | usernameTaken)
        {
            throw new Exception("The email/username is already in use");
        }
        
        var user = UserRepository.Add(new()
        {
            Email = email,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            FullName = fullName,
            Username = username,
            CreatedAt = DateTime.UtcNow,
            TokenValidTime = DateTime.Now.AddDays(-5)
        });
        
        return (await GenerateToken(user), user);
    }
    
    public async Task<bool> CheckAccount(string email, string password)
    {
        var user = UserRepository.Get()
            .FirstOrDefault(
                x => x.Email == email
            );

        if (user == null)
        {
            throw new Exception("Email and password combination not found");
        }

        if (BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return true;
        }

        throw new Exception("Email and password combination not found");;
    }
    
    public bool CheckAccountSync(string email, string password)
    {
        var user = UserRepository.Get()
            .FirstOrDefault(
                x => x.Email == email
            );

        if (user == null)
        {
            throw new Exception("Email and password combination not found");
        }

        if (BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return true;
        }

        throw new Exception("Email and password combination not found");;
    }
    public async Task<string> Login(string email, string password)
    {
        var user = UserRepository.Get()
            .FirstOrDefault(
                x => x.Email.Equals(
                    email
                )
            );
        
        if(await CheckAccount(email, password))
            return await GenerateToken(user!, true);
        throw new Exception();
    }
    public User LoginUsernameSync(string username, string password)
    {
        var user = UserRepository.Get()
            .FirstOrDefault(
                x => x.Username.Equals(
                    username
                )
            );
        
        if(CheckAccountSync(user.Email, password))
            return user;
        
        throw new Exception();
    }

    public async Task ChangePassword(User user, string password, bool isSystemAction = false)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(password);
        user.TokenValidTime = DateTime.Now;
        UserRepository.Update(user);
    }

    public async Task<string> GenerateToken(User user, bool sendMail = false)
    {
        var token = JwtBuilder.Create()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(JwtSecret)
            .AddClaim("exp", DateTimeOffset.UtcNow.AddDays(10).ToUnixTimeSeconds())
            .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            .AddClaim("userid", user.Id)
            .Encode();

        return token;
    }
}