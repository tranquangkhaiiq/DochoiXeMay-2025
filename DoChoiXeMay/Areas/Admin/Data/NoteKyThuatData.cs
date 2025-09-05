using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DoChoiXeMay.Areas.Admin.Data
{
    public class NoteKyThuatData
    {
        Model1 _context;
        public NoteKyThuatData()
        {
            _context = new Model1();
        }
        public List<NoteKythuat> GetListNotebyHD(int loai)
        {
            var model = _context.NoteKythuats.Where(kh => kh.LoaiNoteId==loai)
                    .OrderBy(kh => kh.Id)
                    .ToList();
            for (int i = 0; i < model.Count(); i++)
            {
                model[i].Stt = (i + 1).ToString();
            }
            model = model
                .OrderByDescending(kh => kh.Id)
                .ToList();
            return model;
        }
        public List<NoteKythuat> Get1ListNotebyHD(int loai)
        {
            var modle = _context.NoteKythuats.Where(kh => kh.LoaiNoteId == loai).ToList();
            if(modle != null)
            {
                modle = _context.NoteKythuats.Where(kh => kh.LoaiNoteId==loai)
                    .OrderByDescending(kh => kh.Id)
                    .Take(1)
                    .ToList();
                return modle;
            }
            return null;
        }
    }
}