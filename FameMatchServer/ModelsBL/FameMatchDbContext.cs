using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FameMatchServer.Models;

public partial class FameMatchDbContext : DbContext
{
    public Casted? GetCasted(string email)
    {
        User? u = Users.Include(u => u.Files).Where(x => x.UserEmail == email).FirstOrDefault();
        if (u == null)
            return null;
        else
            return this.Casteds.Where(s => s.UserId == u.UserId)
                               .Include(s => s.User).ThenInclude(u => u.Files).FirstOrDefault();
    }
    public Castor? GetCastor(string email)
    {
        User? u = Users.Include(u => u.Files).Where(x => x.UserEmail == email).FirstOrDefault();
        if (u == null)
            return null;
        else
            return this.Castors.Where(s => s.UserId == u.UserId).Include(s => s.User).ThenInclude(u => u.Files).FirstOrDefault();
    }
    public User? GetUser(string email)
    {
        return this.Users.Include(u=>u.Files).Where(u => u.UserEmail == email)
                            .FirstOrDefault();
    }
    public Audition? GetAudition(int id)
    {
        return this.Auditions.Where(a => a.AudId == id)
                            .FirstOrDefault();
    }
    public List<User> GetUsers()
    {
        return this.Users.Include(u => u.Files).ToList();
    }
    public Models.User? GetUser1(int id)
    {
        return this.Users.Include(u => u.Files).Where(u => u.UserId == id)
                            .FirstOrDefault();
    }
    public List<string> GetAllEmails()
    {
        return this.Users.Include(u => u.Files).Select(u => u.UserEmail).ToList();
    }
    public List<Audition> GetAllAuditions()
    {
        return this.Auditions.ToList();
    }
    public Models.User? GetUserEmail(string email)
    {
        return this.Users.Include(u => u.Files).Where(u => u.UserEmail == email)
                            .FirstOrDefault();
    }

    public Models.Castor? GetUserByAudition(int auditionId)
    {
        Castor? c= this.Castors.Include(u=>u.User).ThenInclude(u => u.Files).Where(u => u.Auditions.Where(c=>c.AudId==auditionId).Any())
                            .FirstOrDefault();
        return c;
    }
}

