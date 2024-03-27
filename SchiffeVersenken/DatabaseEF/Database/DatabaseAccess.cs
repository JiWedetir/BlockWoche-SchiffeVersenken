using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchiffeVersenken.DatabaseEF.Database;
using SchiffeVersenken.DatabaseEF.Models;
using System.Diagnostics;
using System.Xml.Linq;
using Windows.System;

namespace SchiffeVersenken.DatabaseEF.Database
{
    internal class DatabaseAccess
    {
        private static readonly DatabaseContext _context = new DatabaseContext(new DbContextOptions<DatabaseContext>());

        protected static AutoMapper.Mapper _mapper = AutoMapperConfig.InitializeAutomapper();

        /// <summary>
        /// Creates the default users if they don't exist.
        /// </summary>
        internal static async Task CreateDefaultUsers()
        {
            try
            {
                var user = new UserEF() { Name = "Player", PasswordHash = "1234", Salt = "1234" };
                var dumm = new UserEF() { Name = "Dummer_Computer", PasswordHash = "1234", Salt = "1234" };
                var klug = new UserEF() { Name = "Kluger_Computer", PasswordHash = "1234", Salt = "1234" };
                var genial = new UserEF() { Name = "Genialer_Computer", PasswordHash = "1234", Salt = "1234" };

                await _context.Users.AddRangeAsync(new List<UserEF>() { user, dumm, klug, genial });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a list of user names from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of user names.</returns>
        internal static async Task<List<string>> GetUserNamesAsync()
        {
            try
            {
                 var users = await _context.Users
                    .Where(i => i.Name != "Player" && i.Name != "Dummer_Computer" && i.Name != "Kluger_Computer" && i.Name != "Genialer_Computer")
                    .Select(i => i.Name )
                    .ToListAsync();

                return users!;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new List<string>();
			}
        }

        /// <summary>
        /// Retrieves a user from the database based on their name.
        /// </summary>
        /// <param name="name">The name of the user to retrieve.</param>
        /// <returns>The user object if found, otherwise null.</returns>
        internal static async Task<User> GetUserAsync(string name)
        {
            try
            {
                User user = await _context.Users
                    .Where(i => i.Name == name)
                    .ProjectTo<User>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync() ?? new User();

                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new User();
            }
        }

        /// <summary>
        /// Saves a user asynchronously.
        /// </summary>
        /// <param name="user">The user to save.</param>
        /// <returns>The number of rows affected in the database.</returns>
        internal static async Task<bool> SaveUserAsync(User user)
        {
            try
            {
                if (user.Id != 0)
                {
                    var userToFind = await _context.Users.FindAsync(user.Id);
                    if (userToFind != null)
                    {
                        _mapper.Map(user, userToFind);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
                else
                {
                    var userEF = _mapper.Map<UserEF>(user);
                    await _context.Users.AddAsync(userEF);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Deletes a user from the database asynchronously.
        /// </summary>
        /// <param name="user">The user to be deleted.</param>
        /// <returns>The number of rows affected in the database.</returns>
        internal static async Task<bool> DeleteUserAsync(User user)
        {
            try
            {
                var userToRemove = await _context.Users.FindAsync(user.Id);
                if (userToRemove != null)
                {
                    _context.Users.Remove(userToRemove);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Retrieves the user scores for a given username.
        /// </summary>
        /// <param name="username">The username to retrieve scores for.</param>
        /// <returns>A list of UserScore objects representing the user scores.</returns>
        internal static async Task<List<UserScoreView>> GetUserScoreAsync(string username)
        {
            try
            {
                var user = await _context.Users
                    .Where(i => i.Name == username)
                    .ProjectTo<User>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    List<UserScoreView> scores = await _context.HighScores
                        .Where(i => i.UserId == user.Id)
                        .ProjectTo<UserScoreView>(_mapper.ConfigurationProvider)
                        .ToListAsync();
                    return scores;
                }
                return new List<UserScoreView>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new List<UserScoreView>();
            }
        }

        /// <summary>
        /// Updates the scores in the database with the provided highscore.
        /// </summary>
        /// <param name="highscore">The highscore to be updated.</param>
        /// <returns>The number of rows affected in the database.</returns>
        internal static async Task<bool> UpdateScoresAsync(Highscore highscore)
        {
            try
            {
                var highScoreEF = _mapper.Map<HighScoreEF>(highscore);
                await _context.HighScores.AddAsync(highScoreEF);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
