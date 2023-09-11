﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Dolphin_Book.Core.Entities.BaseEntities
{
	public class BaseEntity
	{
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

