using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FameMatchServer.Models;

public partial class FameMatchDbContext : DbContext
{
    public Casted? GetCasted(string email)
    {
        User? u = Users.Where(x => x.UserEmail == email).FirstOrDefault();
        if (u == null)
            return null;
        else
            return this.Casteds.Where(s => s.UserId == u.UserId)
                               .Include(s => s.User).FirstOrDefault();
    }
    public Castor? GetCastor(string email)
    {
        User? u = Users.Where(x => x.UserEmail == email).FirstOrDefault();
        if (u == null)
            return null;
        else
            return this.Castors.Where(s => s.UserId == u.UserId).Include(s => s.User).FirstOrDefault();
    }
    public User? GetUser(string email)
    {
        return this.Users.Where(u => u.UserEmail == email)
                            .FirstOrDefault();
    }
    public List<User> GetUsers()
    {
        return this.Users.ToList();
    }
    public Models.User? GetUser1(int id)
    {
        return this.Users.Where(u => u.UserId == id)
                            .FirstOrDefault();
    }

}

