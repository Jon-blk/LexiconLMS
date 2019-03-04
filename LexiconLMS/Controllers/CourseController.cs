﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LexiconLMS.Data;
using LexiconLMS.Models;
using LexiconLMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LexiconLMS.Controllers
{

    [Authorize(Roles = "Teacher")]
    public class CourseController : Controller
    {
        private readonly LexiconLMSContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CourseController(LexiconLMSContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var courses = _context.Courses;
            var viewModels = await courses.ProjectTo<CourseListViewModel>(_mapper.ConfigurationProvider).ToListAsync();

            //List<CourseListViewModel> viewModel = new List<CourseListViewModel>();
            return View(viewModels);
        }

        public async Task<IActionResult> Add()
        {
            var teachers = await _userManager.GetUsersInRoleAsync("Teacher");

            AddCourseViewModel viewModel = new AddCourseViewModel
            {
                Teachers = teachers.Select(t => new Tuple<string, string>(t.Id, t.UserName)).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCourseViewModel viewModel)
        {
            
            if (ModelState.IsValid)
            {
                var teacher = _userManager.Users.FirstOrDefault(u => u.Id == viewModel.TeacherId);
                Course course = _mapper.Map<Course>(viewModel);
                course.Teacher = teacher;
                await _context.Courses.AddAsync(course);
                _context.SaveChanges();

                return RedirectToAction(nameof(Details), new { course.Id });
            }

            return View();
        }

        public IActionResult Details(int id)
        {
            var course = _context.Courses.Include(c => c.Teacher).FirstOrDefault(c => c.Id == id);

            if(course is null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<CourseDetailsViewModel>(course);

            return View(viewModel);
        }
    }
}