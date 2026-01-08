using Microsoft.AspNetCore.Mvc;
using RSS_Feeds.MVC.Models;
using System.Net.Http.Json;

namespace RSS_Feeds.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _http;

        public AccountController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // ---------- LOGIN ----------
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                model.ErrorMessage = "Email y contraseña son requeridos";
                return View(model);
            }

            var response = await _http.PostAsJsonAsync(
                "api/ServiceLocator/auth/login",
                new
                {
                    email = model.Email,
                    password = model.Password
                }
            );

            if (!response.IsSuccessStatusCode)
            {
                model.ErrorMessage = "Credenciales inválidas";
                return View(model);
            }

            var user = await response.Content
                .ReadFromJsonAsync<LoginResultViewModel>();

            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserNombre", user.Nombre ?? "");

            return RedirectToAction("Index", "Home");
        }

        // ---------- LOGOUT ----------
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ---------- REGISTER ----------
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                model.ErrorMessage = "Todos los campos son obligatorios";
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                model.ErrorMessage = "Las contraseñas no coinciden";
                return View(model);
            }

            var response = await _http.PostAsJsonAsync(
                "api/ServiceLocator/auth/register",
                new
                {
                    nombre = model.Nombre,
                    email = model.Email,
                    password = model.Password,
                    confirmPassword = model.ConfirmPassword
                }
            );

            if (!response.IsSuccessStatusCode)
            {
                model.ErrorMessage = "No se pudo registrar (email ya existe)";
                return View(model);
            }

            return RedirectToAction("Login");
        }
    }

}
