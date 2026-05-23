using BackendAromaseoLimpieza.Interfaces;
using BackendAromaseoLimpieza.Models.Users;
using Dapper;
using Npgsql;

namespace BackendAromaseoLimpieza.Services;

public class UserService : IUserService
{
   private readonly IConfiguration _configuration;
   private readonly string _connectionString;
   
   public UserService(IConfiguration configuration)
   {
      _configuration = configuration;
      _connectionString = _configuration.GetConnectionString("DefaultConnection");
   }
   
   public async Task<Result<string, int>> CreateUser(User user)
   {
      try
      {
         await using var connection = new NpgsqlConnection(_connectionString);
         await connection.OpenAsync();
         
         string insertUser = @"
            insert into usuario (nombres, apellidos, empresa, documento, correo, contrasena, telefono, ubicacion, rol_id)
            values (@nombres, @apellidos, @empresa, @documento, @correo, @contrasena, @telefono, @ubicacion, @rol_id);
         ";
         
         string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password ?? user.Document ?? user.Phone);
         
         var insert = connection.ExecuteAsync(insertUser, new
         {
            nombres = user.Names,
            apellidos = user.LastNames,
            empresa = user.Company,
            documento = user.Document,
            correo = user.Email,
            contrasena = passwordHash,
            telefono = user.Phone,
            ubicacion = user.Location,
            rol_id = user.RoleId
         });

         await connection.DisposeAsync();
         
         if (insert.Result > 0)
            return Result<string, int>.Success("User created successfully", 201);
         
         return Result<string, int>.Failure("Error to create new user", 500);
      }
      catch (Exception e)
      {
         throw new Exception("Error to create new user ", e);
      }
   }

   public async Task<Result<User, int>> GetUserById(int id)
   {
      try
      {
         await using var connection = new NpgsqlConnection(_connectionString);
         await connection.OpenAsync();

         string query = @"
            select
               u.id as Id,
               u.nombres as Names,
               u.apellidos as LastNames,
               u.empresa as Company,
               u.documento as Document,
               u.correo as Email,
               u.telefono as Phone,
               u.ubicacion as Location
            from usuario u where id = @Id;
         ";
         
         var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { Id = id });
         
         await connection.DisposeAsync();
         
         if (user != null)
            return Result<User, int>.Success(user, 200);
         
         return Result<User, int>.Failure("User not found", 404);
      }
      catch (Exception e)
      {
         throw new Exception("Error to get user by id", e);
      }
   }

   public async Task<Result<string, int>> UpdateUser(User user)
   {
      try
      {
         await using var connection = new NpgsqlConnection(_connectionString);
         await connection.OpenAsync();
         
         string updateUser = @"
              update usuario
            set
               nombres = @Names,
               apellidos = @LastNames,
               empresa = @Company,
               documento = @Document,
               correo = @Email,
               telefono = @Phone,
               ubicacion = @Location
            where id = @Id;
         ";
         
         var result = await connection.ExecuteAsync(updateUser, user);
         
         await connection.DisposeAsync();
         
         if (result == 0)
            return Result<string, int>.Failure("User not found", 404);
         
         return Result<string, int>.Success("User updated successfully", 200);
      }
      catch (Exception e)
      {
         throw new Exception("Error to update user", e);
      }
   }

   public async Task<Result<object, int>> GetUsers(int page, int pageSize)
   {
      try
      {
         await using var connection = new NpgsqlConnection(_connectionString);
         await connection.OpenAsync();

         var query = @"
            select count(1) from usuario;

            select
                u.id as Id,
                u.nombres as Names,
                u.apellidos as LastNames,
                u.empresa as Company,
                u.documento as Document,
                u.correo as Email,
                u.telefono as Phone,
                u.ubicacion as Location
            from usuario u
            order by id desc
            OFFSET (@Page - 1) * @PageSize
            LIMIT @PageSize;
        ";

         var result = await connection.QueryMultipleAsync(query, new
         {
            Page = page,
            PageSize = pageSize
         });

         var numberOfRecords = result.Read<int>().Single();

         var collection = result.Read<User>().ToList();

         return Result<object, int>.Success(new
         {
            data = collection,
            total = numberOfRecords
         }, 200);
      }
      catch (Exception e)
      {
         throw new Exception("Error to get users", e);
      }
   }
   
}