﻿using LexiconLMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Data
{
    public class LexiconLMSContext : IdentityDbContext<User>
    {
        public LexiconLMSContext(DbContextOptions<LexiconLMSContext> options) : base(options)
        {
        }

        /*** Properties of DbSets for entities that you want to interact with directly ***/

        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Course>();
        }

    }
}