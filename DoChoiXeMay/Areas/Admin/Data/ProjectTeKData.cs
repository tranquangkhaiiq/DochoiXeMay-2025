using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;

namespace DoChoiXeMay.Areas.Admin.Data
{
    public class ProjectTeKData
    {
        Model1 _context;
        public ProjectTeKData()
        {
            _context = new Model1();
        }
        public List<ProjectTeK> GetListProjectTeK()
        {
            var model = _context.ProjectTeKs
                    .OrderBy(kh => kh.Id)
                    .ToList();
            for (int i = 0; i < model.Count(); i++)
            {
                model[i].GhiChu = (i + 1).ToString();
            }
            model = model
                .OrderByDescending(kh => kh.Id)
                .ToList();
            return model;
        }
        public bool UPdateProjectTeK(ProjectTeK p)
        {
            try
            {
                _context.Entry(p).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool UPdateProjectDetail(ProjectDetail p)
        {
            try
            {
                _context.Entry(p).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool InsertProjecDetail(int Userthamgia, int ProjectId, bool Leader)
        {
            try
            {
                ProjectDetail p = new ProjectDetail();
                p.Id = Guid.NewGuid();
                p.UserId = Userthamgia;
                p.TrangthaiId = 1;
                p.NgayBatDau = DateTime.Now;
                p.NgayUpdate = DateTime.Now;
                p.ProjectId = ProjectId;
                p.Ghichu = "Chưa update.";
                p.Leader = Leader;
                _context.ProjectDetails.Add(p);
                int kt = _context.SaveChanges();
                if (kt > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool InsertProjecUserDetail(string CV, string DetailId)
        {
            try
            {
                var detailId = new Guid(DetailId);
                var ProjectDetail = _context.ProjectDetails.Find(detailId);
                ProjectUserDetail model = new ProjectUserDetail();
                model.Id = Guid.NewGuid();
                model.ProjectDetailId = detailId;
                model.CongViec = CV;
                model.TrangthaiId = 1;
                model.NgayUpdate = DateTime.Now;
                _context.ProjectUserDetails.Add(model);
                var kq = _context.SaveChanges();
                if (kq > 0) {
                    var project = _context.ProjectTeKs.Find(ProjectDetail.ProjectId);
                    if (project != null && project.TrangthaiId==5) {
                        project.TrangthaiId = 2;
                        project.NgayCapNhat = DateTime.Now;
                        _context.Entry(project).State = EntityState.Modified;
                        _context.SaveChanges();
                        var kqp = UpdatetrangthaiProDetail(project.Id);
                    }
                    return true;
                }return false;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public List<ProjectDetail> getlistProjectDetail(int Id)
        {
            var model = _context.ProjectDetails.Where(kh => kh.ProjectId == Id).ToList();
            return model;
        }
        public List<ProjectUserDetail> getlistProjectUserDetail(string Id)
        {
            var PUD = new Guid(Id);
            var model = _context.ProjectUserDetails.Where(kh => kh.ProjectDetailId == PUD).ToList();
            return model;
        }
        public ProjectDetail getProjectDetail(int project,int user)
        {
            var model = _context.ProjectDetails.FirstOrDefault(kh => kh.ProjectId == project 
                        && kh.UserId==user);
            return model;
        }
        public int getCountCVProject(int project) {
            int count = 0;
            var getProjectDetail = _context.ProjectDetails.Where(kh => kh.ProjectId == project).ToList();
            if (getProjectDetail.Count() > 0)
            {
                foreach (var item in getProjectDetail)
                {
                    var getCount1User = _context.ProjectUserDetails.Where(kh => kh.ProjectDetailId == item.Id).ToList();
                    count += getCount1User.Count();
                }
            }
            return count;
        }
        public int getCountCVProjectHoanThanh(int project)
        {
            int count = 0;
            var getProjectDetail = _context.ProjectDetails.Where(kh => kh.ProjectId == project).ToList();
            if (getProjectDetail.Count() > 0)
            {
                foreach (var item in getProjectDetail)
                {
                    var getCount1User = _context.ProjectUserDetails.Where(kh => kh.ProjectDetailId == item.Id && kh.TrangthaiId == 5).ToList();
                    count += getCount1User.Count();
                }
            }
            return count;
        }
        public bool UpdatetrangthaiProDetail(int pro)
        {
            var project = _context.ProjectTeKs.Find(pro);
            var ProjectDetailss = _context.ProjectDetails.Where(kh => kh.ProjectId == pro).ToList();
            if (ProjectDetailss.Count() > 0)
            {
                foreach (var item in ProjectDetailss)
                {
                    var pjd = _context.ProjectDetails.Find(item.Id);
                    if (pjd.TrangthaiId != project.TrangthaiId)
                    {
                        pjd.TrangthaiId = project.TrangthaiId;
                        pjd.NgayUpdate = DateTime.Now;
                        _context.Entry(pjd).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
            }
            return true;
        }
        public int updatephantramht(int pro)
        {
            int count = 0;
            var teK = _context.ProjectTeKs.Find(pro);
            int getCVTeK = new Data.ProjectTeKData().getCountCVProject(teK.Id);
            int getCVTeKOK = new Data.ProjectTeKData().getCountCVProjectHoanThanh(teK.Id);
            teK.PhantramHoanThanh = int.Parse(Math.Ceiling((1.0 * getCVTeKOK / getCVTeK) * 100).ToString());
            if (teK.PhantramHoanThanh == 100)
            {
                teK.TrangthaiId = 5;
            }
            _context.Entry(teK).State = EntityState.Modified;
            _context.SaveChanges();
            count = teK.PhantramHoanThanh;
            return count;
        }
    }
}