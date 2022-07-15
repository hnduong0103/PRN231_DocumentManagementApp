using DataAccess;
using DataAccess.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMAService
{
    public class ProjectService
    {
        private readonly DMSDatabaseContext _context;
        public ProjectService(DMSDatabaseContext db)
        {
            _context = db;
        }

        /*
         * GET LIST OF PROJECT
         */
        public async Task<IQueryable<ProjectViewModel>> GetAll(string email, string searchStr = null)
        {
            var user = _context.Users.Where(x => x.UserEmail == email).FirstOrDefault();
            //var projects = _context.Projects.Where(x => x.UsersUser.UserId == user.UserId).ToList();
            var projects = from x in _context.Projects
                        where x.UsersUserId == user.UserId
                        select new ProjectViewModel
                        {
                            ProjectId = x.ProjectId,
                            ProjectName = x.ProjectName
                        };

            if (!String.IsNullOrEmpty(searchStr))
            {
                projects = from x in projects
                           where x.ProjectName.Contains(searchStr)
                           select new ProjectViewModel
                           {
                               ProjectId = x.ProjectId,
                               ProjectName = x.ProjectName
                           };
            }
            return projects.AsQueryable();
        }

        /*
         * CREATE PROJECT
         */
        public bool CreateProject(Project Proj, string email)
        {
            bool status;
            var user = _context.Users.Where(x => x.UserEmail == email).FirstOrDefault();
            Project item = new Project();
            item.ProjectName = Proj.ProjectName;
            item.UsersUserId = user.UserId;

            try
            {
                _context.Projects.Add(item);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                var exp = ex;
                status = false;
            }
            return status;
        }

        /*
         * DELETE PROJECT
         */
        public bool DeleteProject(int id)
        {
            bool status;
            var item = _context.Projects.Find(id);

            try
            {
                _context.Projects.Remove(item);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                var exp = ex;
                status = false;
            }
            return status;
        }
    }
}
