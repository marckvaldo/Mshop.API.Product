﻿namespace MShop.ProductAPI.Extension
{
    public static class ExtensionResponse
    {
        public static object Success(object result)
        {
            return new
            {
                success = true,
                data = result
            };
        }

        public static object Error(object result)
        {
            return new
            {
                success = false,
                errors = result
            };
        }
    }
}
