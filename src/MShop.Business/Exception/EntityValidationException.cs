﻿using MShop.Business.Validation;

namespace MShop.Business.Exceptions
{
    public class EntityValidationException: Exception
    {
       
        public EntityValidationException(
            string message
            ):base(message) 
        {
        }  
    }


}