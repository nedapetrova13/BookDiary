﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Models;

namespace BookDiary.Core.IServices
{
    public interface IBookService:IService<Book>
    {
        Task AddTagToBook(string bookname,string tagname);
    }
}
