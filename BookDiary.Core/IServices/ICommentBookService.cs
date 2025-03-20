﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Models;

namespace BookDiary.Core.IServices
{
    public interface ICommentBookService:IService<CommentBook>
    {
        //Task<IEnumerable<CommentBook>> GetAllCommentBooks();
        Task DeleteBookComment(int bookid, int comid);

    }
}
