using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comments;
using api.Models;

namespace api.Mappers
{
    public static class CommentsMapper
    {
        public static CommentsDto ToCommentsDto(this Comments comments)
        {
            return new CommentsDto
            {
                Id = comments.Id,
                Title = comments.Title,
                Content = comments.Content,
                CreatedOn = comments.CreatedOn,
                StockId = comments.StockId,
            };
        }
        public static Comments ToCommentsFromCreate(this CreateCommentDto createComment,int stockId)
        {
            return new Comments
            {
                StockId = stockId,
                Title = createComment.Title,
                Content = createComment.Content
            };
        }
    }
}