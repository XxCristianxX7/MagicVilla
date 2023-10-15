using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_API.Datos;
using System.Collections.Immutable;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MagicVilla_API.Repositorio.IRepositorio;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MagicVilla_API.Controllers
{
    
    [Route("swagger/v1/[controller]")]
    [ApiController]
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly INumeroVillaRepositorio _numeroRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepo, INumeroVillaRepositorio numeroRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _numeroRepo = numeroRepo;
            _mapper = mapper;
            _response = new();
        }

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //Anotación Authorize para proteger este endpoint con JWT
        [Authorize]
        public async Task<ActionResult<APIResponse>> GetNumeroVillas()
        {

            try
            {

                _logger.LogInformation("Obtener Numero Villas");

                IEnumerable<NumeroVilla> numeroVillaList = await _numeroRepo.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numeroVillaList);
                _response.statusCode = HttpStatusCode.OK;

                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("id:int", Name ="GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
        {

            try
            {

                if (id == 0)
                {
                    _logger.LogError("Error al traer la Villa con Id " + id);
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }

                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var numeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id);

                if (numeroVilla == null)
                {
                    _response.statusCode=HttpStatusCode.NotFound;
                    _response.IsExitoso=false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<NumeroVillaDto>(numeroVilla);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
            
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearNumeroVilla([FromBody] NumeroVillaCreateDto createDto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //if (VillaStore.villaList.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
                if (await _numeroRepo.Obtener(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("NombreExiste", "El numero de villa ya existe");
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(v=>v.Id==createDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El id de la Villa no existe");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto);

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                await _numeroRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = modelo.VillaNo }, _response);

            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var numeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id);
                if (numeroVilla == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                //VillaStore.villaList.Remove(villa);

                _numeroRepo.Remover(numeroVilla);

                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return BadRequest(_response);

        }




        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto)
        {
            if(updateDto == null || id!= updateDto.VillaNo)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (await _villaRepo.Obtener(v => v.Id == updateDto.VillaId) == null)
            {
                ModelState.AddModelError("ClaveForanea", "El id de la Villa no existe");
                return BadRequest(ModelState);
            }

            NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateDto);

            await _numeroRepo.Actualizar(modelo);
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
       

        [HttpGet("GenerarToken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GenerarToken(string usuario, string contrasena)
        {
            //Validación de usuario y contraseña
            if (ValidarUsuario(usuario, contrasena))
            {
                //Clave para asignar en el token en el espacio signingCredentials
                var claveSecreta = "claveDePrueba123claveDePrueba123";
                var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveSecreta));
                var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
                //Alcances que formarán parte del JWT y que servirán también para la autenticación si así lo modificamos
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, usuario),
                };

                // Configurar el token JWT
                var token = new JwtSecurityToken(
                    issuer: "issuerDePrueba",
                    audience: "audienceDePrueba",
                    claims: claims,
                    expires: DateTime.Now.AddHours(1), // Configura la expiración del token
                    signingCredentials: credenciales //Credencial asignada anteriormente
                );

                // Generar el token como una cadena JWT
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                // Devolver el token JWT como respuesta 
                return Ok(new { Token = tokenString });
            }
            else
            {
                // Si no encuentra el usuario y contraseña devuelve un BadRequest
                return BadRequest("Usuario y/o contraseña incorrectos");
            }
        }

        //Método para validar usuario y contraseña
        private bool ValidarUsuario(string usuario, string contra)
        {
            String uss = "Usuario";
            String cont = "Contrasea";
            if (usuario == uss && contra == cont)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
