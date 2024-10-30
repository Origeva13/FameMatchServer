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

}

