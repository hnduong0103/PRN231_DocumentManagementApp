﻿using DataAccess;
using DataAccess.DBModels;
using DMAService;
using DMAWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMAWebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        public AuthController(DMSDatabaseContext context)
        {
            _authService = new AuthService(context);
        }
        /*
         * SHOW LOGIN FORM
         */
        public IActionResult Index()
        {
            return View();
        }

        /*
         * USER LOGIN
         */
        [HttpPost]
        public IActionResult Login(UserLogin user)
        {
            if (ModelState.IsValid)
            {
                var _user = _authService.CheckCredential(user);
                if (_user == null)
                {
                    TempData["error"] = "Credentials do not match.";
                    return RedirectToAction("Index", "Auth");
                }
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, _user[0].UserName.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Role, _user[0].UserRole));
                identity.AddClaim(new Claim(ClaimTypes.Email, _user[0].UserEmail));
                HttpContext.Session.SetString("UserEmail", _user[0].UserEmail);
                HttpContext.Session.SetInt32("UserId", _user[0].UserId);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                if (_user[0].UserRole == "Admin")
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                    return RedirectToAction("Index", "Home");
                }
                else if (_user[0].UserRole == "User")
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
                    return RedirectToAction("Index", "Home");
                }

            }
            TempData["error"] = "Credentials do not match.";
            return RedirectToAction("Index", "Auth");
        }

        /*
         * LOGOUT
         */
        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Auth");
        }
        public IActionResult Forbidden()
        {
            TempData["error"] = "Permissin Denied!";
            return RedirectToAction("Index", "Auth");
        }

        /*
         * ERROR PAGE
         */
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
