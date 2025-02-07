﻿using Dvchevskii.Blog.Core.Common;
using Dvchevskii.Blog.Core.Files;

namespace Dvchevskii.Blog.Core.Posts;

public class Post : EntityBase
{
    public string Title { get; set; }
    public string? Tagline { get; set; }
    public string? Body { get; set; }
    public bool IsDraft { get; set; }
    public int? HeaderImageId { get; set; }
    public Image? HeaderImage { get; set; }
}
