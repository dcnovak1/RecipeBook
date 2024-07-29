﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.ServiceLibrary.Entities
{
    public class RecipeEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public string Logo { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public IList<IngredientsEntity> Ingredients { get; set; } = new List<IngredientsEntity>();
        public IList<InstructionsEntity> Instructions { get; set; } = new List<InstructionsEntity>();
    }
}
