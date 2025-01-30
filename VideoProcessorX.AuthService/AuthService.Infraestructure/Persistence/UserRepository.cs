﻿using AuthService.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using VideoProcessorX.Domain.Entities;
using VideoProcessorX.Domain.Interfaces;

namespace AuthService.Infraestructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
